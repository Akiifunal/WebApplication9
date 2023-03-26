using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication9.Models;
using WebApplication9.Services;

namespace WebApplication9.Controllers
{
	[Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
	[Authorize(Policy = "MustBeFromAntwerp")]
	[ApiVersion("2.0")]
	[ApiController]
	public class PointsOfInterestController : ControllerBase
	{
		private readonly ILogger<PointsOfInterestController> _logger;
		private readonly IMailService _mailService;
		private readonly IWebApplication9Repository _webApplication9Repository;
		private readonly IMapper _mapper;

		public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
			IMailService mailService,
			IWebApplication9Repository webApplication9Repository,
			IMapper mapper)
		{
			_logger = logger ??
				throw new ArgumentNullException(nameof(logger));
			_mailService = mailService ??
				throw new ArgumentNullException(nameof(mailService));
			_webApplication9Repository = webApplication9Repository ??
				throw new ArgumentNullException(nameof(webApplication9Repository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(
			int cityId)
		{
			//var cityName= User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

			//if(!await _webApplication9Repository.CityNameMatchesCityId(cityName, cityId))
			//{
			//	return Forbid();
			//}
			
			
			if (!await _webApplication9Repository.CityExistsAsync(cityId))
			{
				_logger.LogInformation(
					$"City with id {cityId} wasn't found when accessing points of interest.");
				return NotFound();
			}

			var pointsOfInterestForCity = await _webApplication9Repository
				.GetPointsOfInterestForCityAsync(cityId);
			return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

		}




		[HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
		public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
		   int cityId, int pointOfInterestId)
		{
			if (!await _webApplication9Repository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var pointOfInterest = await _webApplication9Repository
				.GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);

			if (pointOfInterest == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
		}



		[HttpPost]
		public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
			int cityId,
					PointOfInterestForCreationDto pointOfInterest)
		{

			if (!await _webApplication9Repository.CityExistsAsync(cityId))

			{
				return NotFound();
			}
			var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

			 _webApplication9Repository.AddPointOfInterestForCityAsync(
				cityId, finalPointOfInterest);

			await _webApplication9Repository.SaveChangesAsync();

			var createdPointOfInterestToReturn =
				_mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);


			return CreatedAtRoute("GetPointOfInterest",
				new
				{
					cityId = cityId,
					pointofInterestId = createdPointOfInterestToReturn.Id
				},
				createdPointOfInterestToReturn);

		}

		[HttpPut("{pointofinterestid}")]
		public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
			PointOfInterestForUpdateDto pointOfInterest)
		{
			if (!await _webApplication9Repository.CityExistsAsync(cityId))
			{
				return NotFound();
			}
			var pointOfInterestEntity = await _webApplication9Repository
				.GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterestEntity == null)
			{
				return NotFound();
			}
			_mapper.Map(pointOfInterest, pointOfInterestEntity);

			await _webApplication9Repository.SaveChangesAsync();


			return NoContent();
		}

		[HttpPatch("{pointofinterestid}")]
		public async Task<ActionResult> PartiallyUpdatePointOfInterest(
			int cityId, int pointOfInterestId,
			JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
		{
			if(!await _webApplication9Repository.CityExistsAsync(cityId))
			{
				return NotFound();
			}
			var pointOfInterestEntity = await _webApplication9Repository
				.GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterestEntity == null)
			{
				return NotFound();
			}
			var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
				pointOfInterestEntity);

			patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!TryValidateModel(pointOfInterestToPatch))
			{
				return BadRequest(ModelState);
			}
			_mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
			await _webApplication9Repository.SaveChangesAsync();


			return NoContent();


        }
		[HttpDelete("{pointOfInterestId}")]
		public async Task<ActionResult> DeletePointOfInterest(
			int cityId, int pointOfInterestId)
		{
			if(!await _webApplication9Repository.CityExistsAsync(cityId))
			{
				return NotFound();
			}
			var pointOfInterestEntity = await _webApplication9Repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
			if(pointOfInterestEntity == null)
			{
				return NotFound();
			}


			_webApplication9Repository.DeletePointOfInterest(pointOfInterestEntity);
			_ = await _webApplication9Repository.SaveChangesAsync();

			_mailService.Send(
				"Point of interest deleted.",
				$"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");
			return NoContent();

		}






	}
}

    
       






































