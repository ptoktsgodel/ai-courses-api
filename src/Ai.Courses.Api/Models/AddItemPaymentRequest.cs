namespace Ai.Courses.Api.Models;

public class AddItemPaymentRequest
{
    public DateTime Date { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal? PlannedAmount { get; set; }
    public decimal? SpentAmount { get; set; }
}
