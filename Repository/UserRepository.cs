using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public UserRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public List<User> GetAll()
        {
            return _apiDbContext.Users.ToList();
        }
        public User? GetById(Int64 id)
        {
            return _apiDbContext.Users.Where(e => e.Id ==id).FirstOrDefault();
        }


        public User? GetByUserName(string userName)
        {
            return _apiDbContext.Users.Where(e => e.Username == userName).FirstOrDefault();

        }  
        public User? GetByPhoneNumber(string phoneNumber)
        {
            return _apiDbContext.Users.Where(e => e.Mobile == phoneNumber).FirstOrDefault();

        }
         public int verifyByPhoneNumber(string phoneNumber)
        {
            User? user= _apiDbContext.Users.Where(e => e.Mobile == phoneNumber).FirstOrDefault();
            if (user!=null)
            {
              user.MobileConfirmed = true;
             return   _apiDbContext.SaveChanges();
            }

            return 0;
        }

        public User? Login(LoginModel model)
        {
            return _apiDbContext.Users.Where(e => e.Username == model.Username && e.Password == model.Password).FirstOrDefault();
                 
        }
        public int Register(User model)
        { 


           _apiDbContext.Users.Add(model);
          return  _apiDbContext.SaveChanges();

         

        }

        public int GetUsersByIp(string ip)
        {


          return  _apiDbContext.Users.Where(u=>u.Ip==ip).Count();



        }

    }
}
