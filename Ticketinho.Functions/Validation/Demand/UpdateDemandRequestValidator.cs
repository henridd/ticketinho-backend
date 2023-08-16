using FluentValidation;
using Ticketinho.Common.DTOs.Demand;

namespace Ticketinho.Validation.Demand
{
    public class UpdateDemandRequestValidator : AbstractValidator<UpdateDemandRequestDto>
    {
        public UpdateDemandRequestValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Zone).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
