using API.DTOs.Requests.Auths;
using FluentValidation;

namespace API.Validations.Users
{
    public class SignUpValidators : AbstractValidator<SignUpUser>
    {
        public SignUpValidators()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Email Address is invalid")
                .NotEmpty().WithMessage("Email Address is required.");

            RuleFor(u => u.Phone)
                .Length(10).WithMessage("Phone is invalid")
                .NotEmpty().WithMessage("Phone is required.")
                .Matches("^[0-9]*$").WithMessage("Phone must contain only digits.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).When(x => x.DateOfBirth != null)
                .WithMessage("Date of birth should be in the past.");

            RuleFor(x => x.Address)
                .MaximumLength(200).When(x => x.Address != null)
                .WithMessage("Address should not exceed 200 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required");


            RuleFor(x => x.IdentityId).MinimumLength(12).MaximumLength(12).WithMessage("IdentityId must be 12 characters.")
                .NotEmpty().WithMessage("Identity Number is required")
                .Matches("^[0-9]*$").WithMessage("IdentityId must contain only digits.");

            RuleFor(x => x.IssuedDate)
                .LessThan(DateTime.Now).When(x => x.IssuedDate != null)
                .WithMessage("Issued Date must be in the past");

            RuleFor(u => u.IssuedPlace)
               .NotEmpty().WithMessage("Issued Place is required");

            RuleFor(u => u.BankNumber)
                .NotEmpty().WithMessage("Bank Number is required");

            RuleFor(u => u.BankId)
               .NotEmpty().WithMessage("Bank Id is required");

            RuleFor(u => u.BankNameHolder)
               .NotEmpty().WithMessage("Bank Name Holder is required");



        }
    }
}
