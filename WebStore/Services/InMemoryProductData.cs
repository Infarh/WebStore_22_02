using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;

    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public IEnumerable<Product> GetProducts(ProductFilter? Filer = null)
    {
        IEnumerable<Product> query = TestData.Products;

        //if (Filer != null && Filer.SectionId != null)
        //    query = query.Where(p => p.SectionId == Filer.SectionId);

        if (Filer?.SectionId is { } section_id)
            query = query.Where(p => p.SectionId == section_id);

        if (Filer?.BrandId is { } brand_id)
            query = query.Where(p => p.BrandId == brand_id);

        return query;
    }
}
