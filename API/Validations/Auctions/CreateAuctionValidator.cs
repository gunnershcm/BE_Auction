using API.DTOs.Requests.Auctions;
using API.DTOs.Requests.Users;
using Domain.Constants.Enums;
using FluentValidation;

namespace API.Validations.Users
{
    public class CreateAuctionValidator : AbstractValidator<CreateAuctionRequest>
    {
        public CreateAuctionValidator()
        {
            RuleFor(x => x.BiddingStartTime)
                 .GreaterThan(DateTime.Today).When(x => x.BiddingStartTime != null)
                 .WithMessage("Bidding start time should be in the future.");

            RuleFor(x => x.BiddingEndTime)
                .GreaterThan(x => x.BiddingStartTime).When(x => x.BiddingEndTime != null)
                .WithMessage("Bidding end time should be greater than start time.");


        }
    }
}
