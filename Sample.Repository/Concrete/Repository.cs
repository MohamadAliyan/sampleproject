using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sample.Data.Entities;
using Sample.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Util;

namespace Sample.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseEntity

    {
        private readonly ILogger<T> _logger;
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _entity;
        string errorMessage = string.Empty;

        public Repository(ApplicationContext context, ILogger<T> logger)
        {
            this._context = context;
            _logger = logger;
            _entity = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entity;
        }
        public virtual PagingList<T> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            return _entity.ToPagedQuery(pageNumber, pageSize);
        }


        public virtual T GetLast()
        {
            return _entity.OrderByDescending(r => r.Id).First();
        }

        public T Get(long id)
        {
            return _entity.SingleOrDefault(s => s.Id == id);
        }


        public void Insert(T entity, int currentUserId)
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entity.AddedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.CreatorId = currentUserId;
            entity.LastModifierId = currentUserId;


            this._entity.Add(entity);
            _context.SaveChanges();
            try
            {
                _logger.LogInformation(LoggingEvents.InsertItem, new Exception("log"),
                    getString(entity, "Insert", currentUserId), entity);
            }
            catch { }

        }

        public long InsertAndGetId(T entity, int currentUserId)
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entity.AddedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.CreatorId = currentUserId;
            entity.LastModifierId = currentUserId;


            this._entity.Add(entity);
            _context.SaveChanges();

            try
            {
                _logger.LogInformation(LoggingEvents.InsertItem, new Exception("log"),
                    getString(entity, "InsertAndGetId", currentUserId), entity);
            }
            catch { }

            return entity.Id;

        }

        public void Update(T entity, int currentUserId)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                entity.ModifiedDate = DateTime.Now;
                entity.LastModifierId = currentUserId;

                _context.SaveChanges();

                try
                {
                    _logger.LogInformation(LoggingEvents.InsertItem, new Exception("log"),
                        getString(entity, "Update", currentUserId), entity);
                }
                catch { }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void Delete(long id, int currentUserId)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            this._entity.Remove(entity);
            _context.SaveChanges();

            try
            {
                _logger.LogInformation(LoggingEvents.InsertItem, new Exception("log"),
                    getString(entity, "Delete", currentUserId), entity);
            }
            catch { }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }


        public PagingList<T> GetAllbySearch(
            int pageNumber = 1, int pageSize = 10,
            Dictionary<string, dynamic> filterParams = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool allIncluded = false)
        {
            var query = _entity.AsQueryable();

            if (include != null && !allIncluded)
            {
                query = include(query);
            }
            if (allIncluded && include == null)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations()
                    .Where(r => !r.IsCollection()))
                    query = query.Include(property.Name);
            }
            if (filterParams != null && filterParams.Any())
            {
                if (filterParams.Any(r => r.Value != null))
                {
                    var expression = GetSearchFilter(filterParams);
                    return query.Where(expression).ToPagedQuery(pageNumber, pageSize);
                }
            }

            return query.ToPagedQuery(pageNumber, pageSize);

        }
        public IQueryable<T> GetAllbySearch(Dictionary<string, dynamic> filterParams = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool allIncluded = false)
        {
            var query = _entity.AsQueryable();
            if (include != null && !allIncluded)
            {
                query = include(query);
            }
            if (allIncluded && include == null)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations()
                    .Where(r => !r.IsCollection()))
                    query = query.Include(property.Name);
            }
            if (filterParams != null && filterParams.Any())
            {
                if (filterParams.Any(r => r.Value != null))
                {
                    var expression = GetSearchFilter(filterParams);
                    return query.Where(expression);
                }
            }

            return query;
        }


        private static Expression<Func<T, bool>> GetSearchFilter(Dictionary<string, dynamic> filterParams)
        {
            var eParam = Expression.Parameter(typeof(T), "t");

            Expression andExpression = Expression.IsTrue(Expression.Constant(true));
            foreach (var filter in filterParams.Where(r => r.Value != null))
            {
                var filterKey = filter.Key;
                if (filterKey == "FromDate")
                    filterKey = "StartDate";
                if (filterKey == "ToDate")
                    filterKey = "EndDate";
                if (filterKey == "Keyword")
                {
                    var stringProperties = typeof(T).GetProperties().Where(prop =>
                        prop.PropertyType == typeof(string));
                    Expression orExpression = Expression.IsTrue(Expression.Constant(false));
                    foreach (var propertyInfo in stringProperties)
                    {
                        var p = Expression.Property(eParam, propertyInfo.Name);
                        var returnType = ((PropertyInfo)p.Member).PropertyType;
                        var propConvert = Expression.Convert(Expression.Constant(filter.Value), returnType);
                        var x = Expression.Call(p, "Contains", null, propConvert);
                        orExpression = Expression.OrElse(orExpression, x);
                    }
                    andExpression = Expression.AndAlso(andExpression, orExpression);
                }
                else
                {
                    var p = Expression.Property(eParam, filterKey);
                    var returnType = ((PropertyInfo)p.Member).PropertyType;
                    if (returnType == typeof(DateTime))
                    {
                        DateTime datecurrent = (DateTime)filter.Value;
                        var dateFrom = datecurrent.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                        var dateTo = datecurrent.Date.AddHours(23).AddMinutes(59).AddSeconds(0);

                        if (filter.Key == "Date")
                        {
                            var x = Expression.GreaterThanOrEqual(p, (Expression.Constant(dateFrom)));
                            andExpression = Expression.AndAlso(andExpression, x);

                            var y = Expression.LessThanOrEqual(p, (Expression.Constant(dateTo)));
                            andExpression = Expression.AndAlso(andExpression, y);
                        }

                        if (filter.Key == "FromDate")
                        {
                            var x = Expression.GreaterThanOrEqual(p, (Expression.Constant(dateFrom)));
                            andExpression = Expression.AndAlso(andExpression, x);
                        }

                        if (filter.Key == "ToDate")
                        {
                            var y = Expression.LessThanOrEqual(p, (Expression.Constant(dateTo)));
                            andExpression = Expression.AndAlso(andExpression, y);
                        }

                        if (filter.Key != "Date" && filter.Key != "FromDate" && filter.Key != "ToDate")
                        {
                            //var propConvert = Expression.Convert(Expression.Constant(filter.Value), returnType);
                            //var x = Expression.Equal(p, propConvert);
                            //andExpression = Expression.AndAlso(andExpression, x);
                            var x = Expression.GreaterThanOrEqual(p, (Expression.Constant(dateFrom)));
                            andExpression = Expression.AndAlso(andExpression, x);

                            var y = Expression.LessThanOrEqual(p, (Expression.Constant(dateTo)));
                            andExpression = Expression.AndAlso(andExpression, y);
                        }
                    }
                    else
                    {

                        //if (p.Type != typeof(Nullable<>)){
                        //    var x = Expression.Equal(p, (Expression.Constant(filter.Value)));
                        //    andExpression = Expression.AndAlso(andExpression, x);
                        //}
                        //else
                        {
                            //var x = Expression.Equal(p, (Expression.Constant(filter.Value, p.Type)));
                            //andExpression = Expression.AndAlso(andExpression, x);

                            var propConvert = Expression.Convert(Expression.Constant(filter.Value), returnType);
                            var x = Expression.Equal(p, propConvert);
                            andExpression = Expression.AndAlso(andExpression, x);
                        }
                    }

                }
                // var p = Expression.Property(eParam, filter.Key);



            }

            //return ((Expression<Func<TLinqEntity, bool>>)lexList[lexList.Count - 1]).Compile();
            var xx = Expression.Lambda<Func<T, bool>>(andExpression, eParam);
            return xx;
        }

        public virtual IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate);
        }

        public IQueryable<T> Get()
        {
            return _entity.AsQueryable();
        }

        private string getString(T entity, string method, int currentUserId)
        {
            try
            {
                var st = Newtonsoft.Json.JsonConvert.SerializeObject(entity, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new ShouldSerializeContractResolver(),
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    });
                return $"{method} - userId {currentUserId}: {st}";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(LoggingEvents.InsertItem, new Exception("log"), $"{method} - userId {currentUserId} :" + ex.Message, entity);
                return $"{method} - userId {currentUserId}";
            }
        }

       
    

        public class ShouldSerializeContractResolver : DefaultContractResolver
        {
            public static ShouldSerializeContractResolver Instance { get; } = new ShouldSerializeContractResolver();

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    prop.ShouldSerialize = obj => false;
                }
                return prop;
            }
        }
    }

}
