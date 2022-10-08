using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Insurance.Utility;
using Stripe;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Insurance.DataAccess.Initializer;
using Microsoft.AspNetCore.Mvc;
using Insurance.Models;
using Insurance.IdentityClaim;
using Insurance.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Insurance.UserService;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Insurance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }


        public IConfiguration Configuration { get; }
        public IUnitOfWork UnitOfWork { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{

            //    CryptorEngine cryptorEngine = new CryptorEngine(Configuration);

            //    options.UseSqlServer(cryptorEngine.Decrypt(Configuration.GetConnectionString("DefaultConnection"), true));
            //});




            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MyUserClaimsPrincipalFactory>();

            //services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            services.AddScoped<IUserService1, UserService1>();

            //add the following line of code
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MyUserClaimsPrincipalFactory>();

            services.AddSwaggerGen();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.Configure<EmailOptions>(Configuration);
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.Configure<BrainTreeSettings>(Configuration.GetSection("BrainTree"));
            services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));
            services.AddSingleton<IBrainTreeGate, BrainTreeGate>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.TryAddSingleton<IHttpContextAccessor, IHttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "The required is empty.");
            });
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ValidateNameParameterAttribute>();
            services.AddScoped<ApiAuthroization>();
            services.AddScoped<SingleMenuAttributeFilter>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "rewer";
                options.AppSecret = "erer";
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "er";
                options.ClientSecret = "erer";

            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

                            services.AddAuthentication(options =>
                            {
                                options.DefaultScheme = "JWT_OR_COOKIE";
                                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                            })
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/login";
                        options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["AuthSettings:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["AuthSettings:Audience"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"]))
                        };
                    })
                    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
                    {
                        options.ForwardDefaultSelector = context =>
                        {
                            string authorization = context.Request.Headers[HeaderNames.Authorization];
                            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                                return JwtBearerDefaults.AuthenticationScheme;

                            return CookieAuthenticationDefaults.AuthenticationScheme;
                        };
                    });

            services.AddMvc().AddRazorPagesOptions(options => {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer,IUnitOfWork unitOfWork)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            
            app.UseRouting();
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            dbInitializer.Initialize();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //------------------
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Claims App API");
            });

            //-------------------------
        }
    }
}
