using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Core.DTOs;

public record SpecificationDTO(string Id, List<SpecificationLanguage> SpecificationLanguages);
