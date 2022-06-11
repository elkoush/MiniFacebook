using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public ImageRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public Int64 Add(Image model)
        {
            _apiDbContext.Images.Add(model);
             _apiDbContext.SaveChanges();
            return model.Id;
        }

      


    }
}
