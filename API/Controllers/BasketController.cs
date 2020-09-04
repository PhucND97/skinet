using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepo basketRepo, IMapper mapper)
        {
            _mapper = mapper;
            _basketRepo = basketRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(CustomerBasketDto basketDto)
        {
            var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
            
            var updated = await _basketRepo.UpdateBasketAsync(customerBasket);

            return Ok(customerBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket([FromQuery] string id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }
    }
}