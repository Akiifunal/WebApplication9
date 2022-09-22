using Microsoft.EntityFrameworkCore;
using WebApplication9.Entities;

namespace WebApplication9.DbContexts
{
    public class WebApplication9Context : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public WebApplication9Context(DbContextOptions<WebApplication9Context> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(new City("New York City")
            {
                Id = 1,
                Description = "The one with that big park."
            },
               new City("Antwerp")
               {
                   Id = 2,
                   Description = "The one with the cathedral that was never really finished."
               });
              

            modelBuilder.Entity<PointOfInterest>()
             .HasData(
               new PointOfInterest("Central Park")
               {
                   Id = 1,
                   CityId = 1,
                   Description = "The most visited urban park in the United States."
               },
               new PointOfInterest("Empire State Building")
               {
                   Id = 2,
                   CityId = 1,
                   Description = "A 102-story skyscraper located in Midtown Manhattan."
               },
                 new PointOfInterest("Cathedral")
                 {
                     Id = 3,
                     CityId = 2,
                     Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                 },
               new PointOfInterest("Antwerp Central Station")
               {
                   Id = 4,
                   CityId = 2,
                   Description = "The the finest example of railway architecture in Belgium."
               }
             

                );
            base.OnModelCreating(modelBuilder);
        }

       
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
          //  optionsBuilder.UseSqlite(connectionString);
            //base.OnConfiguring(optionsBuilder);

        //}
    }
}
