# EntityFramework Core Repository Implementation
This small library was designed to enable .NET Core developers to speed up development time by providing a generic base repository for Create, Read, Update and Delete operations

## Dependencies
Microsoft.EntityFrameworkCore >= 3.1.2

## Interface
```
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
```

## Usage

Consider you have a `TestProduct` model with an `int` primary key field and `TestProductContext` is your DbContext 
Below is how you would create a `TestProducRepository` (do not forget to register your DbContext in Startup.cs):

```
using EFCrudRepository;

namespace EFCrudRepositoryTests
{
    public class TestProductRepository : CRUDRepository<TestProduct, int, TestProductContext>
    {
        public TestProductRepository(TestProductContext context) : base(context)
        {

        }

        public TestProductRepository(TestProductContext context, bool autoSave) : base(context, autoSave) 
        {

        }
    }
}
```
