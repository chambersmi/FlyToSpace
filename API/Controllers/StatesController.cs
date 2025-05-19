using API.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : ControllerBase
    {
        private readonly GetStatesService _statesService;

        public StatesController(GetStatesService statesService)
        {
            _statesService = statesService;
        }

        [HttpGet]
        public IActionResult GetStates()
        {
            var states = _statesService.GetStates();
            return Ok(states);
        }
    }
}
