using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapplocation.Models;

namespace stockapplocation.Interface
{
    public interface IFMPService
    {
        Task<Stock> FindStockBySymbol(string symbol);

    }
}