using FlightStatusManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FlightStatusManagement.Infrastructure.Auth
{
    public class PasswordHasherService : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        private static readonly object _user = new();

        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(_user, password);
        }
        public bool Verify(string password, string passwordHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(_user, passwordHash, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
