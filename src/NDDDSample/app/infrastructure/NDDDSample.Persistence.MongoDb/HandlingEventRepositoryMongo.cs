using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NDDDSample.Domain.Model.Cargos;
using NDDDSample.Domain.Model.Handlings;

namespace NDDDSample.Persistence.MongoDb
{
    #region Usings

    

    #endregion

    /// <summary>
    /// Hibernate implementation of HandlingEventRepository.
    /// </summary>
    public sealed class HandlingEventRepositoryMongo : IHandlingEventRepository
    {
        private readonly MongoDatabase db;
        private readonly MongoCollection<HandlingEvent> handlingEvents;

        public HandlingEventRepositoryMongo(MongoDatabase db)
        {
            this.db = db;
            handlingEvents = db.GetCollection<HandlingEvent>("handlingEvents");
        }

        #region IHandlingEventRepository Members

        public void Store(HandlingEvent evnt)
        {
            handlingEvents.Save(evnt);
        }


        public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId)
        {
            var cargo = new CargoRepositoryMongo(db).Find(trackingId);
            var data = handlingEvents.AsQueryable().Where(he => he.cargoId == cargo.id);
            return new HandlingHistory(data);
        }

        #endregion
    }
}