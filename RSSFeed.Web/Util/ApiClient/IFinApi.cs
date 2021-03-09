using System.Collections.Generic;
using Refit;
using System.Threading.Tasks;
using RSSFeed.Service.Models;

namespace RSSFeed.Web.Util.ApiClient
{
    public interface IFinApi
    {
        [Multipart]
        [Post("api/values")]
        Task SendPostsAsync(
            List<PostModel> posts
        );
    }
}
