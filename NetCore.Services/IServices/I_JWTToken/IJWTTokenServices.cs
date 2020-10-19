using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetCore.Services.IServices.I_JWTToken
{
   public interface IJWTTokenServices
    {
        ComplexTokenViewModel CreateToken(UserInfoViewModel user);
        ComplexTokenViewModel CreateToken(Claim[] claims);
        TokenViewModel RefreshToken(ClaimsPrincipal claimsPrincipal);
    }
}
