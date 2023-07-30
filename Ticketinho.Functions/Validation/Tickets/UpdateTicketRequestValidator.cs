using FluentValidation;
using Ticketinho.Common.DTOs.Tickets;

namespace Ticketinho.Validation.Tickets
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
