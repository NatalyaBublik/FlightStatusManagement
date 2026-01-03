using FlightStatusManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusManagement.Application.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IApplicationDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IApplicationDbContext db, IPasswordHasher hasher, ITokenService tokenService)
        {
            _db = db;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Username == request.Username, ct);

            if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            var token = _tokenService.CreateAccessToken(user);

            return new LoginResponse(token, ExpiresInSeconds: 60 * 60, Role: user.Role.Code);
        }
    }
}
