using IMS_LEARN.Infratructure;
using IMS_LEARN.Services.Base;

namespace IMS_LEARN.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IBaseRepository<RefreshToken> _tokenService;
        public TokenService(IBaseRepository<RefreshToken> tokenService)
        {

            _tokenService = tokenService;
        }
        public RefreshToken Logout(string staffcode)
        {
            var token = _tokenService.GetAllQueryable(x => x.StaffCode.Equals(staffcode)).FirstOrDefault();
            return _tokenService.Delete(token);
        }
    }
}
