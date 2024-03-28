using Hexus.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace Hexus.Repositories.Interface
{
    
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<string> roles);
    }
}
