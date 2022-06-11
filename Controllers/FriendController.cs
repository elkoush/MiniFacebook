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
	public class FriendController : ControllerBase
	{
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
		private readonly IUserRepository _userRepository;
		private readonly IFriendRepository _friendRepository;
		private ILoggerManager _logger;
		public FriendController(IJWTManagerRepository jWTManager,
			 IStringLocalizer<SharedResource> sharedLocalizer,
			 ILoggerManager logger,
			 IUserRepository userRepository,
			 IFriendRepository friendRepository)
		{
			this._jWTManager = jWTManager;
			_sharedLocalizer = sharedLocalizer;
			_logger = logger;
			_userRepository = userRepository;
			_friendRepository = friendRepository;
		}

		[HttpPost]
		public JsonResult AddFriend(string username)
		{
			
			try
				{
				if (string.IsNullOrEmpty(username))
				{
					ModelState.AddModelError("username", _sharedLocalizer.GetString("Is required").Value);
					return new JsonResult(BadRequest(ModelState));
                }
                User? friend = _userRepository.GetByUserName(username);
				if (friend ==null)
				{
					return new JsonResult(new { status = "103", value = _sharedLocalizer.GetString("Invali username").Value });
				}

				var claimsPrincipal = HttpContext.User;
				if (claimsPrincipal == null)
					return new JsonResult(Unauthorized());
				string currenUuserId = _jWTManager.GetUserId(claimsPrincipal);

				if (string.IsNullOrEmpty(currenUuserId))
				{
					return new JsonResult(Unauthorized());
				}

					User? user = _userRepository.GetById(Int64.Parse(currenUuserId));
					if (user == null)
					{
						return new JsonResult(Unauthorized());

					}
			Int64 result=	_friendRepository.Add(new Friend() { UserId = user.Id,
				                                     FriendId=friend.Id,
				                                     AddedDate=DateTime.UtcNow
				                                     });
                if (result<1)
                {
					return new JsonResult(new { bit = "fail", message = _sharedLocalizer.GetString("Friend Exist").Value });
				}

					return new JsonResult(new { bit = "success", message = _sharedLocalizer.GetString("your friend  has been added ").Value });
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
					return new JsonResult(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
				}


		}

		[HttpGet]
		public JsonResult GetFriends()
		{

			try
			{
			
				
				var claimsPrincipal = HttpContext.User;
				if (claimsPrincipal == null)
					return new JsonResult(Unauthorized());
				string currenUuserId = _jWTManager.GetUserId(claimsPrincipal);

				if (string.IsNullOrEmpty(currenUuserId))
				{
					return new JsonResult(Unauthorized());
				}

				User? user = _userRepository.GetById(Int64.Parse(currenUuserId));
				if (user == null)
				{
					return new JsonResult(Unauthorized());

				}
				var result = _friendRepository.GetByUserId(Int64.Parse(currenUuserId));

				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new JsonResult(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
			}


		}

	}
}
