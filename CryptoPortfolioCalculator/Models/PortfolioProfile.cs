using AutoMapper;
using CryptoPortfolioCalculator.DataContracts;

namespace CryptoPortfolioCalculator.Models
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<PortfolioDto, Portfolio>();
            CreateMap<Portfolio, PortfolioDto>();

            CreateMap<PortfolioItemDto, PortfolioItem>();
            CreateMap<PortfolioItem, PortfolioItemDto>();
        }
    }
}