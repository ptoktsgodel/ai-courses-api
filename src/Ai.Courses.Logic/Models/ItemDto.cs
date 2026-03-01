namespace Ai.Courses.Logic.Models;

public class ItemDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<PaymentDto> Payments { get; set; }
}
