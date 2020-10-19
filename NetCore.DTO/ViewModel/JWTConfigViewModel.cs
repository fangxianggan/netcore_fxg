using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ViewModel
{
   public class JWTConfigViewModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string IssuerSigningKey { get; set; }
        public int AccessTokenExpiresMinutes { get; set; }
        public string RefreshTokenAudience { get; set; }
        public int RefreshTokenExpiresMinutes { get; set; }
    }


    public class ComplexTokenViewModel
    {
        public TokenViewModel AccessToken { get; set; }
        public TokenViewModel RefreshToken { get; set; }
    }


    public class TokenViewModel
    {
        public string TokenContent { get; set; }

        public DateTime Expires { get; set; }
    }
}
