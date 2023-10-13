using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product
{
    public class Tag : DocumentLongTrack
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        [BsonElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the tag.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is Tag Active
        /// </summary>
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
    }
}