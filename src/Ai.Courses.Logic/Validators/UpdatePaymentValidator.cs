using Ai.Courses.Logic.Commands.UpdatePayment;
using FluentValidation;

namespace Ai.Courses.Logic.Validators;

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("PaymentId is required.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("ItemId is required.");

        RuleFor(x => x.TypeName)
            .NotEmpty().WithMessage("Type name is required.")
            .MaximumLength(100).WithMessage("Type name must not exceed 100 characters.");

        RuleFor(x => x)
            .Must(x => x.PlannedAmount.HasValue || x.SpentAmount.HasValue)
            .WithMessage("Either PlannedAmount or SpentAmount must be provided.");

        When(x => x.PlannedAmount.HasValue, () =>
        {
            RuleFor(x => x.PlannedAmount!.Value)
                .GreaterThan(0).WithMessage("Planned amount must be greater than 0.");
        });

        When(x => x.SpentAmount.HasValue, () =>
        {
            RuleFor(x => x.SpentAmount!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Spent amount must be greater than or equal to 0.");
        });

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
