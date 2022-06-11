using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public interface IImageRepository
    {
        Int64 Add(Image model);
    }
}
