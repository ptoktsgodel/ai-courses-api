namespace Ai.Courses.Data.Entities;

public class ItemEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<PaymentEntity> Payments { get; set; } = [];
}
