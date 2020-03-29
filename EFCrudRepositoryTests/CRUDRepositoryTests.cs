using EFCrudRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace EFCrudRepositoryTests
{
    [TestClass]
    public class CRUDRepositoryTests
    {
        DbContextOptions<TestProductContext> options = null;

        [TestInitialize]
        public void Initialize()
        {
            options = new DbContextOptionsBuilder<TestProductContext>()
                .UseInMemoryDatabase(databaseName: "TestProducts")
                .Options;
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                foreach (var product in repo.FindAll().ToList())
                {
                    repo.Delete(product);
                }
            }
        }

        [TestMethod]
        public void Add_Creates_A_New_Entry()
        {
            
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);
                Assert.AreEqual(1, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_Creates_New_Entry()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                await repo.AddAsync(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                await repo.AddAsync(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);
                Assert.AreEqual(2, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public void Add_Without_Autosave_does_not_apply_changes_to_database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, false);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);
                Assert.AreEqual(0, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public void FindById_returns_entity()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);

                Assert.AreEqual("Test Product 01", product.Name);
            }
        }

        [TestMethod]
        public async Task FindByIdAsync_returns_entity()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = await repo.FindByIdAsync(1);

                Assert.AreEqual("Test Product 01", product.Name);
            }
        }

        [TestMethod]
        public void FindAll_With_Paging_Detail_Skips_Records()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
                repo.Add(new TestProduct() { Id = 3, Name = "Test Product 03", Price = 10 });
                repo.Add(new TestProduct() { Id = 4, Name = "Test Product 04", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var pagingDetail = new PagingDetail(pageSize: 2, pageNumber: 2);

                var products = repo.FindAll(pagingDetail);

                Assert.AreEqual(2, products.Count());
                Assert.AreEqual("Test Product 04", products.ElementAt(1).Name);
            }
        }

        [TestMethod]
        public async Task FindAllAsync_With_Paging_Detail_Skips_Records()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
                repo.Add(new TestProduct() { Id = 3, Name = "Test Product 03", Price = 10 });
                repo.Add(new TestProduct() { Id = 4, Name = "Test Product 04", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var pagingDetail = new PagingDetail(pageSize: 2, pageNumber: 1);

                var products = await repo.FindAllAsync(pagingDetail);

                Assert.AreEqual(2, products.Count());
            }
        }

        [TestMethod]
        public void FindAll_With_Negative_PageNumber_Returns_Empty_List()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var pagingDetail = new PagingDetail(pageSize: 1, pageNumber: -1);

                var products = repo.FindAll(pagingDetail);

                Assert.AreEqual(0, products.Count());
            }
        }

        [TestMethod]
        public async Task FindAllAsync_Returns_Entities()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);
                var products = await repo.FindAllAsync();

                Assert.AreEqual(2, products.Count());
            }
        }

        [TestMethod]
        public void Filter_Returns_Filtered_Entities()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 30 });
                repo.Add(new TestProduct() { Id = 3, Name = "Test Product 03", Price = 15 });
                repo.Add(new TestProduct() { Id = 4, Name = "Test Product 04", Price = 50 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);
                var products = repo.Filter(prod => prod.Price > 20).ToList();

                Assert.AreEqual(2, products.Count());
                Assert.AreEqual(2, products.ElementAt(0).Id);
                Assert.AreEqual(4, products.ElementAt(1).Id);
            }
        }

        [TestMethod]
        public void Update_With_AutoSave_Modifies_Entity_In_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);
                product.Name = "Modified Product";
                repo.Update(product);
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);

                Assert.AreEqual("Modified Product", product.Name);
            }
        }

        [TestMethod]
        public void Update_Returns_Updated_Entity()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);
                product.Name = "Modified Product";
                product = repo.Update(product);

                Assert.AreEqual("Modified Product", product.Name);
            }
        }

        [TestMethod]
        public void Update_Without_AutoSave_Does_not_Save_to_database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                var product = repo.FindById(1);
                product.Name = "Modified Product";
                repo.Update(product);
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);

                Assert.AreEqual("Test Product 01", product.Name);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_Updates_And_Returns_Updated_Entity()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);
                product.Name = "Modified Product";
                product = await repo.UpdateAsync(product);

                Assert.AreEqual("Modified Product", product.Name);
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);

                Assert.AreEqual("Modified Product", product.Name);
            }
        }

        [TestMethod]
        public void Delete_Removes_Entity_From_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);
                repo.Delete(product);

                Assert.AreEqual(1, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public async Task DeleteAsync_Removes_Entity_From_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                var product = repo.FindById(1);
                await repo.DeleteAsync(product);

                Assert.AreEqual(1, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public void Delete_Without_AutoSave_Does_Not_Commit_to_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                var product = repo.FindById(1);
                repo.Delete(product);
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);
                var products = repo.FindAll();

                Assert.AreEqual(2, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public void SaveChanges_Commit_Changes_To_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });

                repo.SaveChanges();
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                Assert.AreEqual(2, repo.FindAll().Count());
            }
        }

        [TestMethod]
        public async Task SaveChangesAsync_Commit_Changes_To_Database()
        {
            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                repo.Add(new TestProduct() { Id = 1, Name = "Test Product 01", Price = 10 });
                repo.Add(new TestProduct() { Id = 2, Name = "Test Product 02", Price = 20 });

                await repo.SaveChangesAsync();
            }

            using (var context = new TestProductContext(options))
            {
                var repo = new TestProductRepository(context, autoSave: false);

                Assert.AreEqual(2, repo.FindAll().Count());
            }
        }
    }
}
