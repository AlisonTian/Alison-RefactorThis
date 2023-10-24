using Microsoft.VisualBasic;
using RefactorThis.Dtos;
using RefactorThis.Models;

namespace RefactorThis.Extensions
{
    public static class DtoExtension
    {
        public static Product ProductToModel(this ProductDto productDto)
        {
            Product product = new Product
            {
                Price = productDto.Price,
                DeliveryPrice = productDto.DeliveryPrice,
                Name = productDto.Name,
                Description = productDto.Description
            };
            return product;
        }

        public static Product AssignDtoToProduct(this Product product, ProductDto productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.DeliveryPrice = productDto.DeliveryPrice;
            return product;
        }

        public static ProductOption AssignDtoToProductOption(this ProductOption productOption, ProductOptionDto productOptionDto)
        {
            productOption.Name = productOptionDto.Name;
            productOption.Description = productOptionDto.Description;
            productOption.ProductId = productOptionDto.ProductId;
            return productOption;
        }
    }
}
