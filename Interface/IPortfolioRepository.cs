using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapplocation.Models;

namespace stockapplocation.Interface
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser appUser);
        Task<Portfolio> AddPortfolio(Portfolio portfolio);

        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}