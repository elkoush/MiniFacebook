using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public interface IFriendRepository
    {

         List<FriendsDisplayModel> GetByUserId(Int64 userId);
         Int64 Add(Friend model);
         Int64 Delete(Friend model);
    }
}
