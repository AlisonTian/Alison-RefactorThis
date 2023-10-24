using System;

namespace RefactorThis.Dtos
{
    public class ProductOptionDto: CreateProductOptionDto
    {
        public Guid ProductId { get; set; }
      
    }
}
