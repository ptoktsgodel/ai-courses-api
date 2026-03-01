using MediatR;

namespace Ai.Courses.Logic.Commands.DeletePayment;

public class DeletePaymentCommand : IRequest<bool>
{
    public Guid PaymentId { get; set; }
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
}
