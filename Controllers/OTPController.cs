using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MiniFacebook.Data;
using MiniFacebook.helper;
using MiniFacebook.Localization;
using MiniFacebook.Models;
using MiniFacebook.Models.Otp;
using MiniFacebook.Models.users;
using MiniFacebook.Repository;

namespace MiniFacebook.Controllers
{
	[MiddlewareFilter(typeof(LocalizationPipeline))]
	[Route("{lang:lang}/[controller]/[action]")]
	[ApiController]
	public class OTPController : ControllerBase
	{

        private readonly IConfiguration _configuration;
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
		private readonly IOTPRepository _otpRepository;
        private readonly IUserRepository _userRepository;

        private ILoggerManager _logger;
		public OTPController(IConfiguration configuration,
            IJWTManagerRepository jWTManager,
			 IStringLocalizer<SharedResource> sharedLocalizer,
			 ILoggerManager logger,
			 IOTPRepository otpRepository,
             IUserRepository userRepository)
		{
            _configuration = configuration;
			this._jWTManager = jWTManager;
			_sharedLocalizer = sharedLocalizer;
			_logger = logger;
			_otpRepository = otpRepository;
            _userRepository = userRepository;

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Add(string PhoneNumber)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User? existUser = _userRepository.GetByPhoneNumber(PhoneNumber);
                    if (existUser == null)
                    {
                        return Ok(new { status = 3003, value = _sharedLocalizer.GetString("phone number  not Exist ").Value });
                       
                    }
                    if (existUser.MobileConfirmed)
                    {
                        return Ok(new { status = 3004, value = _sharedLocalizer.GetString("phone number  confirmed before  ").Value });
                    }


                    Random rnd = new Random();
                    Otp otp = new Otp();
                    otp.Mobile = PhoneNumber;
                    otp.Code=rnd.Next(10000,99999).ToString();
                    otp.CreationDate = DateTime.UtcNow;
                    otp.VerificationCount = 0;
                    _otpRepository.Add(otp);
                    return Ok(new { otp = otp.Code, message = _sharedLocalizer.GetString("This code is Valid for 30 Minutes").Value });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Ok(new { status = 500, value = _sharedLocalizer.GetString("Something wrong").Value });
                }


            }
            else
                return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPut]
        public IActionResult Verification(VerificationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int maxVerification = int.Parse(_configuration.GetValue<string>("AppSettings:maxVerification"));

                    Otp? Existmodel=   _otpRepository.GetByMobile(model.phoneNumber);
                    if (Existmodel == null)
                    {
                        return Ok(new { status = "300", value = _sharedLocalizer.GetString("Wrong Phone number or code").Value });
                    }
                    if (Existmodel.VerificationCount >= maxVerification)
                    {
                        return Ok(new { status = "301", value = _sharedLocalizer.GetString("you are tried more 3 times ").Value });
                    }
                    if (Existmodel.Code != model.code)
                    {
                        Existmodel.VerificationCount ++;
                        _otpRepository.update(Existmodel);
                        return Ok(new { status = "300", value = _sharedLocalizer.GetString("Wrong Phone number or code").Value });

                    }
                    if (Existmodel.CreationDate.AddMinutes(30) < DateTime.UtcNow)
                    {
                        return Ok(new { status = "300", value = _sharedLocalizer.GetString("your code Is expired ").Value });
                    }
                    User? existUser = _userRepository.GetByPhoneNumber(model.phoneNumber);


                    if (existUser ==null)
                    {
                        return Ok(new { status = "3005", value = _sharedLocalizer.GetString("User not Exist ").Value });
                    }
                    if (existUser.MobileConfirmed)
                    {
                        return Ok(new { status = "3004", value = _sharedLocalizer.GetString("phone number  confirmed before  ").Value });
                    }
                    int result = _userRepository.verifyByPhoneNumber(model.phoneNumber);

                    if (result<1)
                    {
                        return Ok(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
                    }

                        var token = _jWTManager.Authenticate(existUser);
                        if (token == null)
                        {
                            return Unauthorized();
                        }

                        return Ok(token);
                    

                   

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Ok(new { status = "500", value = _sharedLocalizer.GetString("Something wrong").Value });
                }


            }
            else
                return Ok(ModelState);
        }




    }
}
