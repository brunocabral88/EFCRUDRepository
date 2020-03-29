using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFCrudRepository
{
    public class CRUDRepository<TModel, TPrimaryKey, TDbContext> : ICRUDRepository<TModel, TPrimaryKey>
        where TModel: class
        where TPrimaryKey: struct
        where TDbContext: DbContext
    {
        private readonly TDbContext dbContext;

        public bool AutoSave { get; } = true;

        public CRUDRepository(TDbContext dbContext, bool autoSave)
        {
            this.dbContext = dbContext;
            AutoSave = autoSave;
        }

        public CRUDRepository(TDbContext dbContext) : this(dbContext, true)
        {

        }

        public virtual TModel FindById(TPrimaryKey id)
        {
            return dbContext.Set<TModel>().Find(id);
        }

        public async virtual Task<TModel> FindByIdAsync(TPrimaryKey id)
        {
            return await dbContext.Set<TModel>().FindAsync(id);
        }

        public virtual IEnumerable<TModel> FindAll()
        {
            return dbContext.Set<TModel>().ToList();
        }

        public virtual IEnumerable<TModel> FindAll(PagingDetail pagingDetail)
        {
            if (pagingDetail.PageSize <= 0 || pagingDetail.PageNumber < 0)
            {
                return new List<TModel>();
            }

            var results = dbContext.Set<TModel>().Skip((pagingDetail.PageNumber * pagingDetail.PageSize) - pagingDetail.PageSize);
            results = results.Take(pagingDetail.PageSize);

            return results.ToList();
        }

        public async virtual Task<IEnumerable<TModel>> FindAllAsync(PagingDetail pagingDetail)
        {
            if (pagingDetail.PageSize <= 0 || pagingDetail.PageNumber < 0)
            {
                return new List<TModel>();
            }

            var results = dbContext.Set<TModel>().Skip((pagingDetail.PageNumber * pagingDetail.PageSize) - pagingDetail.PageSize);
            results = results.Take(pagingDetail.PageSize);

            return await results.ToListAsync();
        }

        public async virtual Task<IEnumerable<TModel>> FindAllAsync()
        {
            return await dbContext.Set<TModel>().ToListAsync();
        }

        public virtual IQueryable<TModel> Filter(Expression<Func<TModel, bool>> expression)
        {
            return dbContext.Set<TModel>().Where(expression);
        }

        public virtual TModel Add(TModel entity)
        {
            dbContext.Set<TModel>().Add(entity);

            if (AutoSave)
            {
                dbContext.SaveChanges();
            }

            return entity;
        }

        public async virtual Task<TModel> AddAsync(TModel entity)
        {
            dbContext.Set<TModel>().Add(entity);

            if (AutoSave)
            {
                await dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public virtual TModel Update(TModel entity)
        {
            dbContext.Attach(entity).State = EntityState.Modified;

            if (AutoSave)
            {
                dbContext.SaveChanges();
            }

            return entity;
        }

        public async virtual Task<TModel> UpdateAsync(TModel entity)
        {
            dbContext.Attach(entity).State = EntityState.Modified;

            if (AutoSave)
            {
                await dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public virtual void Delete(TModel entity)
        {
            dbContext.Set<TModel>().Remove(entity);

            if (AutoSave)
            {
                dbContext.SaveChanges();
            }
        }

        public virtual async Task DeleteAsync(TModel entity)
        {
            dbContext.Set<TModel>().Remove(entity);

            if (AutoSave)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public virtual void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
