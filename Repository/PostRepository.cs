using Microsoft.EntityFrameworkCore;
using MiniFacebook.Data;
using MiniFacebook.Models.Post;

namespace MiniFacebook.Repository
{
    public class PostRepository: IPostRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public PostRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public  List<DisplayPostModel> GetByUserId(Int64 userId) {

            var query = (from p in _apiDbContext.Posts
                         where p.UserId == userId
                         select p);

            var query2 = (from p in _apiDbContext.Posts
                          join f in _apiDbContext.Friends on p.UserId equals f.FriendId
                          where f.UserId == userId
                          select p);
            return query.Union(query2).Select(p => new DisplayPostModel
            {
                Id = p.Id,
                AddedDate = p.AddedDate,
                UserId = p.UserId,
                PostText = p.PostText==null?"": p.PostText.Text,
                PostImage = p.Image == null ? Array.Empty<byte>() : p.Image.ImageData,
            }).ToList(); 
             
        }
        public List<Post> GetComments(Int64 PostId)
        {
            return _apiDbContext.Posts.Where(p => p.ParentId == PostId).ToList();
        }
        public int Add(Post model)
        {
             _apiDbContext.Posts.Add(model);
            return _apiDbContext.SaveChanges();
        }
        public int Delete(Post model)
        {
            _apiDbContext.Posts.Remove(model);
            return _apiDbContext.SaveChanges();
        }

        public Post? GetById(Int64 id)
        {
            return _apiDbContext.Posts.Where(_p => _p.Id == id).FirstOrDefault();   
        }

    }
}
