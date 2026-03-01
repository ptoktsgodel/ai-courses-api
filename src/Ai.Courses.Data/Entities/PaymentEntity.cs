namespace Ai.Courses.Data.Entities;

public class PaymentEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid TypeId { get; set; }
    public decimal? PlannedAmount { get; set; }
    public decimal? SpentAmount { get; set; }

    public ItemEntity Item { get; set; }
    public TypeEntity Type { get; set; }
}
