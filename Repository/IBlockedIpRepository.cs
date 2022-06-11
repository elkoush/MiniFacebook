using MiniFacebook.Data;

namespace MiniFacebook.Repository
{
    public interface IBlockedIpRepository
    {
         BlockedIp? GetByIp(string ip);
         int Add(string ip);

         int Update(BlockedIp model);

         int Delete(BlockedIp model);

         int update(Otp model);
    }
}
