namespace Ai.Courses.Logic.Models;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal? PlannedAmount { get; set; }
    public decimal? SpentAmount { get; set; }
}
