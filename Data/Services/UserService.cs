using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class UserService
    {
        private readonly InventarioContext _context;
        public UserService(InventarioContext context)
        {
            _context = context;
        }

        //GET
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUsersByIdAsync(int ID_USER)
        {
            var result = await _context.Users.FromSqlRaw(sql: "SELECT * FROM \"USER\" WHERE \"ID_USER\" = {0}", ID_USER).FirstOrDefaultAsync();

            if(result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        //POST

        public async Task<User> CreateUser (User USER)
        {
            try
            {
                var userEntity = new User
                {
                    EMAIL = USER.EMAIL,
                    UUID = USER.UUID,
                    NAME = USER.NAME,
                    DATE = DateTimeOffset.UtcNow
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();
                return userEntity;
            }
            catch(Exception)
            {
                throw;
            }

        }
    }
}
