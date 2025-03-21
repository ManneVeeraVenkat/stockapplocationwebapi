using stockapplocation.Dtos;
using stockapplocation.Models;

namespace stockapplocation.Mapper
{
    public static class CommentsMapper
    {
        public static CommentsDto ToCommentDto(this Comments commentModel)
        {
            return new CommentsDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.AppUser.UserName,
                StockId = commentModel.StockId
            };
        }
        public static Comments ToCommentFromCreate(this CreateCommentDTO commentDto, int stockId)
        {
            return new Comments
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId,
            };
        }
    }
}
