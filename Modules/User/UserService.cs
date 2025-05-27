using Auth.JwtStrategy;

using Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Models;
using Models.AuthModel;

namespace Services
{
    public class UserService(AppDbContext context)
    {
        public async Task<bool> Exists(UserModel user) =>
            await context.Users.AnyAsync(u => u.Email == user.Email || u.Name == user.Name);

        private async Task<UserModel?> GetByEmail(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

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

        public async Task<string?> Login(LoginModel login)
        {
            var user = await GetByEmail(login.Email);
            if (user == null) return null;

            var passwordHasher = new PasswordHasher<UserModel>();
            var verify = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (verify == PasswordVerificationResult.Failed) return null;

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
                var user = await context.Users.FirstOrDefaultAsync(user => user.Id == auth.Id);
                if (user != null)
                {
                    return auth;
                }
            }

            return null;
        }

        public async Task<bool> Edit(UserModel user)
        {
            if (!await Exists(user)) return false;

            var findUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (findUser == null) return false;

            findUser.Name = user.Name;
            findUser.Email = user.Email;
            findUser.Role = user.Role;
            findUser.UpdatedAt = DateTime.UtcNow;

            context.Users.Update(findUser);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
