namespace PmeMemberApi.Core.IDao
{
    public interface IPmeMemberApiDao
	{
        Task<PmeMemberApi?> GetMember(long id);

        Task<List<PmeMemberApi>> GetAllMembers();

        Task AddMember(PmeMemberApi member);

        Task DeleteMember(long id);

        Task UpdateMember(PmeMemberApi member);

    }
}
