using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NDDDSample.Domain.Model.Voyages;

namespace NDDDSample.Persistence.MongoDb
{
    #region Usings

    

    #endregion

    /// <summary>
    /// Hibernate implementation of CarrierMovementRepository.
    /// </summary>
    public sealed class VoyageRepositoryMongo : IVoyageRepository
    {
        private MongoCollection<Voyage> voyages;

        public VoyageRepositoryMongo(MongoDatabase db)
        {
            voyages = db.GetCollection<Voyage>("voyages");
        }
        #region IVoyageRepository Members

        public Voyage Find(VoyageNumber voyageNumber)
        {
            return voyages.AsQueryable().SingleOrDefault(v => v.voyageNumber.id == voyageNumber.id); 
        }

        #endregion
    }
}