using FluentValidation;
using Ticketinho.Common.DTOs.Demand;

namespace Ticketinho.Validation.Demand
{
    internal class CreateDemandRequestValidation : AbstractValidator<CreateDemandRequestDto>
    {
        public CreateDemandRequestValidation()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Zone).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
