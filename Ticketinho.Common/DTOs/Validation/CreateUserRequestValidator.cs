using FluentValidation;

namespace Ticketinho.Common.DTOs.Validation
{
	public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
	{
		public CreateUserRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
	}
}

