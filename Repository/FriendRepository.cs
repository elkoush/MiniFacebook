using Microsoft.EntityFrameworkCore;
using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class FriendRepository: IFriendRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public FriendRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public List<FriendsDisplayModel> GetByUserId(Int64 userId)
        {
            var query = (from f1 in _apiDbContext.Friends
                 where f1.UserId == userId
                 join f2 in _apiDbContext.Users on f1.FriendId equals f2.Id
                 select f2);

            return query.Select(f=> new FriendsDisplayModel
            {
                
                Username=f.Username,
                Mobile=f.Mobile,

            }).ToList();
        }

        public Int64 Add(Friend model)
        { 
           var existFriend= _apiDbContext.Friends.Where(f => f.UserId == model.UserId && f.FriendId==model.FriendId).ToList();
            if (existFriend.Any())
                return 0;
           _apiDbContext.Friends.Add(model);
          return  _apiDbContext.SaveChanges();
        }
        public Int64 Delete(Friend model)
        {
            _apiDbContext.Friends.Remove(model);
            return _apiDbContext.SaveChanges();
        }
    }
}
