using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCrudRepository
{
    public interface ICRUDRepository<TModel, TPrimaryKey>
        where TModel : class
        where TPrimaryKey : struct
    {
        bool AutoSave { get; }

        // CREATE
        TModel Add(TModel entity);
        Task<TModel> AddAsync(TModel entity);

        // READ
        IEnumerable<TModel> FindAll();
        IEnumerable<TModel> FindAll(PagingDetail pagingDetail);
        Task<IEnumerable<TModel>> FindAllAsync();
        Task<IEnumerable<TModel>> FindAllAsync(PagingDetail pagingDetail);
        TModel FindById(TPrimaryKey id);
        Task<TModel> FindByIdAsync(TPrimaryKey id);
        IQueryable<TModel> Filter(Expression<Func<TModel, bool>> expression);

        // UPDATE
        TModel Update(TModel entity);
        Task<TModel> UpdateAsync(TModel entity);

        // DELETE
        void Delete(TModel entity);
        Task DeleteAsync(TModel entity);


        void SaveChanges();
        Task SaveChangesAsync();
    }
}