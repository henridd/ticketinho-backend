using FluentValidation;
using Ticketinho.Common.DTOs.Tickets;

namespace Ticketinho.Validation.Tickets
{
    public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequestDto>
    {
        public CreateTicketRequestValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Zone).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
