using EmandAPI.Models.DTOs;
using FluentValidation;

namespace EmandAPI.Validators
{
    public class ClaimDTOValidator : AbstractValidator<ClaimDTO>
    {
        public ClaimDTOValidator()
        {
            RuleFor(c => c.PolicyId).GreaterThan(0);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(500);
            RuleFor(c => c.Amount).GreaterThan(0);
        }
    }
}
