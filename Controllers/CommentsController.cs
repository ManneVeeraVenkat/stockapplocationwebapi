using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stockapplocation.Data;
using stockapplocation.Dtos;
using stockapplocation.Extensions;
using stockapplocation.Helper;
using stockapplocation.Interface;
using stockapplocation.Mapper;
using stockapplocation.Models;
using stockapplocation.Repository;
using System.Xml.Linq;

namespace stockapplocation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _CommentRepository;
        private readonly IStockRepository _StockRepository;

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IFMPService _fMPService;
        public CommentsController(ICommentRepository CommentRepository, IStockRepository StockRepository, UserManager<AppUser> userManager, IMapper mapper, IFMPService fMPService)
        {
            _CommentRepository = CommentRepository;
            _StockRepository = StockRepository;
            _mapper = mapper;
            _userManager = userManager;
            _fMPService = fMPService;
        }
        [HttpGet]
        [Authorize]

        public async Task<ActionResult> GetAllComments([FromQuery] CommentsQueryObject queryObject)
        {
            var comments = await _CommentRepository.GetAllComments(queryObject);

            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);
        }
        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<CommentsDto>> GetCommentById([FromRoute] int id)
        {
            // Fetch the comment from the repository by id
            var comment = await _CommentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            // Map the single Comment to CommentsDto
            var commentDto = _mapper.Map<CommentsDto>(comment);

            return Ok(commentDto);
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<ActionResult> DeleteCommentById([FromRoute] int id)
        {
            var comment = await _CommentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok("Deleted");
        }
        [HttpPost("{symbol}")]
        [Authorize]

        public async Task<ActionResult> CreateComment([FromRoute] string symbol, [FromBody] CreateCommentDTO createComment)
        {
            var stock = await _StockRepository.GetBySymbol(symbol);

            if (stock == null)
            {
                stock = await _fMPService.FindStockBySymbol(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists");
                }
                else
                {
                    await _StockRepository.CreateStock(stock);
                }
            }
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);

            var commentToCreated = createComment.ToCommentFromCreate(stock.Id);
            commentToCreated.AppUserId = appUser.Id;
            commentToCreated.CreatedBy = appUser.UserName;
            await _CommentRepository.CreateComment(commentToCreated);
            var commentsDto = _mapper.Map<CommentsDto>(commentToCreated);

            return CreatedAtAction(nameof(GetCommentById), new { id = commentToCreated.Id }, commentsDto);

        }
        [HttpPut("{id}")]
        [Authorize]

        public async Task<ActionResult<CommentsDto>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentsDto updateComment)
        {

            var updatedComment = await _CommentRepository.UpdateComment(id, updateComment);
            if (updateComment == null)
            {
                return NotFound();
            }
            var commentsDto = _mapper.Map<CommentsDto>(updatedComment);

            return Ok(commentsDto);
        }
    }
}
