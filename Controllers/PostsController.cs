using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MiniFacebook.Data;
using MiniFacebook.helper;
using MiniFacebook.Localization;
using MiniFacebook.Models;
using MiniFacebook.Models.Post;
using MiniFacebook.Models.users;
using MiniFacebook.Repository;

namespace MiniFacebook.Controllers
{
	[MiddlewareFilter(typeof(LocalizationPipeline))]
	[Authorize]
	[Route("{lang:lang}/[controller]/[action]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly IPostTextRepository _postTextRepository;
		private readonly IImageRepository _imageRepository;
		private ILoggerManager _logger;
		public PostsController(IJWTManagerRepository jWTManager,
			 IStringLocalizer<SharedResource> sharedLocalizer,
			 ILoggerManager logger,
			 IUserRepository userRepository,
			 IPostRepository postRepository,
			 IImageRepository imageRepository ,
			 IPostTextRepository postTextRepository
			)
		{
			this._jWTManager = jWTManager;
			_sharedLocalizer = sharedLocalizer;
			_logger = logger;
			_userRepository = userRepository;
			_postRepository= postRepository;
			_postTextRepository= postTextRepository;
			_imageRepository= imageRepository;
		}


		[HttpGet]
		public JsonResult Get()

		{
            try
            {
				var  claimsPrincipal = HttpContext.User;
				if (claimsPrincipal == null)
					return new JsonResult(Unauthorized());
				string currenUuserId = _jWTManager.GetUserId(claimsPrincipal);
                if (string.IsNullOrEmpty(currenUuserId))
                {
					return new JsonResult(Unauthorized());
				}

			    var posts=	_postRepository.GetByUserId(Int64.Parse( currenUuserId));
				
				return new JsonResult(posts);
			}
            catch (Exception ex)
            {

				_logger.LogError(ex.Message);
				return new JsonResult(BadRequest(new { status = 500, value = _sharedLocalizer.GetString("Something wrong").Value }));

			}
			
		}

		[HttpPost]
		public JsonResult Add(PostModel model)
		{
			if (ModelState.IsValid)
			{
                try
                {
                    if (string.IsNullOrEmpty(model.PostText) && model.PostImage.Length==0 )
                    {
						ModelState.AddModelError("Data", _sharedLocalizer.GetString("No post Data").Value);
						return new JsonResult(BadRequest(ModelState));
					}


					var claimsPrincipal = HttpContext.User;
					if (claimsPrincipal == null)
						return new JsonResult(Unauthorized());
					string currenUuserId = _jWTManager.GetUserId(claimsPrincipal);
					if (string.IsNullOrEmpty(currenUuserId))
					{
						return new JsonResult(Unauthorized());
					}
					Post post = new Post();
					post.UserId = Int64.Parse( currenUuserId);
					if (!string.IsNullOrEmpty(model.PostText)){
					post.PostTextId=_postTextRepository.Add(new PostText { Text = model.PostText });
                    }
					if (model.PostImage.Length>0)
					{
						post.ImageId = _imageRepository.Add(new Image { ImageData = model.PostImage });
					}
					post.AddedDate = DateTime.UtcNow;
                    if (_postRepository.Add(post)>0)
						return new JsonResult(new { bit = "success",message= _sharedLocalizer.GetString("your post has been added").Value });
					else
						return new JsonResult(new { bit = "fail", message = _sharedLocalizer.GetString("your post not added").Value });




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

		[HttpDelete]
		public JsonResult Delete(Int64 postid)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (postid<=0 )
					{
						ModelState.AddModelError("PostId", _sharedLocalizer.GetString("Invalid Post id").Value);
						return new JsonResult(BadRequest(ModelState));
					}


					var claimsPrincipal = HttpContext.User;
					if (claimsPrincipal == null)
						return new JsonResult(Unauthorized());
					string currenUuserId = _jWTManager.GetUserId(claimsPrincipal);
					if (string.IsNullOrEmpty(currenUuserId))
					{
						return new JsonResult(Unauthorized());
					}
					Post post = new Post();
					post.UserId = Int64.Parse(currenUuserId);
					
					var existPost=_postRepository.GetById(postid);
					if (existPost == null)
						return new JsonResult(new { status = "600", value = _sharedLocalizer.GetString("Post not found").Value });
			
					 if(_postRepository.Delete(existPost)>0)
						return new JsonResult(new { bit = "success", message = _sharedLocalizer.GetString("your post has been deleted").Value });
					else
						return new JsonResult(new { bit = "fail", message = _sharedLocalizer.GetString("your post not deleted").Value });

				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
					return new JsonResult(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
				}


			}
			else
				return new JsonResult(BadRequest(ModelState));
		}


	}
}
