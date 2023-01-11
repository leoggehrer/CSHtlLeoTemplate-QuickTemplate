//@Ignore
#if GENERATEDCODE_ON
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickTemplate.Logic.Contracts;
using TAccessType = QuickTemplate.Logic.Models.TestPublic.Member;

namespace QuickTemplate.Logic.UnitTest
{
    [TestClass]
    public class MemberUnitTest : DataAccessUnitTest<TAccessType>
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
#if DBOPERATION_ON
            Task.Run(async () =>
            {
                await Logic.Modules.Database.DbManager.DeleteDatabaseAsync();
                await Logic.Modules.Database.DbManager.CreateDatabaseAsync();
            }).Wait();
#endif
        }
        public override IDataAccess<TAccessType> CreateDataAccess()
        {
            return new Logic.Controllers.TestPublic.MembersController();
        }

        [TestMethod]
        public void Create_And_Compare()
        {
            var entity = new TAccessType
            {
                FirstName = $"FirstName{Counter++,2}",
                LastName = $"LastName{Counter,2}",
            };

            Task.Run(async () =>
            {
                await Create_AccessModel_AndCheck(entity);
            }).Wait();
        }

        [TestMethod]
        public void Create10_000Entities_And_Compare()
        {
            List<TAccessType> entities = new List<TAccessType>();

            for (int i = 0; i < 10_000; i++)
            {
                entities.Add(new TAccessType
                {
                    FirstName = $"FirstName{Counter++,2}",
                    LastName = $"LastName{Counter,2}",
                });
            }

            Task.Run(async () =>
            {
                await CreateArray_AccessModels_AndCheckAll(entities);
            }).Wait();
        }

        [TestMethod]
        public void Update10_000Entities_And_Compare()
        {
            List<TAccessType> entities = new List<TAccessType>();
            List<TAccessType> updEntities = new List<TAccessType>();

            for (int i = 0; i < 10_000; i++)
            {
                entities.Add(new TAccessType
                {
                    FirstName = $"FirstName{Counter++,2}",
                    LastName = $"LastName{Counter,2}",
                });
                updEntities.Add(new TAccessType
                {
                    FirstName = $"FirstName{Counter++,2}-Update",
                    LastName = $"LastName{Counter,2}-Update",
                });
            }

            Task.Run(async () =>
            {
                await CreateUpdateArray_AccessModels_AndCheckAll(entities, updEntities);
            }).Wait();
        }

    }
}
#endif
