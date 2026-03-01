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

        // --- Seed types ---
        var typeNames = new[] { "Rent", "UtilityBills", "Taxes", "Product", "Other", "Entertainment" };
        var types = typeNames.Select(name => new TypeEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name
        }).ToList();

        await db.Types.AddRangeAsync(types);
        await db.SaveChangesAsync();

        TypeEntity TypeByName(string name) => types.First(t => t.Name == name);

        // --- Build schedule ---
        // Fixed days
        var schedule = new List<(int Day, (string TypeName, decimal Amount)[] Payments)>
        {
            (10, [("Rent", 3500m), ("UtilityBills", 1500m)]),
            (20, [("Taxes", 5000m)])
        };

        // Pick random days for variable entries (max 15, excludes fixed days)
        var rng = new Random();
        var fixedDays = new HashSet<int> { 10, 20 };
        var availableDays = Enumerable.Range(1, 31).Where(d => !fixedDays.Contains(d)).ToList();
        var randomDays = availableDays.OrderBy(_ => rng.Next()).Take(15).OrderBy(d => d).ToList();

        // Entertainment: exactly 3 random days with fixed spent amounts (no planned amount)
        var entertainmentAmounts = new[] { 500m, 700m, 1000m };
        var entertainmentDays = randomDays.Take(3).ToList();
        foreach (var (day, spent) in entertainmentDays.Zip(entertainmentAmounts))
            schedule.Add((day, [("Entertainment", spent)]));

        // Remaining 12 days: Other or Product with random planned amounts
        var otherProductTypes = new[] { "Other", "Product" };
        var otherProductAmounts = new Dictionary<string, (decimal Min, decimal Max)>
        {
            ["Other"]   = (30m,  100m),
            ["Product"] = (100m, 300m)
        };
        foreach (var day in randomDays.Skip(3))
        {
            var typeName = otherProductTypes[rng.Next(otherProductTypes.Length)];
            var (min, max) = otherProductAmounts[typeName];
            var amount = Math.Round(min + (decimal)rng.NextDouble() * (max - min), 2);
            schedule.Add((day, [(typeName, amount)]));
        }

        schedule = [.. schedule.OrderBy(s => s.Day)];

        var items = schedule.Select(entry =>
        {
            var item = new ItemEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = new DateTime(2026, 3, entry.Day, 0, 0, 0, DateTimeKind.Utc),
                Payments = entry.Payments.Select(p => new PaymentEntity
                {
                    Id = Guid.NewGuid(),
                    TypeId = TypeByName(p.TypeName).Id,
                    PlannedAmount = p.TypeName == "Entertainment" ? null : p.Amount,
                    SpentAmount   = p.TypeName == "Entertainment" ? p.Amount : null
                }).ToList()
            };
            return item;
        }).ToList();

        await db.Items.AddRangeAsync(items);
        await db.SaveChangesAsync();

        logger.LogInformation("Payments seed data applied: {ItemCount} items, {PaymentCount} payments.",
            items.Count,
            items.Sum(i => i.Payments.Count));
    }
}
