using AutoMapper;
using CryptoPortfolioCalculator.DataContracts;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.API.Models
{
    public class PortfolioItemProfile : Profile
    {
        public PortfolioItemProfile()
        {
            CreateMap<PortfolioItemDto, PortfolioItem>();
            CreateMap<PortfolioItem, PortfolioItemDto>();
        }
    }
}
