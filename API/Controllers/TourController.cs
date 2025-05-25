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
        public async Task<IActionResult> CreateTourAsync([FromBody] CreateTourDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tourService.CreateTourAsync(dto);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTourAsync(int id, [FromBody] UpdateTourDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tour = await _tourService.UpdateTourAsync(id, dto);

            if (tour == null)
                return BadRequest($"{tour.TourId} not found.");

            return Ok(tour);
        }

        [HttpDelete("{id}")]
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
