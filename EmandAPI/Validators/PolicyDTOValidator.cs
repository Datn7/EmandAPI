using EmandAPI.Models.DTOs;
using FluentValidation;

namespace EmandAPI.Validators
{
    public class PolicyDTOValidator : AbstractValidator<PolicyDTO>
    {
        public PolicyDTOValidator()
        {
            RuleFor(p => p.PolicyName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.CoverageDetails).NotEmpty();
            RuleFor(p => p.Premium).GreaterThan(0);
            RuleFor(p => p.StartDate).NotEmpty();
            RuleFor(p => p.EndDate).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
        }
    }
}
