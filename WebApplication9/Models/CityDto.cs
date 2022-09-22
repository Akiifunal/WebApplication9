namespace WebApplication9.Models
{
    public class CityDto
    {
        public CityDto()
        {
            PointsOfInterest = new List<PointOfInterestDto>();
        }
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;    

        public string? Description{ get; set; }
        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
        






    }

    internal class pointOfInterestDto
    {
    }
}
