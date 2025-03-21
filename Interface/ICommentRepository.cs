using stockapplocation.Dtos;
using stockapplocation.Helper;
using stockapplocation.Models;

namespace stockapplocation.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comments>> GetAllComments(CommentsQueryObject queryObject);
        Task<Comments> GetCommentById(int id);
        Task<Comments> CreateComment(Comments createCommentBody);
        Task<Comments> UpdateComment(int id, UpdateCommentsDto UpdateComment);
        Task<Comments> DeleteComment(int id);
    }
}
