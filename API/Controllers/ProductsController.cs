using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<ProductBrand> _brandRepo;
        private readonly IGenericRepo<ProductType> _typeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepo<Product> productRepo, IGenericRepo<ProductBrand> brandRepo, IGenericRepo<ProductType> typeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepo.GetAllBySpecAsync(spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepo.GetBySpecification(spec);
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var spec = new BrandsSpecification();
            var brands = await _brandRepo.GetAllBySpecAsync(spec);
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var spec = new TypesSpecification();
            var types = await _typeRepo.GetAllBySpecAsync(spec);
            return Ok(types);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // await _productRepo.DbMutationByProduct(_productRepo.Add, product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // await _repo.DbMutationById(_repo.DeleteProduct, id);
            return NoContent();
        }


    }
}