using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Modules.Auth;

namespace Modules.User
{
    public class UserService(AppDbContext context)
    {
        public async Task<bool> Exists(UserModel user) =>
            await context.Users.AnyAsync(_user =>
                _user.Email == user.Email || _user.Name == user.Name
            );

        private async Task<UserModel?> GetByEmail(string email) =>
            await context.Users.FirstOrDefaultAsync(_user => _user.Email == email);

        public async Task<UserModel?> Create(UserModel user)
        {
            var passwordHasher = new PasswordHasher<UserModel>();
            user.Password = passwordHasher.HashPassword(user, user.Password);
            context.Users.Add(user);
            var saved = await context.SaveChangesAsync();
            return saved > 0 ? user : null;
        }

        public async Task<List<UserModel>> ListAll() =>
            await context.Users.OrderBy(u => u.Role).ToListAsync();

        public async Task<bool?> Login(LoginModel login)
        {
            var user = await GetByEmail(login.Email);
            if (user == null)
                return false;

            var passwordHasher = new PasswordHasher<UserModel>();
            var verify = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (verify == PasswordVerificationResult.Failed)
                return false;

            return true;
        }

        public async Task<string?> GetTokenAsync(LoginModel login)
        {
            var user = await GetByEmail(login.Email);
            if (user == null)
                return null;

            var passwordHasher = new PasswordHasher<UserModel>();
            var verify = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (verify == PasswordVerificationResult.Failed)
                return null;

            string token = TokenService.GetToken(user);

            return token;
        }

        public async Task Clean()
        {
            var users = await context.Users.ToListAsync();
            context.Users.RemoveRange(users);
            await context.SaveChangesAsync();
        }

        public async Task<AuthModel?> Me(string token)
        {
            AuthModel? auth = TokenService.ValidateToken(token);
            if (auth != null)
            {
                var user = await context.Users.FirstOrDefaultAsync(_user => _user.Id == auth.Id);
                if (user != null)
                {
                    return auth;
                }
            }

            return null;
        }

        public async Task<bool> Edit(EditUserDto dto)
        {
            var findUser = await context.Users.FirstOrDefaultAsync(_user => _user.Id == dto.Id);

            if (findUser == null)
                return false;

            if (
                !string.IsNullOrWhiteSpace(dto.Password)
                && !string.IsNullOrWhiteSpace(dto.NewPassword)
            )
            {
                var login = new LoginModel { Email = findUser.Email, Password = dto.Password };

                if (!(await Login(login)).GetValueOrDefault())
                    return false;

                var passwordHasher = new PasswordHasher<UserModel>();
                findUser.Password = passwordHasher.HashPassword(findUser, dto.NewPassword);
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
                findUser.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                findUser.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Role))
                findUser.Role = dto.Role;

            findUser.UpdatedAt = DateTime.UtcNow;

            context.Users.Update(findUser);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<UserModel?> Inactive(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(_user => _user.Id == id);
            if (user is null)
                return null;

            user.Active = false;
            context.Users.Update(user);

            var changes = await context.SaveChangesAsync();
            return changes > 0 ? user : null;
        }
    }
}
