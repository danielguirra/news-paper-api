using Data;
using Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Auth.Model;
using Modules.Auth.Token;
using Modules.User.Dto;
using Modules.User.Model;

namespace Modules.User.Service
{
    public class UserService(AppDbContext context) : BaseService(context)
    {
        public async Task<bool> Exists(string email, string name) =>
            await context.Users.AnyAsync(u => u.Email == email || u.Name == name);

        private async Task<UserModel?> GetByEmail(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        private async Task<UserModel> GetByIdOrThrow(Guid id) =>
            await context.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new UserNotFoundException(id);

        public async Task<UserModel> Create(UserModel user, AuthModel? auth)
        {
            if (await Exists(user.Email, user.Name))
                throw new UserConflictException($"{user.Email} / {user.Name}");

            if (auth != null)
                if (user.Role != null && user.Role != auth.Role)
                {
                    ValidateRolePermission(auth.Role, user.Role);
                }

            PasswordHasher<UserModel>? passwordHasher = new();
            user.Password = passwordHasher.HashPassword(user, user.Password);
            context.Users.Add(user);
            await SaveAsync();

            return user;
        }

        public async Task<List<UserModel>> ListAll() =>
            await context.Users.OrderBy(u => u.Role).ToListAsync()
            ?? throw new UserNotFoundException(null);

        public async Task<bool> ValidateCredentials(LoginBodyModelDto login)
        {
            UserModel? user =
                await GetByEmail(login.Email) ?? throw new InvalidUserCredentialsException();

            PasswordHasher<UserModel>? passwordHasher = new();
            PasswordVerificationResult verify = passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                login.Password
            );

            if (verify == PasswordVerificationResult.Failed)
                throw new InvalidUserCredentialsException();

            return true;
        }

        public async Task<string> GetTokenAsync(LoginBodyModelDto login)
        {
            await ValidateCredentials(login);

            UserModel? user =
                await GetByEmail(login.Email) ?? throw new UserNotFoundException(null);

            string token = TokenService.GetToken(user);
            return token;
        }

        public async Task Clean()
        {
            List<UserModel> users = await context.Users.ToListAsync();
            context.Users.RemoveRange(users);
            await SaveAsync();
        }

        public async Task<AuthModel> Me(string token)
        {
            AuthModel? auth =
                TokenService.ValidateToken(token) ?? throw new InvalidUserTokenException();
            UserModel? user =
                await context.Users.FirstOrDefaultAsync(u => u.Id == auth.Id)
                ?? throw new UserNotFoundException(auth.Id);
            return auth;
        }

        public async Task Edit(EditUserDto dto, AuthModel auth)
        {
            UserModel? findUser = await GetByIdOrThrow(dto.Id);

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
                LoginBodyModelDto login = new() { Email = findUser.Email, Password = dto.Password };

                if (!await ValidateCredentials(login))
                    throw new InvalidUserCredentialsException();

                PasswordHasher<UserModel>? passwordHasher = new();
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
            UserModel user = await GetByIdOrThrow(id);

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
