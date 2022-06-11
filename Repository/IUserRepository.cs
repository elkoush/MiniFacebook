using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public interface   IUserRepository
    {
        
        List<User> GetAll();
        User? Login(LoginModel model);
        int Register (User model);
        User? GetById(Int64 id);
        User? GetByUserName(string userName);
        User? GetByPhoneNumber(string phoneNumber);
        int  verifyByPhoneNumber(string phoneNumber);
        int GetUsersByIp(string ip);
    }
}
