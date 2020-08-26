using Core.Entities;

namespace Core.Specifications
{
    public class ProductFilterWithCountSpecification : BaseSpecification<Product>
    {
        public ProductFilterWithCountSpecification(ProductSpecParams productParams)
            : base(p =>
                    (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
                    (!productParams.BrandId.HasValue || productParams.BrandId == p.ProductBrandId) &&
                    (!productParams.TypeId.HasValue || productParams.TypeId == p.ProductTypeId))
        { }
    }
}