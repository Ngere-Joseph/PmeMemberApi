using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Core.IDao
{
    public interface IAuthDao
    {
        Task<JsonWebToken?> Login(Login login);

        Task<AuthActivityFeedback> Register(UserRegistration userDetails);
    }
}
