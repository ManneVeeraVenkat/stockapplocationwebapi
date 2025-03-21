using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockapplocation.Data;
using stockapplocation.Dtos;
using stockapplocation.Helper;
using stockapplocation.Interface;
using stockapplocation.Models;

namespace stockapplocation.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StcokDbContext _context;

        public StockRepository(StcokDbContext context)
        {
            _context = context;

        }


        public async Task<Stock> CreateStock(Stock stockModel)
        {
            await _context.stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> DeleteStock(int id)
        {
            var stock = await _context.stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
            {
                return null;
            }
            _context.stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;

        }


        public async Task<List<Stock>> GetAll(QueryObject query)
        {
            var stocks = _context.stocks
                .Include(s => s.Comments) // Include comments
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                string companyNameLower = query.CompanyName.ToLower();
                stocks = stocks.Where(s => s.CompanyName.ToLower().Contains(companyNameLower));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                string symbolLower = query.Symbol.ToLower();
                stocks = stocks.Where(s => s.Symbol.ToLower().Contains(symbolLower));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            // Apply pagination
            int offset = query.offset >= 0 ? query.offset : 0;
            int limit = query.limit > 0 ? query.limit : 10; // Default limit of 10

            stocks = stocks.Skip(offset).Take(limit);

            return await stocks.ToListAsync();
        }

        public async Task<Stock> GetById(int id)
        {
            var Stock = await _context.stocks.Include(_ => _.Comments).FirstOrDefaultAsync(x => x.Id == id);
            return Stock;
        }

        public async Task<Stock> GetBySymbol(string symbol)
        {
            var Stock = await _context.stocks.Include(_ => _.Comments).FirstOrDefaultAsync(x => x.Symbol == symbol);
            return Stock;
        }

        public async Task<Stock?> UpdateStock(int id, CreateStockDTO UpdateStock)
        {
            var StockToUpdate = await _context.stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (StockToUpdate == null)
            {
                return null;
            }
            StockToUpdate.Symbol = UpdateStock.Symbol;
            StockToUpdate.CompanyName = UpdateStock.CompanyName;
            StockToUpdate.MarketCap = UpdateStock.MarketCap;
            StockToUpdate.Purchase = UpdateStock.Purchase;
            StockToUpdate.LastDiv = UpdateStock.LastDiv;
            StockToUpdate.Industry = UpdateStock.Industry;

            await _context.SaveChangesAsync();

            return StockToUpdate;

        }
    }
}
