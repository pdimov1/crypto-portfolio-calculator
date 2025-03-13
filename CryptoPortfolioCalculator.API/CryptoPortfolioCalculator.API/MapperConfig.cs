using AutoMapper;
using CryptoPortfolioCalculator.DataContracts;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.API
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PortfolioItem, PortfolioItemDto>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}