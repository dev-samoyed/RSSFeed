using Microsoft.Extensions.DependencyInjection;

namespace RSSFeed.Common.Interfaces
{
    public interface IPlatformInitializer
    {
        void ConfigureServices(IServiceCollection services);
    }
}
