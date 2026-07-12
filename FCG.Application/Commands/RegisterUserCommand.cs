using FCG.Application.DataContracts.UserDtos;
using FCG.Contracts.Events;
using FCG.Domain.Entities.UserEntity;
using FCG.Domain.Entities.UserEntity.Params;
using FCG.Domain.Enums;
using FCG.Domain.Repositories;
using FCG.Domain.ValueObjects;
using MassTransit;
using MediatR;

namespace FCG.Application.Commands
{
    public class RegisterUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class RegisterUserCommandHandler(
        IUserRepository _userRepository,
        IPublishEndpoint _publishEndpoint)
        : IRequestHandler<RegisterUserCommand, UserResponse>
    {
        public async Task<UserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.ExistsByEmailAsync(request.Email);
            if (exists)
                throw new InvalidOperationException("Este e-mail já está cadastrado.");

            var plainPasswordVo = new Password(request.Password);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPasswordVo.Value);

            var userParams = new UpsertUserParams
            {
                Name = request.Name,
                Email = new Email(request.Email),
                Password = new Password(hashedPassword),
                Role = request.Role
            };

            var user = new User(userParams);

            await _userRepository.AddAsync(user);

            await _publishEndpoint.Publish(new UserCreatedEvent(
                user.Id,
                user.Email.Address,
                user.Name,
                DateTime.UtcNow
            ), cancellationToken);

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
