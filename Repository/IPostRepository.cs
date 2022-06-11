using MiniFacebook.Data;
using MiniFacebook.Models.Post;

namespace MiniFacebook.Repository
{
    public interface IPostRepository
    {
        
        List<DisplayPostModel> GetByUserId(Int64 userId);
        List<Post> GetComments(Int64 PostId);
        int Add(Post model);
        Post? GetById(Int64 id);
        int Delete(Post model);

    }
}
