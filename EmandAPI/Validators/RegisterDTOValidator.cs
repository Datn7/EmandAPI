using EmandAPI.Models.DTOs;
using FluentValidation;

namespace EmandAPI.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.ProfilePictureUrl).MaximumLength(500);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue);
        }
    }
}
