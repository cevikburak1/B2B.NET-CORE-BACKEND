using Business.Repositories.Product2Repository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product2sController : ControllerBase
    {
        private readonly IProduct2Service _product2Service;

        public Product2sController(IProduct2Service product2Service)
        {
            _product2Service = product2Service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(Product2 product2)
        {
            var result = await _product2Service.Add(product2);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(Product2 product2)
        {
            var result = await _product2Service.Update(product2);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(Product2 product2)
        {
            var result = await _product2Service.Delete(product2);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _product2Service.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _product2Service.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
