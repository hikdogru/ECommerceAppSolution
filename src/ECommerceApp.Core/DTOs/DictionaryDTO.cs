using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ECommerceApp.Core.DTOs;

public record DictionaryDTO(ObjectId Id, string Key, string Value, string LanguageId);

