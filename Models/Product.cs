using System;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using RefactorThis.Dtos;

namespace RefactorThis.Models {
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
        }
    }
}
