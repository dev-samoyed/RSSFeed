using AutoMapper;
using RSSFeed.Data.Entities;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSSFeed.Service
{
    public class CategoryService : BaseQueryService<Category, CategoryModel, PostSortType>, ICategoryService
    {
        public CategoryService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        public void AddCategories(CategoryModel categoryModel, Guid channelId)
        {
            try
            {
                if (categoryModel.Name.Any(char.IsLower) && categoryModel.Name.Any(char.IsUpper) && categoryModel.Name != null)
                {
                    var existingCategories = _uow.GetRepository<Category>().All()
                                        .FirstOrDefault(x => x.Name == categoryModel.Name && x.ChannelId == channelId);

                    if (existingCategories != null)
                        return;

                    var category = _mapper.Map<Category>(categoryModel);
                    _uow.GetRepository<Category>().Insert(category);
                    _uow.SaveChanges();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Duplicate");
                return;
            }
        }

        public IEnumerable<CategoryModel> GetAllCategories(Guid channelId)
        {
            var categories = channelId == Guid.Empty ? _uow.GetRepository<Category>().All()
                                                     : _uow.GetRepository<Category>().All().Where(x => x.ChannelId == channelId);
            return _mapper.Map<IEnumerable<CategoryModel>>(categories.OrderBy(x => x.Name));
        }

        protected override IQueryable<Category> Category(IQueryable<Category> items, QuerySearch category)
        {
            return items;
        }

        protected override IQueryable<Category> Order(IQueryable<Category> items, bool isFirst, QueryOrder<PostSortType> order)
        {
            return items;
        }

        protected override IQueryable<Category> Search(IQueryable<Category> items, QuerySearch search)
        {
            return items;
        }

        protected override IQueryable<Category> SourceOrder(IQueryable<Category> items, QuerySearch source)
        {
            return items;
        }
    }
}
