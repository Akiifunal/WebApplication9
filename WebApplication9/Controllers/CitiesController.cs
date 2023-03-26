using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;
using WebApplication9.Models;

using WebApplication9.Services;

namespace WebApplication9.Controllers
{
    [ApiController]
	[Authorize]
	[ApiVersion("1.0")]
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/cities")]
    public class CitiesController:ControllerBase
    {

		private readonly IWebApplication9Repository _webApplication9Repository;
		private readonly IMapper _mapper;
		const int maxCitiesPageSize = 20;

		public CitiesController(IWebApplication9Repository webApplication9Repository,
			IMapper mapper)
			
        {
			_webApplication9Repository = webApplication9Repository ?? 
				throw new ArgumentNullException(nameof(WebApplication9Repository));
			_mapper= mapper ??
				throw new ArgumentNullException(nameof(mapper));
			

		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
			 string? name, string? searchQuery, int pageNumber=1,int pageSize =10)
        {

			var (cityEntities, paginationMetadata) = await _webApplication9Repository
				.GetCitiesAsync(name, searchQuery,pageNumber,pageSize);

			Response.Headers.Add("X-Pagination",
			  JsonSerializer.Serialize(paginationMetadata));

			return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
			
        }
		/// <summary>
		/// Get a city by id
		/// </summary>
		/// <param name="id">The id of the city to get</param>
		/// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
		/// <returns>An IActionResult</returns>
		/// <response code="200">Returns the requested city</response>

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetCity(
			int id, bool includePointsOfInterest = false)
        {
			var city =await  _webApplication9Repository.GetCityAsync(id, includePointsOfInterest);
			if(city == null)
			{
				return NotFound();
			}
			if(includePointsOfInterest)
			{
				return Ok(_mapper.Map<CityDto>(city));
			}
			return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));







		}
    }
}
