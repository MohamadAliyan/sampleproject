using Microsoft.EntityFrameworkCore.Query;
using Sample.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Util;

namespace Sample.Repository.Abstract
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// خروجی متد به صورت IQueryable است
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// خروجی متد به صورت T است به صورت صفحه بندی 
        /// </summary>
        /// <param name="pageNumber">
        ///  شماره صفحه پیش فرض 1
        /// </param>
        /// <param name="pageSize">
        /// تعداد رکورد های برگشتی پیش فرض 10
        /// </param>
        /// <returns></returns>
        PagingList<T> GetAll(int pageNumber = 1, int pageSize = 10);

        /// <summary>
        ///  T می باشد متد فوق خروجی آن به صورت صفحه بندی 
        /// </summary>
        /// <param name="pageNumber">
        ///  شماره صفحه پیش فرض 1
        /// </param>
        /// <param name="pageSize">
        /// تعداد رکورد های برگشتی پیش فرض 10
        /// </param>
        /// <param name="filterParams">
        /// اضافه کردن شروط به صورت دیکشنری
        /// </param>
        /// <param name="include">
        /// درصورت موجود بودن به صورت navigation property با استفاده از الگوی لامبدا در قسمت کویری آن انتیتی شامل میشود 
        /// مثال: g => g.Include(r => r.xxx)
        /// </param>
        /// <param name="allIncluded">
        /// در صورت false تمامی انتیتی های یک مدل را برمیگرداند پیش فرض  false است
        /// </param>
        /// <returns></returns>
        PagingList<T> GetAllbySearch(int pageNumber = 1, int pageSize = 10,
            Dictionary<string, dynamic> filterParams = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool allIncluded = false);

        IQueryable<T> GetAllbySearch(Dictionary<string, dynamic> filterParams = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool allIncluded = false);
        T Get(long id);
        T GetLast();
        void Insert(T entity, int currentUserId);
        long InsertAndGetId(T entity, int currentUserId);
        void Update(T entity, int currentUserId);
        void Delete(long id, int currentUserId);
        void SaveChanges();
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get();
    }


}
