using RSSFeed.Data.Entities;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Models;
using RSSFeed.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Service.Interfaces
{
    public interface ICategoryService : IBaseQueryService<Category, CategoryModel, PostSortType>
    {
        IEnumerable<CategoryModel> GetAllCategories(Guid channelId);
        void AddCategories(CategoryModel category, Guid channelId);
    }
}
