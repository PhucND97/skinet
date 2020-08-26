using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<ProductBrand> _brandRepo;
        private readonly IGenericRepo<ProductType> _typeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IGenericRepo<Product> productRepo, IGenericRepo<ProductBrand> brandRepo, IGenericRepo<ProductType> typeRepo, IMapper mapper, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductFilterWithCountSpecification(productParams);

            var totalItem = await _productRepo.CountAsync(countSpec);
            var products = await _productRepo.GetAllBySpecAsync(spec);
            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);
            
            return Ok(new Pagination<ProductToReturnDto> (productParams.PageIndex, productParams.PageSize, totalItem, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepo.GetBySpecification(spec);
            if (product == null) return NotFound(new ApiResponse(404));
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