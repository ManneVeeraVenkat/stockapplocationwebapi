using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using stockapplocation.Data;
using stockapplocation.Interface;
using stockapplocation.Models;

namespace stockapplocation.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly StcokDbContext _context;
        public PortfolioRepository(StcokDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> AddPortfolio(Portfolio portfolio)
        {
            await _context.portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;

        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel != null)
            {
                _context.portfolios.Remove(portfolioModel);
                await _context.SaveChangesAsync();
                return portfolioModel;

            }
            return null;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser appUser)
        {
            var userPortfolio = await _context.portfolios.Where(x => x.AppUserId == appUser.Id)
            .Select(
                stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                }

            ).ToListAsync();

            return userPortfolio;

        }
    }
}