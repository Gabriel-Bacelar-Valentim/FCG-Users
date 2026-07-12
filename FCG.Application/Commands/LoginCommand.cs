using FCG.Application.Auth.Interface;
using FCG.Domain.Repositories;
using MediatR;

namespace FCG.Application.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService) : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password.Value))
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
            }

            return tokenService.GenerateToken(user);
        }
    }
}
