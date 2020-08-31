using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepo _basketRepo;
        public BasketController(IBasketRepo basketRepo)
        {
            _basketRepo = basketRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(CustomerBasket basket)
        {
            var updated = await _basketRepo.UpdateBasketAsync(basket);

            return Ok(updated);
        }

        [HttpDelete]
        public async Task DeleteBasket([FromQuery] string id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }
    }
}