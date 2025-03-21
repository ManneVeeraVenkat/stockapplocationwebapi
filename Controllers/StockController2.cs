using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockapplocation.Data;
using stockapplocation.Dtos;
using stockapplocation.Helper;
using stockapplocation.Interface;
using stockapplocation.Models;

namespace stockapplocation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController2 : ControllerBase
    {
        private readonly IStockRepository _StockRepository;
        private readonly IMapper _mapper;
        public StockController2(IStockRepository StockRepository, IMapper mapper)
        {
            _StockRepository = StockRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _StockRepository.GetAll(query);
            var updated = _mapper.Map<List<StockDto>>(stocks);
            return Ok(updated);
        }
        [HttpGet("stock/{id}")]
        [Authorize]

        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            var stock = await _StockRepository.GetById(id);
            var updated = _mapper.Map<StockDto>(stock);
            return Ok(updated);

        }

        [HttpGet("stock/symbol/{symbol}")]
        public async Task<IActionResult> GetStockBySymbol([FromRoute] string symbol)
        {
            var stock = await _StockRepository.GetBySymbol(symbol);
            var updated = _mapper.Map<StockDto>(stock);
            return Ok(updated);

        }
        [HttpPost]
        [Authorize]

        public async Task<ActionResult> CreateNewStock([FromBody] CreateStockDTO stock)
        {
            var newstock = _mapper.Map<Stock>(stock);
            await _StockRepository.CreateStock(newstock);

            return CreatedAtAction(nameof(GetStockById), new { id = newstock.Id }, newstock);
        }
        [HttpDelete("stock/{id}")]
        [Authorize]

        public async Task<ActionResult<Stock>> DeleteStock(int id)
        {
            var stock = await _StockRepository.DeleteStock(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok("deleted");
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]

        public async Task<ActionResult> UpdateStock([FromRoute] int id, [FromBody] CreateStockDTO updateBody)
        {
            var UpdatedStock = await _StockRepository.UpdateStock(id, updateBody);
            if (UpdatedStock == null)
            {
                return NotFound();
            }
            return Ok(UpdatedStock);

        }

    }
}
