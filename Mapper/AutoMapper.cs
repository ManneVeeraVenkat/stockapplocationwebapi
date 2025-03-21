using AutoMapper;
using stockapplocation.Dtos;
using stockapplocation.Models;

namespace stockapplocation.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StockDto, Stock>();
            CreateMap<Stock, StockDto>();
            CreateMap<Stock, FMPStockDto>();

            CreateMap<CreateStockDTO, Stock>();
            CreateMap<CommentsDto, Comments>();
            CreateMap<Comments, CommentsDto>();
            CreateMap<CreateCommentDTO, Comments>();
            CreateMap<UpdateCommentsDto, Comments>();
        }
    }

}
