using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Commands.UpdatePayment;

public class UpdatePaymentCommand : IRequest<PaymentDto?>
{
    public Guid PaymentId { get; set; }
    public Guid ItemId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal? PlannedAmount { get; set; }
    public decimal? SpentAmount { get; set; }
    public Guid UserId { get; set; }
}
