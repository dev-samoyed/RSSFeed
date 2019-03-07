using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Common.Mapping
{
    public class Configuration
    {
        private static Lazy<IConfigurationProvider> _defaultConfiguration =
            new Lazy<IConfigurationProvider>(() =>
                 new MapperConfiguration(config =>
                 {
                     config.AddProfile(new DataProfile());
                 })
            );

        public static IConfigurationProvider DefaultConfiguration => _defaultConfiguration.Value;

        public static IMapper CreateDefaultMapper() => new Mapper(DefaultConfiguration);
    }
}
