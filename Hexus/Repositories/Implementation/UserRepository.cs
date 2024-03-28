using AutoMapper;
using Hexus.Data;
using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Hexus.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly HexusApiAuthDbContext dbContext;
        private readonly IMapper mapper;

        public UserRepository(HexusApiAuthDbContext dbContext,
            IMapper mapper) {

            this.dbContext = dbContext;
            this.mapper = mapper;
        } 
        public async Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams)
        {

            var query =dbContext.Users.ProjectTo<UserDto>(mapper.ConfigurationProvider).AsNoTracking();
            return await PagedList<UserDto>.CreateAsync(query,userParams.PageNumber,userParams.PageSize);
        }
    }
}
