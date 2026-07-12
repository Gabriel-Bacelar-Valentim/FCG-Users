using FCG.Application.DataContracts.UserDtos;

namespace FCG.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(RegisterUserRequest request);
    }
}
