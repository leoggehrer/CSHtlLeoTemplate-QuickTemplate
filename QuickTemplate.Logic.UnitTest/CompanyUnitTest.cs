//@Ignore
#if GENERATEDCODE_ON
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickTemplate.Logic.Contracts;
using QuickTemplate.Logic.Models.TestInternal;
using TAccessType = QuickTemplate.Logic.Models.TestInternal.Company;

namespace QuickTemplate.Logic.UnitTest
{
    [TestClass]
    public partial class CompanyUnitTest : DataAccessUnitTest<Logic.Models.TestInternal.Company>
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

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }
        public override IDataAccess<Company> CreateDataAccess()
        {
            return new Logic.Facades.TestInternal.CompaniesFacade();
        }

        [TestMethod]
        public void Create_And_Compare()
        {
            var entity = new TAccessType
            {
                Name = $"Name{Counter++,2}",
                Address = $"LastName{Counter,2}",
            };

            Task.Run(async () =>
            {
                await Create_AccessModel_AndCheck(entity);
            }).Wait();
        }

        [TestMethod]
        public void Create1_000_000Entities_And_Compare()
        {
            List<TAccessType> entities = new List<TAccessType>();

            for (int i = 0; i < 1_000_000; i++)
            {
                entities.Add(new TAccessType
                {
                    Name = $"Name{Counter++,2}",
                    Address = $"Addrese{Counter,2}",
                });
            }

            Task.Run(async () =>
            {
                await CreateArray_AccessModels_AndCheckAll(entities);
            }).Wait();
        }

        [TestMethod]
        public void Update1_000_000Entities_And_Compare()
        {
            List<TAccessType> entities = new List<TAccessType>();
            List<TAccessType> updEntities = new List<TAccessType>();

            for (int i = 0; i < 1_000_000; i++)
            {
                entities.Add(new TAccessType
                {
                    Name = $"Name{Counter++,2}",
                    Address = $"Addresse{Counter,2}",
                });
                updEntities.Add(new TAccessType
                {
                    Name = $"FirstName{Counter++,2}-Update",
                    Address = $"Addresse{Counter,2}-Update",
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