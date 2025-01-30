using WebBackendAPI.Models;

namespace WebBackendAPI.Services
{
    public class AuthService
    {
        private readonly List<User> _users = new();

        public bool Login(LoginRequest request)
        {
            return _users.Any(u => u.Email == request.Email && u.Password == request.Password);
        }

        public bool Signup(SignupRequest request)
        {
            if (_users.Any(u => u.Email == request.Email)) return false;

            _users.Add(new User
            {
                Id = _users.Count + 1,
                Email = request.Email,
                Password = request.Password // In production, hash the password!
            });

            return true;
        }
    }
}
