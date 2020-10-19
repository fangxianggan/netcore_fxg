using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore.Core.Extensions;
using NetCore.Domain.Interface;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_JWTToken;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCore.Services.Services.S_JWTToken
{
    public class JWTTokenServices : BaseServices<UserInfo,UserInfoViewModel>, IJWTTokenServices
    {

        private readonly IBaseDomain<UserInfo> _baseDomain;
        private IOptions<JWTConfigViewModel> _options;
        public JWTTokenServices(IOptions<JWTConfigViewModel> options, IBaseDomain<UserInfo> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
            _options = options;
        }

        public TokenViewModel CreateAccessToken(UserInfoViewModel user)
        {
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserCode),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            return CreateToken(claims, TokenType.AccessToken);
        }

        public ComplexTokenViewModel CreateToken(UserInfoViewModel user)
        {
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserCode),
                new Claim(ClaimTypes.Name, user.UserName)
                //下面两个Claim用于测试在Token中存储用户的角色信息
                , new Claim(ClaimTypes.Role, "TestPutBookRole"),
                new Claim(ClaimTypes.Role, "TestPutStudentRole")
            };

            return CreateToken(claims);
        }

        public ComplexTokenViewModel CreateToken(Claim[] claims)
        {
            return new ComplexTokenViewModel { AccessToken = CreateToken(claims, TokenType.AccessToken), RefreshToken = CreateToken(claims, TokenType.RefreshToken) };
        }

        /// <summary>
        /// 用于创建AccessToken和RefreshToken。
        /// 这里AccessToken和RefreshToken只是过期时间不同，【实际项目】中二者的claims内容可能会不同。
        /// 因为RefreshToken只是用于刷新AccessToken，其内容可以简单一些。
        /// 而AccessToken可能会附加一些其他的Claim。
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        private TokenViewModel CreateToken(Claim[] claims, TokenType tokenType)
        {
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(tokenType.Equals(TokenType.AccessToken) ? _options.Value.AccessTokenExpiresMinutes : _options.Value.RefreshTokenExpiresMinutes));//设置不同的过期时间
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: tokenType.Equals(TokenType.AccessToken) ? _options.Value.Audience : _options.Value.RefreshTokenAudience,//设置不同的接受者
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256));
            return new TokenViewModel { TokenContent = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires };
        }

        public TokenViewModel RefreshToken(ClaimsPrincipal claimsPrincipal)
        {
            var code = claimsPrincipal.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier));
            if (null != code)
            {
                var ent = _baseDomain.GetEntity(p => p.UserCode == code.Value.ToString());
                return CreateAccessToken(ent.MapTo<UserInfoViewModel>());
            }
            else
            {
                return null;
            }
        }
    }
}
