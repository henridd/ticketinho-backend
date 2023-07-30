using FluentValidation;
using Ticketinho.DTOs.Tickets;

namespace Ticketinho.DTOs.Validation.Tickets
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
