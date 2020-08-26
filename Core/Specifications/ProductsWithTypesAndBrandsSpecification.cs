using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // public ProductsWithTypesAndBrandsSpecification()
        // {
        //     AddInclude(p => p.ProductType);
        //     AddInclude(p => p.ProductBrand);
        // }
        public ProductsWithTypesAndBrandsSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
                : base(p =>
                            (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
                            (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
                            (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)
                        )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddOrderBy(p => p.Name);
            ApplyPaging(skip: productParams.PageSize * (productParams.PageIndex - 1), take: productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDes":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}