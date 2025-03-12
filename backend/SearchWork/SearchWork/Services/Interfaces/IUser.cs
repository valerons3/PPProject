using SearchWork.Models.DTO;

namespace SearchWork.Services.Interfaces
{
    public interface IUser
    {
        public Task<(bool Success, UserDTO? user)> GetUserInfoByIdAsync(int id);
    }
}
