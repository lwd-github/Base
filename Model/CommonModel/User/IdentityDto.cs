using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class IdentityDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

    }
}
