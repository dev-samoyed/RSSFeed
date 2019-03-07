using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSSFeed.Common;
using RSSFeed.Data;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service;
using RSSFeed.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Console
{
    public class Startup
    {
        protected IConfigurationRoot _configuration;
        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;

        }

        public ServiceProvider Config()
        {
            return new ServiceCollection()
                .AddSingleton(Common.Mapping.Configuration.CreateDefaultMapper())
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IPostService, PostService>()
                .AddSingleton<IChannelService, ChannelService>()
                
                .AddDbContext<DbContext, RSSContext>(options =>
                       options.UseSqlServer(
                           _configuration.GetConnectionString("DefaultConnection")))
                .BuildServiceProvider();
        }
    }
}
