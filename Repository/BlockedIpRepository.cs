using MiniFacebook.Data;
using MiniFacebook.Models.users;

namespace MiniFacebook.Repository
{
    public class BlockedIpRepository : IBlockedIpRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public BlockedIpRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public BlockedIp? GetByIp(string ip)
        {

           return _apiDbContext.BlockedIps.Where(b=>b.Ip==ip).FirstOrDefault();
           
        }
        public int Add(string ip)
        {

            var model = GetByIp(ip);
            if (model==null)
            {
                  _apiDbContext.BlockedIps.Add(new BlockedIp() { Ip = ip, FailedAttempts = 1, Blocked = false });
                return _apiDbContext.SaveChanges();
            }
            if (model.Blocked)
            {
                return 0;
            }
            if(model.FailedAttempts>=4)
                model.Blocked = true;
            model.FailedAttempts++;

          return  Update(model);
        }

        public int Update(BlockedIp model)
        {
             _apiDbContext.BlockedIps.Update( model);
           return   _apiDbContext.SaveChanges();
        }

        public int Delete(BlockedIp model)
        {
            _apiDbContext.BlockedIps.Remove(model);
            return _apiDbContext.SaveChanges();
        }

        public int update(Otp model)
        {
             _apiDbContext.Otps.Update(model);
             return  _apiDbContext.SaveChanges();
        }


    }
}
