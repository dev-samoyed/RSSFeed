﻿using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSSFeed.Common;
using RSSFeed.Data;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RSSFeed.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
            var startup = new Startup(configuration);
            //setup DI
            
            var serviceProvider = startup.Config();
            var channelService = serviceProvider.GetService<IChannelService>();
            var postService = serviceProvider.GetService<IPostService>();

            // add channels, if not exist
            var channelModels = new List<ChannelModel>
            {
                new ChannelModel
                {
                    Title = "Habr",
                    Url = "http://habrahabr.ru/rss/",
                    Image = "https://habr.com/images/habr.png"
                },
                new ChannelModel
                {
                    Title = "24kg",
                    Url = "https://24.kg/rss/",
                    Image = "https://24.kg/assets/42adfee/images/logo.png"
                },
                new ChannelModel
                {
                    Title = "Sputnik Бишкек",
                    Url = "https://sputnik.kg/export/rss2/archive/index.xml",
                    Image = "https://ru.sputnik.kg/i/logo.png"
                },
                new ChannelModel
                {
                    Title = "Kaktus Media",
                    Url = "https://kaktus.media/?rss",
                    Image = "https://kaktus.media/lenta4/static/img/logo.png?2"
                }
            };

            foreach (var channel in channelModels)
            {
                channelService.AddChannel(channel);
            }
            
            foreach (var channel in channelService.GetChannels())
            {
                foreach (var channelItem in postService.FeedItems(channel))
                {
                    postService.AddPost(channelItem);
                }
            }

            var posts = postService.GetPosts();
            System.Console.WriteLine($"{posts.Where(x => x.IsNew).Count()} новых новостей");
            System.Console.WriteLine($"{posts.Where(x => x.IsSeen).Count()} просмотренных новостей");
            System.Console.WriteLine($"{posts.Count()} всего новостей");
            System.Console.ReadLine();
        }
    }
}
