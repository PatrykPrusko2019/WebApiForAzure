using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MusicStoreApi.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.SetNewRecordsToDatabase;

namespace WebApplication1.Services
{

    public interface IAccountService
    {
        string GenerateJwt(LoginDto loginDto);
        void RegisterUser(RegisterUserDto registerUserDto);
    }
    public class AccountService : IAccountService
    {
        private readonly ProductsDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(ProductsDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = new User()
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                DateOfBirth = registerUserDto.DateOfBirth,
                Nationality = registerUserDto.Nationality
            };

            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;

            //create table when is not exists in database
            DataSeeder dataSeeder = new DataSeeder(dbContext);
            dataSeeder.SeedUser(newUser);
        }

        /// <summary>
        /// checks whether the user logged valid to his account
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>generates token</returns>
        /// <exception cref="BadRequestException"></exception>
        public string GenerateJwt(LoginDto loginDto)
        {
            var user = dbContext.Users
                .FirstOrDefault(u => u.Email == loginDto.Email);

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Invalid email(login) or password"); //There is no precise information that this is an incorrect passwordHash, for the security of data of customers using a given application.

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),

            };

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
