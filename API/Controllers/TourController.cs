using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourController : Controller
    {
        private readonly ILogger<TourController> _logger;
        private readonly ITourService _tourService;
        private readonly IMapper _mapper;

        public TourController(
            ILogger<TourController> logger, 
            ITourService tourService,
            IMapper mapper)
        {
            _logger = logger;
            _tourService = tourService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllToursAsync()
        {
            var tours = await _tourService.GetAllToursAsync();
            return Ok(tours);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourByIdAsync(int id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);

            if(tour == null)
            {
                return NotFound($"{id} not found.");
            }

            return Ok(tour);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTourAsync([FromForm] CreateTourDto dto, IFormFile imageFile)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/assets/tourImages", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                dto.ImageUrl = $"/assets/tourImages/{fileName}";
            }

            await _tourService.CreateTourAsync(dto);

            return Ok(dto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTourAsync(int id, [FromForm] UpdateTourDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var existingTour = await _tourService.GetTourByIdAsync(id);

            if (existingTour == null)
            {
                return NotFound($"Tour with ID {id} not found.");
            }

            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/assets/tourImages", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                dto.ImageUrl = $"/assets/tourImages/{fileName}";
            }
            else
            {
                dto.ImageUrl = existingTour.ImageUrl; 
            }

            var updatedTour = await _tourService.UpdateTourAsync(id, dto);

            if (updatedTour == null)
                return BadRequest($"Tour with ID {id} could not be updated.");

            return Ok(updatedTour);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTourAsync(int id)
        {
            var isSuccess = await _tourService.DeleteTourByIdAsync(id);

            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok(isSuccess);
        }
    }
}
