using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NDDDSample.Domain.Model.Cargos;

namespace NDDDSample.Persistence.MongoDb
{
    #region Usings

    

    #endregion

    /// <summary>
    /// Hibernate implementation of CargoRepository.
    /// </summary>
    public sealed class CargoRepositoryMongo : ICargoRepository
    {
        private readonly MongoCollection<Cargo> cargoCollection;

        public CargoRepositoryMongo(MongoDatabase db)
        {
           cargoCollection = db.GetCollection<Cargo>("cargo");
        }
        #region ICargoRepository Members

        public Cargo Find(TrackingId tid)
        {
            return cargoCollection.AsQueryable().SingleOrDefault(c => c.trackingId.Id.Equals(tid.Id));

        }

        public void Store(Cargo cargo)
        {
            cargoCollection.Save(cargo);
            
            // Delete-orphan does not seem to work correctly when the parent is a component
            //Session.CreateSQLQuery("delete from Leg where cargo_id = null").ExecuteUpdate();
        }

        public TrackingId NextTrackingId()
        {
            // TODO use an actual DB sequence here, UUID is for in-mem
            string random = Guid.NewGuid().ToString().ToUpper();
            return new TrackingId(
                random.Substring(0, random.IndexOf("-"))
                );
        }

        public IList<Cargo> FindAll()
        {
            return cargoCollection.FindAll().ToList();
        }

        #endregion
    }
}