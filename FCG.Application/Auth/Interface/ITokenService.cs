using FCG.Domain.Entities.UserEntity;

namespace FCG.Application.Auth.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
