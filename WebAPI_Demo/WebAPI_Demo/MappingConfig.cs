using AutoMapper;
using WebAPI_Demo.Models;
using WebAPI_Demo.Models.DTO;

namespace WebAPI_Demo
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}