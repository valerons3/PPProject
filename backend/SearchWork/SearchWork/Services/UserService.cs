using SearchWork.Data;
using SearchWork.Models.DTO;
using SearchWork.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SearchWork.Services
{
    public class UserService : IUser
    {
        private ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<(bool Success, UserDTO? user)> GetUserInfoByIdAsync(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) { return (false, null); }

            string userRole = string.Empty;
            switch (user.RoleId)
            {
                case 1:
                    userRole = "Соискатель";
                    break;
                case 2:
                    userRole = "Работодатель";
                    break;
                case 3:
                    userRole = "Администратор";
                    break;
            }

            UserDTO userDTO = new UserDTO()
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = userRole,
                CreatedAt = user.CreatedAt
            };

            return (true, userDTO);
        }
    }
}
