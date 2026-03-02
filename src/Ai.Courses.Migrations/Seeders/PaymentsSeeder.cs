using Ai.Courses.Data;
using Ai.Courses.Data.Contexts;
using Ai.Courses.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ai.Courses.Migrations.Seeders;

public static class PaymentsSeeder
{
    private const string AdminEmail = "admin@test.com";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<PaymentDbContext>();
        var userManager = services.GetRequiredService<UserManager<UserEntity>>();
        var logger = services.GetRequiredService<ILogger<PaymentDbContext>>();

        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            logger.LogWarning("Admin user not found. Skipping payments seed.");
            return;
        }

        var userId = Guid.Parse(admin.Id);

        if (db.Items.Any(i => i.UserId == userId))
        {
            logger.LogInformation("Payments data already seeded. Skipping.");
            return;
        }

        var typeNames = new[] { "Product", "Other", "Entertainment" };
        var types = typeNames.Select(name => new TypeEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name
        }).ToList();

        await db.Types.AddRangeAsync(types);
        await db.SaveChangesAsync();

        var rng = new Random(42);

        var allItems = new List<ItemEntity>();

        // February 2026 — all spent
        allItems.AddRange(BuildMonthItems(userId, types, 2026, 2, rng, allPlanned: false, allSpent: true));
        // March 2026 — Product/Other = planned, Entertainment = spent
        allItems.AddRange(BuildMonthItems(userId, types, 2026, 3, rng, allPlanned: false, allSpent: false));
        // April 2026 — all planned
        allItems.AddRange(BuildMonthItems(userId, types, 2026, 4, rng, allPlanned: true, allSpent: false));

        await db.Items.AddRangeAsync(allItems);
        await db.SaveChangesAsync();

        logger.LogInformation("Payments seed data applied: {ItemCount} items, {PaymentCount} payments.",
            allItems.Count,
            allItems.Sum(i => i.Payments.Count));
    }

    // Schedule rules:
    //   Product      — every 3rd day  — planned 50–200
    //   Other        — every 6th day  — planned 30–50
    //   Entertainment — days 5,10,15,20,25 — spent 250–1000
    //
    // allPlanned: override all payments to PlannedAmount   (April)
    // allSpent  : override all payments to SpentAmount     (February)
    // neither   : Product/Other → planned, Entertainment → spent  (March)
    private static List<ItemEntity> BuildMonthItems(
        Guid userId,
        List<TypeEntity> types,
        int year,
        int month,
        Random rng,
        bool allPlanned,
        bool allSpent)
    {
        TypeEntity TypeByName(string name) => types.First(t => t.Name == name);

        var daysInMonth = DateTime.DaysInMonth(year, month);

        var productDays       = Enumerable.Range(1, daysInMonth).Where(d => d % 3 == 0);
        var otherDays         = Enumerable.Range(1, daysInMonth).Where(d => d % 6 == 0);
        var entertainmentDays = new[] { 5, 10, 15, 20, 25 }.Where(d => d <= daysInMonth);

        // Collect payments per day (multiple types can share the same day)
        var dayPayments = new Dictionary<int, List<(TypeEntity Type, decimal Amount, bool IsEntertainment)>>();

        void Add(int day, TypeEntity type, decimal amount, bool isEnt = false)
        {
            if (!dayPayments.TryGetValue(day, out var list))
                dayPayments[day] = list = [];
            list.Add((type, amount, isEnt));
        }

        foreach (var day in productDays)
            Add(day, TypeByName("Product"), Rnd(rng, 50m, 200m));

        foreach (var day in otherDays)
            Add(day, TypeByName("Other"), Rnd(rng, 30m, 50m));

        foreach (var day in entertainmentDays)
            Add(day, TypeByName("Entertainment"), Rnd(rng, 250m, 1000m), isEnt: true);

        return [.. dayPayments
            .OrderBy(kv => kv.Key)
            .Select(kv =>
            {
                var payments = kv.Value.Select(p =>
                {
                    var usePlanned = allPlanned || (!allSpent && !p.IsEntertainment);
                    return new PaymentEntity
                    {
                        Id            = Guid.NewGuid(),
                        TypeId        = p.Type.Id,
                        PlannedAmount = usePlanned ? p.Amount : null,
                        SpentAmount   = usePlanned ? null : p.Amount
                    };
                }).ToList();

                return new ItemEntity
                {
                    Id       = Guid.NewGuid(),
                    UserId   = userId,
                    Date     = new DateTime(year, month, kv.Key, 0, 0, 0, DateTimeKind.Utc),
                    Payments = payments
                };
            })];
    }

    private static decimal Rnd(Random rng, decimal min, decimal max)
        => Math.Round(min + (decimal)rng.NextDouble() * (max - min), 2);
}
