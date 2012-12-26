using NDDDSample.Persistence.MongoDb;

namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NUnit.Framework;
    using Rhino.Commons;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class HandlingEventRepositoryTest : BaseRepositoryTest
    {
        private IHandlingEventRepository handlingEventRepository;
        private ICargoRepository cargoRepository;
        private ILocationRepository locationRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var db = Utils.ShippingDb;
            handlingEventRepository = new HandlingEventRepositoryMongo(db);
            cargoRepository = new CargoRepositoryMongo(db);
            locationRepository = new LocationRepositoryMongo(db);
        }

        [Test]
        public void Save()
        {
            //TODO:atrosin make the method transaction  

            Location location = locationRepository.Find(new UnLocode("SESTO"));

            Cargo cargo = cargoRepository.Find(new TrackingId("XYZ"));
            var completionTime = new DateTime(2008, 2, 2);
            var registrationTime = new DateTime(2008, 3, 3);
            var evnt = new HandlingEvent(cargo, completionTime, registrationTime, HandlingType.CLAIM, location);

            handlingEventRepository.Store(evnt);

            Flush();

            IList list = GetPlainHandlingEventListFromDb(evnt);

            Assert.IsNotNull(list, "The object is not inserted");
            Assert.AreEqual(list.Count, 4, "The number of retrivied objects is not as expected");
            Assert.AreEqual(1, list[0] /*CARGO_ID*/);
            Assert.AreEqual(completionTime, list[1] /*COMPLETIONTIME*/);
            Assert.AreEqual(registrationTime, list[2] /*REGISTRATIONTIME*/);
            Assert.AreEqual("CLAIM", list[3] /*TYPE*/);
            // TODO: the rest of the columns
        }

        [Test]
        public void FindEventsForCargo()
        {
            var trackingId = new TrackingId("XYZ");
            IList<HandlingEvent> handlingEvents =
                handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId).DistinctEventsByCompletionTime();
            Assert.AreEqual(12, handlingEvents.Count);
        }

        private static IList GetPlainHandlingEventListFromDb(HandlingEvent evnt)
        {
            return
                UnitOfWork.CurrentSession.CreateSQLQuery(
                    "select CARGO_ID, COMPLETIONTIME, REGISTRATIONTIME, TYPE from HandlingEvent where id = ?")
                    .SetInt32(0, GetIntId(evnt))
                    .List()[0] as object[];
        }
    }
}