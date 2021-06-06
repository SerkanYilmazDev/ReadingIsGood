using System;

namespace Api.Controllers.Dtos
{
    public class UserClaimDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}