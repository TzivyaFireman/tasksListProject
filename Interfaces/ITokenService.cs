using System.Reflection.Metadata.Ecma335;
using taskList.Models;
using taskList.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace taskList.Interfaces;

public interface ITokenService
{
    SecurityToken GetToken(List<Claim> claims);
    TokenValidationParameters GetTokenValidationParameters();
    string WriteToken(SecurityToken token);
}