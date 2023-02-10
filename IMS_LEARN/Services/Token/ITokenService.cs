using IMS_LEARN.Infratructure;

namespace IMS_LEARN.Services.Token
{
    public interface ITokenService
    {
        public RefreshToken Logout(string staffcode);
    }
}
