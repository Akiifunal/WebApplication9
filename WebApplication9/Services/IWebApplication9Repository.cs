using WebApplication9.Entities;

namespace WebApplication9.Services
{
	public interface IWebApplication9Repository
	{
		Task<IEnumerable<City>> GetCitiesAsync();
		Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
		Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

		Task<bool> GetCityAsync(int cityId);
		Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId) ;
		Task<PointOfInterest?> GetPointOfInterestForPointOfInterestAsync(int cityId, int pointOfInterestId);
		Task<bool> CityExistsAsync(int cityId);
		Task<PointOfInterest> GetPointsOfInterestForCityAsync(int cityId, int pointOfInterestId);
		Task AddPointOfInterestForCityAsync(int cityId,PointOfInterest pointOfInterest);
		void DeletePointOfInterest(PointOfInterest pointOfInterest);

		Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
		Task<bool> SaveChangesAsync();
		Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
		Task<PointOfInterest> DeletePointOfInterest(Task pointOfInterestEntity);
		
	}
}
