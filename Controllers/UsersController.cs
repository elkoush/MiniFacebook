using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MiniFacebook.Data;
using MiniFacebook.helper;
using MiniFacebook.Localization;
using MiniFacebook.Models;
using MiniFacebook.Models.users;
using MiniFacebook.Repository;

namespace MiniFacebook.Controllers
{
	[MiddlewareFilter(typeof(LocalizationPipeline))]
	[Authorize]
	[Route("{lang:lang}/[controller]/[action]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
		private readonly IUserRepository _userRepository;
        private readonly IBlockedIpRepository _blockedIpRepository;
		private ILoggerManager _logger;
		public UsersController(IConfiguration configuration,
			IJWTManagerRepository jWTManager,
			 IStringLocalizer<SharedResource> sharedLocalizer,
			 ILoggerManager logger,
			 IUserRepository userRepository,
			 IBlockedIpRepository blockedIpRepository)
		{
			_configuration = configuration;
			this._jWTManager = jWTManager;
			_sharedLocalizer = sharedLocalizer;
			_logger = logger;
			_userRepository = userRepository;
			_blockedIpRepository = blockedIpRepository;
		}


		[HttpGet]

		public JsonResult Get()

		{
            try
            {
				List<UserDisplayModel> result = new List<UserDisplayModel>();
				List<User> users = _userRepository.GetAll();
				foreach (User userItem in users)
				{
					result.Add(new UserDisplayModel()
					{
						Username = userItem.Username,
						Mobile = userItem.Mobile,
						MobileConfirmed = userItem.MobileConfirmed,
						AddedDate = userItem.AddedDate
					});
				}
				return new JsonResult(result);
			}
            catch (Exception ex)
            {

				_logger.LogError(ex.Message);
				return new JsonResult(BadRequest(new { status = 500, value = _sharedLocalizer.GetString("Something wrong").Value }));

			}
			
		}

		[AllowAnonymous]
		[HttpPost]
		public JsonResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
                try
                {
					var currentIp = HttpContext.Connection.RemoteIpAddress;
					
					if (currentIp != null)
					{
					   var blockingModel=	_blockedIpRepository.GetByIp(currentIp.ToString());
						if (blockingModel != null && blockingModel.Blocked)
							return new JsonResult(new { status = "100", value = _sharedLocalizer.GetString("Invali username or password").Value });
					}

				   model.Password= helper.Helper.EncryptText(model.Password,_configuration.GetValue<string>("AppSettings:EncryptPassword"));
					User? user = _userRepository.Login(model);
					if (user == null)
					{
						_logger.LogInfo("login Fail : username= " + model.Username + " password " + model.Password);
						
                        if (currentIp!=null)
                        {
							_blockedIpRepository.Add(currentIp.ToString());
						}
						
						return new JsonResult(new { status = "100", value = _sharedLocalizer.GetString("Invali username or password").Value });

					}
					if (user.MobileConfirmed == false)
					{
						_logger.LogInfo("Mobile not confirmed : username= " + model.Username + " password " + model.Password);
						return new JsonResult(new { status = "102", value = _sharedLocalizer.GetString("Mobile Not Confirmed").Value });
					}
					var token = _jWTManager.Authenticate(user);

					if (token == null)
					{
						return new JsonResult(Unauthorized());
					}

					return new JsonResult(token);
				}
                catch (Exception ex)
                {
					_logger.LogError(ex.Message);
					return new JsonResult(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
				}
               

			}
			else
				return new JsonResult( BadRequest(ModelState));
		}

		[AllowAnonymous]
		[HttpPost]
		public JsonResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				var currentIp = HttpContext.Connection.RemoteIpAddress;
				if (currentIp!=null)
                {
					int maxRegisterfromIp = int.Parse(_configuration.GetValue<string>("AppSettings:MaxRegiterFromIp"));

					if (_userRepository.GetUsersByIp(currentIp.ToString())>= maxRegisterfromIp)
                    {
						_logger.LogWarn("Max Register from the same Ip " + currentIp);
						return new JsonResult(new { status = "111", value = _sharedLocalizer.GetString("Register Fail").Value });
					}
                }

				if (_userRepository.GetByUserName(model.Username) !=null)
                {
					_logger.LogWarn("Exist User : username= " + model.Username + " password " + model.Password);
					return new JsonResult(new { status = "101", value = _sharedLocalizer.GetString("User Name is taken").Value });
				}
				string userIP = currentIp!=null? currentIp.ToString():"";
				User newUser = new User()
				{
					Username = model.Username,
					Password =  helper.Helper.EncryptText(model.Password, _configuration.GetValue<string>("AppSettings:EncryptPassword")),
					Mobile = model.Mobile,
					Ip = userIP,
					AddedDate=DateTime.UtcNow,
					MobileConfirmed=false
				};
				int userId = _userRepository.Register(newUser);
				if (userId == 0)
				{
					_logger.LogError ("Register Fail : username= " + model.Username +" password " +model.Password);
					return new JsonResult(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
					
				}
			    

				return new JsonResult(new {bit= "success" });

			}
			else
				return new JsonResult( BadRequest(ModelState));
		}

		

	}
}
