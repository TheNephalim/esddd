using NDDDSample.Infrastructure.DI;

namespace NDDDSample.Domain.Model.Voyages
{
    public interface IVoyageRepository : IRequestLifeTimeDependency
    {
        /// <summary>
        /// Finds a voyage using voyage number.
        /// </summary>
        /// <param name="voyageNumber">voyage number</param>
        /// <returns> The voyage, or null if not found.</returns>
        Voyage Find(VoyageNumber voyageNumber);
    }
}