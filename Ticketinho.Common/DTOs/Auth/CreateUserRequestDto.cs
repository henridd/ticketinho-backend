﻿namespace Ticketinho.Common.DTOs.Auth
{
    public class CreateUserRequestDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
    }
}

