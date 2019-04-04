using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSSFeed.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RSSFeed.Data.Interfaces;
using RSSFeed.Data;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service;
using Microsoft.EntityFrameworkCore;

namespace RSSFeed.Common
{
    public class PlatformInitializer : IPlatformInitializer
    {
        protected IConfiguration _configuration;

        public PlatformInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // configure database
            ConfigureDatabase(services);

            // add singletone instances
            services.AddSingleton(Mapping.Configuration.CreateDefaultMapper());

            // add scoped registrations
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<DbContext, RSSContext>(options =>
               options.UseMySql(
                   _configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
