using AutoMapper;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        UserDto GetUserByEmail(string email);
    }
    public class UserService : IUserService
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;

        public UserService(ProductsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }
        
    }
}
