using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniFacebook.Data;
using MiniFacebook.Extensions;
using MiniFacebook.Localization;
using MiniFacebook.Repository;
using NLog;
using System.Globalization;
using System.Text;


namespace MiniFacebook
{
    public class Startup
    {
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddDbContext<ApiDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
				o.SaveToken = true;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["JWT:Issuer"],
					ValidAudience = Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Key)
				};
			});
			services.Configure<RouteOptions>(options =>
			{
				options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
			});

			services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();
			services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IOTPRepository, OTPRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
			services.AddScoped<IPostTextRepository, PostTextRepository>();
			services.AddScoped<IImageRepository, ImageRepository>();
			services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IBlockedIpRepository, BlockedIpRepository>();

			services.AddControllers();
			services.AddSwaggerGen();

			services.AddLocalization(options => options.ResourcesPath = "Resources");
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
		{
			var path = Directory.GetCurrentDirectory();
			
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
				app.UseDeveloperExceptionPage();
				// Configure the HTTP request pipeline.

			}
			app.ConfigureCustomExceptionMiddleware();
			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication(); 
			app.UseAuthorization();

			IList<CultureInfo> supportedCultures = new List<CultureInfo>
				{
					new CultureInfo("en"),
					new CultureInfo("ar")
				};

			var requestLocalizationOptions = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			};
			var requestProvider = new RouteDataRequestCultureProvider();
			requestLocalizationOptions.RequestCultureProviders.Insert(0, requestProvider);
			var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(options.Value);


			app.UseEndpoints(endpoints =>
			{

				endpoints.MapControllerRoute(
					name: "LocalizedDefault",
					pattern: "{lang:lang}/{controller=Users}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
				   name: "default",
				   pattern: "{*catchall}",
					 defaults: new { controller = "Users", action = "RedirectToDefaultLanguage" }
				   );




			});
		}

	}
}
