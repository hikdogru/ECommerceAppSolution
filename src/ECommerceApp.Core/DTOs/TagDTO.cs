using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ECommerceApp.Core.DTOs;

public record TagDTO(ObjectId Id, string Name, string Description, bool IsActive);
