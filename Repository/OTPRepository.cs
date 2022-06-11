using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class OTPRepository : IOTPRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public OTPRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public int Add(Otp model)
        {
          var oldModel=  _apiDbContext.Otps.Where(e => e.Mobile == model.Mobile).FirstOrDefault();
            if (oldModel != null)
                Delete(oldModel);
            _apiDbContext.Otps.Add(model);
            return _apiDbContext.SaveChanges();
        }

        public int Delete(Otp model)
        {
             _apiDbContext.Otps.Remove(model);
           return   _apiDbContext.SaveChanges();
        }

        public Otp? GetByMobile(string  Mobile)
        {
            return _apiDbContext.Otps.Where(e => e.Mobile == Mobile).FirstOrDefault();
        }

        public int update(Otp model)
        {
             _apiDbContext.Otps.Update(model);
             return  _apiDbContext.SaveChanges();
        }


    }
}
