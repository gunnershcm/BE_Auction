using API.DTOs.Requests.Users;
using Domain.Constants.Enums;
using FluentValidation;

namespace API.Validations.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.Phone)
                .Length(10).WithMessage("Phone is invalid")
                .Matches("^[0-9]*$").WithMessage("Phone must contain only digits.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).When(x => x.DateOfBirth != null)
                .WithMessage("Date of birth should be in the past.");

            RuleFor(x => x.Address)
                .MaximumLength(200).When(x => x.Address != null)
                .WithMessage("Address should not exceed 200 characters.");

            RuleFor(x => x.IdentityId).MinimumLength(12).MaximumLength(12).WithMessage("IdentityId must be 12 characters.")
                .Matches("^[0-9]*$").WithMessage("IdentityId must contain only digits.");

        }
    }
}
