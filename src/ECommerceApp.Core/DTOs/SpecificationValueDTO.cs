using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Core.DTOs;

public record SpecificationValueDTO(string Id, List<SpecificationValueLanguage> SpecificationValueLanguages, string SpecificationId);
