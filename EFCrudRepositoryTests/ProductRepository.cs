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
