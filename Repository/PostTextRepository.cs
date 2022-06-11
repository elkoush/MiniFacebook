using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class PostTextRepository : IPostTextRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public PostTextRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public Int64 Add(PostText model)
        {
            _apiDbContext.PostTexts.Add(model);
             _apiDbContext.SaveChanges();
            return model.Id;
        }

      


    }
}
