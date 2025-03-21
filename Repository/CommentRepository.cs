using Microsoft.EntityFrameworkCore;
using stockapplocation.Data;
using stockapplocation.Dtos;
using stockapplocation.Helper;
using stockapplocation.Interface;
using stockapplocation.Models;

namespace stockapplocation.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly StcokDbContext _context;
        public CommentRepository(StcokDbContext context)
        {
            _context = context;

        }

        public async Task<Comments> CreateComment(Comments createCommentBody)
        {
            await _context.comments.AddAsync(createCommentBody);
            await _context.SaveChangesAsync();
            return createCommentBody;

        }

        public async Task<Comments> DeleteComment(int id)
        {
            var comment = await _context.comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return null;
            }

            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comments>> GetAllComments(CommentsQueryObject queryObject)
        {
            var comments = _context.comments.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
            }
            ;
            if (queryObject.IsDescending == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comments> GetCommentById(int id)
        {
            return await _context.comments.Include(a => a.AppUser).FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<Comments?> UpdateComment(int id, UpdateCommentsDto UpdateComment)
        {
            var commentToUpdate = await _context.comments.FirstOrDefaultAsync(_ => _.Id == id);
            if (commentToUpdate == null)
            {
                return null;
            }
            commentToUpdate.Title = UpdateComment.Title;
            commentToUpdate.Content = UpdateComment.Content;
            await _context.SaveChangesAsync();
            return commentToUpdate;
        }
    }
}
