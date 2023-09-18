using System.Text.RegularExpressions;
using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Models;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using MongoDB.Bson;
using ECommerceApp.Infrastructure.Mappings;
using ECommerceApp.Infrastructure.Mappings.Product;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace ECommerceApp.Application.Services.Concrete;

public class CategoryService : CRUDService<IRepository<Category, ObjectId>, Category, ObjectId>, ICategoryService
{
    public CategoryService(IRepository<Category, ObjectId> repository) : base(repository)
    {

    }

    public async Task<PaginatedList<CategoryDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll();

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {

                if (field.Field == "name")
                {
                    // Todo: Refactor this - Remove AsEnumerable and AsQueryable
                    allData = allData.AsEnumerable()
                        .Where(x => x.CategoryLanguages.Any(cl => cl.Name.ContainsCaseInsensitive(field.Value)))
                        .AsQueryable();
                }

                else if (field.Field == "isActive")
                    allData = allData.Where(x => x.IsActive == bool.Parse(field.Value));
            }
        }

        var categoriesAsJson = allData.Where(x => !x.IsDeleted)
                                       .ToJson();
        var deserializedCategories = BsonSerializer.Deserialize<List<CategoryDTO>>(categoriesAsJson);
        var pagedData = await deserializedCategories.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
        return pagedData;
    }

    public async Task<CategoryDetailDTO> GetCategoryById(ObjectId id)
    {
        var category = await base.GetById(id);
        var categoryJson = category.ToJson();
        var deserializedCategory = BsonSerializer.Deserialize<CategoryDetailDTO>(categoryJson);
        return deserializedCategory;
    }


    public List<CategoryTreeArrowModel> GetCategoryTree(string languageCode = "English", string seperator = " > ")
    {
        var allCategories = GetAll().Where(x => !x.IsDeleted).ToList();
        Func<Category, List<Category>, string>? parents = null;
        parents = (c, l) =>
        {
            string? cs = null;
            var p = l.FirstOrDefault(m => !string.IsNullOrEmpty(c.ParentId) && m.Id == ObjectId.Parse(c.ParentId));
            if (p != null)
            {
                cs = p.CategoryLanguages.FirstOrDefault(m => m.LanguageCode == languageCode)?.Name;
                if (!string.IsNullOrEmpty(p.ParentId))
                {
                    cs = parents(p, l) + " > " + cs;
                }
            }
            return cs;
        };
        var lastdepthCategories = allCategories.ToList();
        var categoryTreeArrows = new List<CategoryTreeArrowModel>();
        foreach (var f in lastdepthCategories)
        {
            string upname = parents(f, allCategories);
            var c = new CategoryTreeArrowModel { Id = f.Id, Text = (string.IsNullOrEmpty(upname) ? "" : upname + " > ") + f.CategoryLanguages.FirstOrDefault(m => m.LanguageCode == languageCode)?.Name };
            categoryTreeArrows.Add(c);
        }
        return categoryTreeArrows;
    }

    public override async Task Delete(Category entity, bool isBulk = false)
    {
        await base.Delete(entity);
        var subCategories = GetAll().ToList()
                                    .Where(c => !string.IsNullOrEmpty(c.ParentId) && ObjectId.Parse(c.ParentId) == entity.Id);
        if (subCategories.Any())
        {
            foreach (var subCategory in subCategories)
            {
                await base.Delete(subCategory);
            }
        }
    }

    public List<CategoryHierarchicalModel> GetHierarchicals(ObjectId? parent = null)
    {
        var allCategories = GetAll().Where(x => !x.IsDeleted).ToList();
        var hierarchies = new List<CategoryHierarchicalModel>();
        Action<CategoryHierarchicalModel> getSubs = null;
        getSubs = (h) =>
        {
            var subcats = allCategories.Where(m => m.ParentId == h.Category.Id.ToString()).ToList();
            for (var i = 0; i < subcats.Count; i++)
            {
                var subcat = new CategoryHierarchicalModel(subcats[i]);
                h.SubCategories.Add(subcat);
                getSubs(subcat);
            }
        };
        foreach (var c in allCategories.Where(m => (parent == null && (m.ParentId == parent.ToString() || string.IsNullOrEmpty(m.ParentId))) || m.ParentId == parent.ToString()).ToList())
        {
            var hierarchy = new CategoryHierarchicalModel(c);
            getSubs(hierarchy);
            hierarchies.Add(hierarchy);
        }
        return hierarchies;
    }
}

