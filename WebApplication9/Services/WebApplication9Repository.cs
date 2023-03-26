using Microsoft.EntityFrameworkCore;
using WebApplication9.DbContexts;
using WebApplication9.Entities;

namespace WebApplication9.Services
{
	public class WebApplication9Repository : IWebApplication9Repository
	{
		private WebApplication9Context _context;
		private int cityId;

		public WebApplication9Repository(WebApplication9Context context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}
		public async Task<IEnumerable<City>> GetCitiesAsync()
		{
			return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
		}

		public async Task <bool> CityNameMatchesCityId(string? cityName,int cityId)
		{
			return await _context.Cities.AnyAsync(c =>c.Id == cityId && c.Name == cityName);	
		}
		public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
		{

			// collection to start from
			var collection = _context.Cities as IQueryable<City>;

			if (!string.IsNullOrEmpty(name))
			{
				name = name.Trim();
				collection = collection.Where(c => c.Name == name);
			}
			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				searchQuery = searchQuery.Trim();
				collection = collection.Where(a => a.Name.Contains(searchQuery)
					|| (a.Description != null && a.Description.Contains(searchQuery)));
			}
			var totalItemCount = await collection.CountAsync();

			var paginationMetadata= new PaginationMetadata(
				totalItemCount, pageSize, pageNumber);

			var collectionToReturn =  await collection.OrderBy(c => c.Name)
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToListAsync();



			return(collectionToReturn, paginationMetadata);
		}


		public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
		{
			if (includePointsOfInterest)
			{
				return await _context.Cities.Include(c => c.PointsOfInterest)
					.Where(c => c.Id == cityId).FirstOrDefaultAsync();
			}
			return await _context.Cities
				.Where(c => c.Id == cityId).FirstOrDefaultAsync();
		}

		public async Task<bool> CityExistsAsync(int cityId)
		{
			return await _context.Cities.AnyAsync(c => c.Id == cityId);
		}



		public async Task<PointOfInterest?> GetPointOfInterestForPointOfInterestAsync(
			int cityId,
			int pointOfInterestId)
		{
			return await _context.PointsOfInterest
				.Where(p => p.CityId == cityId && p.Id == pointOfInterestId )
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
		{
			return await _context.PointsOfInterest
				.Where(p => p.CityId == cityId).ToListAsync();
		}


		public async Task<PointOfInterest> GetPointsOfInterestForCityAsync(int cityId, int pointOfInterestId)
		{
			return await _context.PointsOfInterest.Where(x => x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefaultAsync();
		}
		public async Task AddPointOfInterestForCityAsync(int cityId,
			PointOfInterest pointOfInterest)
		{
			var city = await GetCityAsync(cityId, false);
			if (city != null)
			{
				city.PointsOfInterest.Add(pointOfInterest);
			}
		}
		public void DeletePointOfInterest(PointOfInterest pointOfInterest)
		{
			_context.PointsOfInterest.Remove(pointOfInterest);
		}
		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task<bool> GetCityAsync(int cityId)
		{
			throw new NotImplementedException();
		}


		public async Task<PointOfInterest> DeletePointOfInterest(int pointOfInterestEntity)
		{
			throw new NotImplementedException();
		}

		public async Task<PointOfInterest> DeletePointOfInterest()
		{
			throw new NotImplementedException();
		}

		public void DeletePointOfInterest(Task pointOfInterestEntity)
		{
			throw new NotImplementedException();
		}

		Task<PointOfInterest> IWebApplication9Repository.DeletePointOfInterest(Task pointOfInterestEntity)
		{
			throw new NotImplementedException();
		}

		public Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
		{
			throw new NotImplementedException();
		}
	}
}
