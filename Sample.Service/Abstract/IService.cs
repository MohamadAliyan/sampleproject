using Microsoft.EntityFrameworkCore.Query;
using Sample.Data.Entities;
using Sample.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util;

namespace Sample.Service.Abstract
{
  public interface IService<TInput, TResult>
        where TInput : BaseEntity
        where TResult : BaseServiceModel
    {
        IEnumerable<TResult> GetAll();
        PagingList<TResult> GetAll(int pageNumber = 1, int pageSize = 10);
        PagingList<TResult> GetAllbySearch(int pageNumber = 1, int pageSize = 10,
                                        Dictionary<string, dynamic> filterParams = null,
                                        Func<IQueryable<TInput>, IIncludableQueryable<TInput, object>> include = null,
                                        bool allIncluded = false);
        IEnumerable<TResult> GetAllbySearch(Dictionary<string, dynamic> filterParams = null,
            Func<IQueryable<TInput>, IIncludableQueryable<TInput, object>> include = null,
            bool allIncluded = false);
        TResult Get(long id);
        TResult GetLast();
        void Insert(TResult model, int currentUserId);
        long InsertAndGetId(TResult entity, int currentUserId);
        void Update(TResult model, int currentUserId);
        void Delete(long id, int currentUserId);

        IEnumerable<TResult> GetBy(Expression<Func<TResult, bool>> predicate);

    }





}
