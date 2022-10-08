using System;
using Insurance.DataAccess.Data;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Insurance.Areas.Identity.IdentityHostingStartup))]
namespace Insurance.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {

        public IdentityHostingStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {


               
            });

           
        }
    }
}