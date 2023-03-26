using AutoMapper;

namespace WebApplication9.Profiles
{
	public class CityProfile : Profile
	{
		public CityProfile()
		{
			CreateMap<Entities.City,Models. CityWithoutPointsOfInterestDto>();
			CreateMap<Entities.City, Models.CityDto>();

		}
			
	}
}
