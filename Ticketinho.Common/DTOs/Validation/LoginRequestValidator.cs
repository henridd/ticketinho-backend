﻿using FluentValidation;

namespace Ticketinho.Common.DTOs.Validation
{
	public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
        }
	}
}
