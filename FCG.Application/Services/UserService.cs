using FCG.Application.DataContracts.UserDtos;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Entities.UserEntity;
using FCG.Domain.Entities.UserEntity.Params;
using FCG.Domain.Repositories;
using FCG.Domain.ValueObjects;

namespace FCG.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> RegisterAsync(RegisterUserRequest request)
        {
            var exists = await _userRepository.ExistsByEmailAsync(request.Email);
            if (exists)
                throw new InvalidOperationException("Este e-mail já está cadastrado.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var emailVo = new Email(request.Email);
            var passwordVo = new Password(hashedPassword);

            var plainPasswordVo = new Password(request.Password);
            var hashedString = BCrypt.Net.BCrypt.HashPassword(plainPasswordVo.Value);

            var userParams = new UpsertUserParams
            {
                Name = request.Name,
                Email = emailVo,
                Password = new Password(hashedString),
                Role = request.Role
            };

            var user = new User(userParams);

            await _userRepository.AddAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Address,
                Role = user.Role
            };
        }
    }
}
