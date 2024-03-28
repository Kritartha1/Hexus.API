using Hexus.Data;
using Hexus.Models.Domain;
using Hexus.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexus.Repositories.Implementation
{
    public class DMRepository : IDMRepository
    {
        private readonly HexusApiAuthDbContext dbContext;

        public DMRepository(HexusApiAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<DM> CreateAsync(DM dm)
        {
            await dbContext.DMs.AddAsync(dm);
            await dbContext.SaveChangesAsync();
            return dm;
        }

        public async Task<DM?> DeleteAsync(Guid id)
        {
            var existingDM = await dbContext.DMs.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDM == null) { return null; }

            dbContext.DMs.Remove(existingDM);
            await dbContext.SaveChangesAsync();
            return existingDM;
        }

        public async Task<List<DM>> GetAllAsync()
        {
            return await dbContext.DMs.ToListAsync();
        }

        public async Task<DM?> GetByIdAsync(Guid id)
        {
            return await dbContext.DMs.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<DM?> UpdateAsync(Guid id, DM dm)
        {
            var existingDM = await dbContext.DMs.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDM == null) { return null; }

            existingDM.Id = id;
            existingDM.Message=dm.Message;

            await dbContext.SaveChangesAsync();
            return existingDM;
        }
    }
}
