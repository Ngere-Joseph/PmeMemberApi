using Microsoft.EntityFrameworkCore;
using PmeMemberApi.Core.IDao;
using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Core.Dao
{
    public class PmeMemberApiDao : IPmeMemberApiDao
	{
        private readonly AppDbContext _context;

        public PmeMemberApiDao(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PmeMemberApi?> GetMember(long id)
        {
            return await _context.PmeMemberApis.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<PmeMemberApi>> GetAllMembers()
        {
            return (await _context.PmeMemberApis.ToListAsync())!;
        }

        public async Task AddMember(PmeMemberApi member)
        {
            _context.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMember(long id)
        {
            var member = await GetMember(id);
            if (member != null) _context.Remove<PmeMemberApi>(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMember(PmeMemberApi member)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
