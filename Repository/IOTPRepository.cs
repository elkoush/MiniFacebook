using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public interface IOTPRepository
    {


         int Add(Otp model);

         int Delete(Otp model);

         Otp? GetByMobile(string Mobile);

         int update(Otp model);

    }
}
