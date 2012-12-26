using NDDDSample.Persistence.MongoDb;

namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using System.Reflection;
    using NUnit.Framework;
    using Rhino.Commons;
    using Rhino.Commons.ForTesting;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class BaseRepositoryTest : DatabaseTestFixtureBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            LoadData();
        }


        private static void LoadData()
        {
            // TODO store Sample* and object instances here instead of handwritten SQL
            SampleDataGenerator.LoadSampleData();
        }

        protected static void Flush()
        {
            UnitOfWork.CurrentSession.Flush();
        }

        // Instead of exposing a getId() on persistent classes
        protected static int GetIntId(object o)
        {
            if (UnitOfWork.CurrentSession.Contains(o))
            {
                return (int) UnitOfWork.CurrentSession.GetIdentifier(o);
            }

            FieldInfo id = o.GetType().GetField("id");
            return (int) id.GetValue(o);
        }
    }
}