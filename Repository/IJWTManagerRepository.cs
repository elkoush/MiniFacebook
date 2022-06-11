
using MiniFacebook.Models;
using MiniFacebook.Data;
using System.Security.Claims;

namespace MiniFacebook.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(User users);
        string GetUserId(ClaimsPrincipal claimsPrincipal);
    }
}
