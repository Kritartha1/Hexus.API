using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;

namespace Hexus.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams);

        
    }
}
