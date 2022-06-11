using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public interface IPostTextRepository 
    {
        Int64 Add(PostText model);
    
    }
}
