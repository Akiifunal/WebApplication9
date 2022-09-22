using WebApplication9.Models;

namespace WebApplication9
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        // public static CitiesDataStore Current { get; } = new CitiesDataStore();
       

        public CitiesDataStore() =>
            // init dummy data
            Cities = new List<CityDto>()
          {
               new CityDto()
               {
               Id = 1,
               Name = " New York City",
               Description = "The one that big park.",
               PointsOfInterest = new List<PointOfInterestDto>(){
                        new PointOfInterestDto(){

                            Id=1,
                            Name="Central Park",
                            Description="The most visited urban park in the United States."
                        },
                        new PointOfInterestDto() {


                             Id=2,
                             Name="Empire State Building",
                             Description="A 102-story skyscraper located in Midtown Manhattan."
                         },
                        }
               },
               new CityDto()
                {
                      Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                         new PointOfInterestDto() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDto() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium."
                         },
                    }










                      }
                };

        private CityDto NewCityDto()
        {
            throw new NotImplementedException();
        }
    }
}
