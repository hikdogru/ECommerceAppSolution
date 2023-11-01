using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace WebUI.Test;

public static class Items
{
    public static IEnumerable<Category> GetCategories()
    {
        var categories = new List<Category>()
        {
            // Test Category objects
            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bce"),
                ParentId = "ParentId1",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId1",
                        LanguageId = "LanguageId1",
                        LanguageCode = "en",
                        Name = "Category Name 1",
                        Description = "Category Description 1",
                        SortNr = 1
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId1",
                        LanguageId = "LanguageId1",
                        LanguageCode = "en",
                        MediaType = 1,
                        SizeType = 2,
                        Title = "Media Title 1",
                        Path = "Media Path 1"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"),
                ParentId = "ParentId2",
                IsActive = false,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId2",
                        LanguageId = "LanguageId2",
                        LanguageCode = "es",
                        Name = "Category Name 2",
                        Description = "Category Description 2",
                        SortNr = 2
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId2",
                        LanguageId = "LanguageId2",
                        LanguageCode = "es",
                        MediaType = 2,
                        SizeType = 1,
                        Title = "Media Title 2",
                        Path = "Media Path 2"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcb"),
                ParentId = "ParentId3",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId3",
                        LanguageId = "LanguageId3",
                        LanguageCode = "fr",
                        Name = "Category Name 3",
                        Description = "Category Description 3",
                        SortNr = 3
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId3",
                        LanguageId = "LanguageId3",
                        LanguageCode = "fr",
                        MediaType = 3,
                        SizeType = 3,
                        Title = "Media Title 3",
                        Path = "Media Path 3"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcc"),
                ParentId = "ParentId4",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId4",
                        LanguageId = "LanguageId4",
                        LanguageCode = "de",
                        Name = "Category Name 4",
                        Description = "Category Description 4",
                        SortNr = 4
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId4",
                        LanguageId = "LanguageId4",
                        LanguageCode = "de",
                        MediaType = 4,
                        SizeType = 4,
                        Title = "Media Title 4",
                        Path = "Media Path 4"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcd"),
                ParentId = "ParentId5",
                IsActive = false,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId5",
                        LanguageId = "LanguageId5",
                        LanguageCode = "it",
                        Name = "Category Name 5",
                        Description = "Category Description 5",
                        SortNr = 5
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId5",
                        LanguageId = "LanguageId5",
                        LanguageCode = "it",
                        MediaType = 5,
                        SizeType = 5,
                        Title = "Media Title 5",
                        Path = "Media Path 5"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bae"),
                ParentId = "ParentId6",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId6",
                        LanguageId = "LanguageId6",
                        LanguageCode = "pt",
                        Name = "Category Name 6",
                        Description = "Category Description 6",
                        SortNr = 6
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId6",
                        LanguageId = "LanguageId6",
                        LanguageCode = "pt",
                        MediaType = 6,
                        SizeType = 6,
                        Title = "Media Title 6",
                        Path = "Media Path 6"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bbe"),
                ParentId = "ParentId7",
                IsActive = false,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId7",
                        LanguageId = "LanguageId7",
                        LanguageCode = "ja",
                        Name = "Category Name 7",
                        Description = "Category Description 7",
                        SortNr = 7
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId7",
                        LanguageId = "LanguageId7",
                        LanguageCode = "ja",
                        MediaType = 7,
                        SizeType = 7,
                        Title = "Media Title 7",
                        Path = "Media Path 7"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9dabce"),
                ParentId = "ParentId8",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId8",
                        LanguageId = "LanguageId8",
                        LanguageCode = "ko",
                        Name = "Category Name 8",
                        Description = "Category Description 8",
                        SortNr = 8
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId8",
                        LanguageId = "LanguageId8",
                        LanguageCode = "ko",
                        MediaType = 8,
                        SizeType = 8,
                        Title = "Media Title 8",
                        Path = "Media Path 8"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6cce"),
                ParentId = "ParentId9",
                IsActive = false,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId9",
                        LanguageId = "LanguageId9",
                        LanguageCode = "zh",
                        Name = "Category Name 9",
                        Description = "Category Description 9",
                        SortNr = 9
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId9",
                        LanguageId = "LanguageId9",
                        LanguageCode = "zh",
                        MediaType = 9,
                        SizeType = 9,
                        Title = "Media Title 9",
                        Path = "Media Path 9"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9a6bce"),
                ParentId = "ParentId10",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId10",
                        LanguageId = "LanguageId10",
                        LanguageCode = "ru",
                        Name = "Category Name 10",
                        Description = "Category Description 10",
                        SortNr = 10
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId10",
                        LanguageId = "LanguageId10",
                        LanguageCode = "ru",
                        MediaType = 10,
                        SizeType = 10,
                        Title = "Media Title 10",
                        Path = "Media Path 10"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7cad6bce"),
                ParentId = "ParentId11",
                IsActive = false,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId11",
                        LanguageId = "LanguageId11",
                        LanguageCode = "ar",
                        Name = "Category Name 11",
                        Description = "Category Description 11",
                        SortNr = 11
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId11",
                        LanguageId = "LanguageId11",
                        LanguageCode = "ar",
                        MediaType = 11,
                        SizeType = 11,
                        Title = "Media Title 11",
                        Path = "Media Path 11"
                    }
                }
            },

            new Category
            {
                Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7a9d6bce"),
                ParentId = "ParentId12",
                IsActive = true,
                CategoryLanguages = new List<CategoryLanguage>
                {
                    new CategoryLanguage
                    {
                        Id = "LanguageId12",
                        LanguageId = "LanguageId12",
                        LanguageCode = "tr",
                        Name = "Category Name 12",
                        Description = "Category Description 12",
                        SortNr = 12
                    }
                },
                CategoryMedias = new List<CategoryMedia>
                {
                    new CategoryMedia
                    {
                        Id = "MediaId12",
                        LanguageId = "LanguageId12",
                        LanguageCode = "tr",
                        MediaType = 12,
                        SizeType = 12,
                        Title = "Media Title 12",
                        Path = "Media Path 12"
                    }
                }
            }

        };
        return categories;
    }

    public static IEnumerable<CategoryDTO> ToCategoryDto(IEnumerable<Category> categories)
    {
        var categoryDTOList = new List<CategoryDTO>();
        foreach (var category in categories)
        {
            var categoryDto = new CategoryDTO(category.Id.ToString(), category.CategoryLanguages, category.IsActive, category.CategoryMedias);
            categoryDTOList.Add(categoryDto);
        }

        return categoryDTOList;
    }
}




