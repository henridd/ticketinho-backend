using FluentValidation;
using Ticketinho.Common.DTOs.Negotiation;

namespace Ticketinho.Validation.Negotiation
{
    public class CreateNegotiationRequestValidator : AbstractValidator<CreateNegotiationRequestDto>
    {
        public CreateNegotiationRequestValidator()
        {
            RuleFor(x => x.TicketId).NotEmpty();
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x => x.BuyerId).NotEmpty();
        }
    }
}
