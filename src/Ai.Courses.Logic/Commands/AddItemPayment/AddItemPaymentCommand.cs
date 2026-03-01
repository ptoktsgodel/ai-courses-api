using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Commands.AddItemPayment;

public class AddItemPaymentCommand : IRequest<ItemDto>
{
    public DateTime Date { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal? PlannedAmount { get; set; }
    public decimal? SpentAmount { get; set; }
    public Guid UserId { get; set; }
}
