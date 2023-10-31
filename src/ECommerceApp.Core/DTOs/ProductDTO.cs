using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Core.DTOs;

public record ProductDTO(string Id, string Code, string GroupCode, decimal Price, decimal TotalQuantity,
                         bool IsItOffSale, string BrandId, List<string> CategoryIds, List<string> TagIds,
                         List<ProductImage> Images, List<ProductLanguage> ProductLanguages, int TaxRate,
                         List<ProductVariant> ProductVariants, List<ProductSpecification> ProductSpecifications);
