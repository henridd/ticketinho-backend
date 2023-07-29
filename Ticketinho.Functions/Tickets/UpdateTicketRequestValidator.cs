using FluentValidation;
using Ticketinho.DTOs.Tickets;

namespace Ticketinho.DTOs.Validation.Tickets
{
    public class UpdateTicketRequestValidator : AbstractValidator<UpdateTicketRequestDto>
    {
        public UpdateTicketRequestValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Zone).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
