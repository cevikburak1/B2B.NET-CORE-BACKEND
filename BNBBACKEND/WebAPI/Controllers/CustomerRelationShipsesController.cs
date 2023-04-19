using Business.Repositories.CustomerRelationShipsRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRelationShipsesController : ControllerBase
    {
        private readonly ICustomerRelationShipsService _customerRelationShipsService;

        public CustomerRelationShipsesController(ICustomerRelationShipsService customerRelationShipsService)
        {
            _customerRelationShipsService = customerRelationShipsService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CustomerRelationShips customerRelationShips)
        {
            var result = await _customerRelationShipsService.Add(customerRelationShips);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CustomerRelationShips customerRelationShips)
        {
            var result = await _customerRelationShipsService.Update(customerRelationShips);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(CustomerRelationShips customerRelationShips)
        {
            var result = await _customerRelationShipsService.Delete(customerRelationShips);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _customerRelationShipsService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerRelationShipsService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
