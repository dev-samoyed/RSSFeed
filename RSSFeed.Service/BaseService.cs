using AutoMapper;
using RSSFeed.Data.Interfaces;
using RSSFeed.Services.Interfaces;
using System;

namespace RSSFeed.Service
{
    public class BaseService : IBaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;

        protected Guid Id => Guid.NewGuid();

        public BaseService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
    }
}
