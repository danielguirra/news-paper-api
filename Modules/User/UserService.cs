using Data;
using Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Modules.Auth;

namespace Modules.User
{
    public class UserService(AppDbContext context) : BaseService(context)
    {
        public async Task<bool> Exists(string email, string name) =>
            await context.Users.AnyAsync(u => u.Email == email || u.Name == name);

        private async Task<UserModel?> GetByEmail(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        private async Task<UserModel> GetByIdOrThrow(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException(id);
            return user;
        }

        public async Task<UserModel> Create(UserModel user, AuthModel? auth)
        {
            if (await Exists(user.Email, user.Name))
                throw new UserConflictException($"{user.Email} / {user.Name}");

            if (auth != null)
                if (user.Role != null && user.Role != auth.Role)
                {
                    ValidateRolePermission(auth.Role, user.Role);
                }

            var passwordHasher = new PasswordHasher<UserModel>();
            user.Password = passwordHasher.HashPassword(user, user.Password);
            context.Users.Add(user);
            await SaveAsync();

            return user;
        }

        public async Task<List<UserModel>> ListAll() =>
            await context.Users.OrderBy(u => u.Role).ToListAsync();

        public async Task<bool> ValidateCredentials(LoginModel login)
        {
            var user = await GetByEmail(login.Email);
            if (user == null)
                throw new InvalidUserCredentialsException();

            var passwordHasher = new PasswordHasher<UserModel>();
            var verify = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (verify == PasswordVerificationResult.Failed)
                throw new InvalidUserCredentialsException();

            return true;
        }

        public async Task<string> GetTokenAsync(LoginModel login)
        {
            await ValidateCredentials(login);

            var user = await GetByEmail(login.Email);
            if (user == null)
                throw new UserNotFoundException(null);

            string token = TokenService.GetToken(user);
            return token;
        }

        public async Task Clean()
        {
            var users = await context.Users.ToListAsync();
            context.Users.RemoveRange(users);
            await SaveAsync();
        }

        public async Task<AuthModel> Me(string token)
        {
            var auth = TokenService.ValidateToken(token) ?? throw new InvalidUserTokenException();
            var user =
                await context.Users.FirstOrDefaultAsync(u => u.Id == auth.Id)
                ?? throw new UserNotFoundException(auth.Id);
            return auth;
        }

        public async Task Edit(EditUserDto dto, AuthModel auth)
        {
            var findUser = await GetByIdOrThrow(dto.Id);

            if (dto.Role != null && dto.Role != auth.Role)
            {
                ValidateRolePermission(auth.Role, dto.Role);

                findUser.Role = dto.Role;
            }
            if (
                !string.IsNullOrWhiteSpace(dto.Password)
                && !string.IsNullOrWhiteSpace(dto.NewPassword)
            )
            {
                var login = new LoginModel { Email = findUser.Email, Password = dto.Password };

                if (!await ValidateCredentials(login))
                    throw new InvalidUserCredentialsException();

                var passwordHasher = new PasswordHasher<UserModel>();
                findUser.Password = passwordHasher.HashPassword(findUser, dto.NewPassword);
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
                findUser.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                findUser.Name = dto.Name;

            findUser.UpdatedAt = DateTime.UtcNow;

            context.Users.Update(findUser);
            await SaveAsync();
        }

        public async Task Inactivate(Guid id)
        {
            var user = await GetByIdOrThrow(id);

            user.Active = false;
            context.Users.Update(user);

            await SaveAsync();
        }

        private static void ValidateRolePermission(string authRole, string userRole)
        {
            if (Roles.GetLevel(authRole) < 3 || Roles.GetLevel(userRole) == 0)
                throw new PermissionForbiddenUserExcepion();
        }
    }
}
