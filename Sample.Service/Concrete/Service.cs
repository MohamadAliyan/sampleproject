using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mapster;
using Sample.Data.Entities;
using Sample.Repository.Abstract;
using Sample.Service.Abstract;
using Sample.Service.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Util;

namespace Sample.Service.Concrete
{
    public class Service<TInput, TResult> : IService<TInput, TResult>

         where TResult : BaseServiceModel
         where TInput : BaseEntity
    {
        private readonly IRepository<TInput> _repository;
        private readonly IMemoryCache _memoryCache;

        public Service(IRepository<TInput> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }


        public virtual IEnumerable<TResult> GetAll()
        {
            var cacheKey = typeof(TResult).Name + CacheKeys.GetAll;
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<TResult> list))
                return list;
            list = _repository.GetAll().ToList().Adapt<IEnumerable<TResult>>();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            // Save data in cache.
            _memoryCache.Set(cacheKey, list, cacheEntryOptions);
            return list;

            //return _repository.GetAll().ToList().Adapt<IEnumerable<TResult>>();
        }

        public virtual PagingList<TResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            return _repository.GetAll(pageNumber, pageSize).Adapt<PagingList<TResult>>();

        }
        public virtual PagingList<TResult> GetAllbySearch(int pageNumber = 1, int pageSize = 10, Dictionary<string, dynamic> filterParams = null
            , Func<IQueryable<TInput>, IIncludableQueryable<TInput, object>> include = null,
            bool allIncluded = false)
        {
            return _repository.GetAllbySearch(pageNumber, pageSize, filterParams, include, allIncluded).Adapt<PagingList<TResult>>();
        }

        public virtual IEnumerable<TResult> GetAllbySearch(Dictionary<string, dynamic> filterParams = null
            , Func<IQueryable<TInput>, IIncludableQueryable<TInput, object>> include = null,
            bool allIncluded = false)
        {
            return _repository.GetAllbySearch(filterParams, include, allIncluded).AsEnumerable().Adapt<IEnumerable<TResult>>();
        }

        public virtual TResult Get(long id)
        {
            return _repository.Get(id).Adapt<TResult>();

        }

        public virtual TResult GetLast()
        {
            return _repository.GetLast().Adapt<TResult>();

        }



        public virtual void Insert(TResult serviceModel, int currentUserId)
        {
            _repository.Insert(serviceModel.Adapt<TInput>(), currentUserId);
            RemoveByKey();
        }
        private void RemoveByKey()
        {
            var cacheKey = typeof(TResult).Name + CacheKeys.GetAll;
            _memoryCache.Remove(cacheKey);
        }

        public virtual long InsertAndGetId(TResult serviceModel, int currentUserId)
        {

            return _repository.InsertAndGetId(serviceModel.Adapt<TInput>(), currentUserId);

        }


        public virtual void Update(TResult serviceModel, int currentUserId)
        {
            var props = typeof(TResult).GetProperties().Where(p => p.GetGetMethod().IsVirtual).Select(p => p.Name).ToArray();
            TypeAdapterConfig<TResult, TInput>.NewConfig()
                .Ignore(props)
                .Ignore(p => p.AddedDate)
                .Ignore(p => p.CreatorId)
                .Map(p => p.ModifiedDate, s => DateTime.Now)
                .Map(p => p.LastModifierId, s => currentUserId)

                ;
            TypeAdapterConfig<TInput, TInput>.NewConfig()
                .Ignore(props)
                .Ignore(p => p.AddedDate)
                .Ignore(p => p.CreatorId)

                .Map(p => p.ModifiedDate, s => DateTime.Now)
                .Map(p => p.LastModifierId, s => currentUserId);

            var model = _repository.Get(serviceModel.Id);
            var mapedEntity = serviceModel.Adapt(model);

            var m = mapedEntity.Adapt<TInput>();
            _repository.Update(m, currentUserId);
            RemoveByKey();


        }

        public virtual void Delete(long id, int currentUserId)
        {
            _repository.Delete(id, currentUserId);
            RemoveByKey();

        }
        //todo:mohamad felan lazem nist (no time)
        //public long AllocatedCount()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<TResult> FindAll(Expression<Func<TResult, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<TResult> GetBy(Expression<Func<TResult, bool>> predicate)
        {
            var ff = _repository.GetBy(predicate.ToReplaceParameter<TResult, TInput>())
                .AsEnumerable()
                .Adapt<IEnumerable<TResult>>();
            return ff;

        }
        //public PagingList<TResult> GetAllInclude(params Expression<Func<TResult, object>>[] includeProperties)
        //{
        //   // throw new NotImplementedException();
        //    return _repository.GetAllPagedListByInclude(includeProperties.Adapt<TInput>);
        //}
    }
    public static class ExpressionExtensions
    {
        public static Expression<Func<TTo, bool>> ToReplaceParameter<TFrom, TTo>(this Expression<Func<TFrom, bool>> target)
        {
            return (Expression<Func<TTo, bool>>)new WhereReplacerVisitor<TFrom, TTo>().Visit(target);
        }
        private class WhereReplacerVisitor<TFrom, TTo> : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTo), "c");

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                // replace parameter here
                return Expression.Lambda(Visit(node.Body), _parameter);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                // replace parameter member access with new type
                if (node.Member.DeclaringType == typeof(TFrom) && node.Expression is ParameterExpression)
                {
                    return Expression.PropertyOrField(_parameter, node.Member.Name);
                }
                return base.VisitMember(node);
            }
        }
    }

}
