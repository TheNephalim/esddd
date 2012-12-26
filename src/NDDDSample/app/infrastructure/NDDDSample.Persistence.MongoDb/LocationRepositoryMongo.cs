using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using NDDDSample.Domain.Model.Locations;

namespace NDDDSample.Persistence.MongoDb
{
    #region Usings

    

    #endregion

    public sealed class LocationRepositoryMongo : ILocationRepository
    {
        private readonly MongoCollection<Location> locations;

        public LocationRepositoryMongo(MongoDatabase db)
        {
            locations = db.GetCollection<Location>("locations");
        }
        #region ILocationRepository Members

        public Location Find(UnLocode unLocode)
        {
            return locations.AsQueryable().SingleOrDefault(l => l.UnLocode.Id == unLocode.Id);
        }

        public IList<Location> FindAll()
        {
            return locations.FindAll().ToList();
        }

        #endregion
    }
}