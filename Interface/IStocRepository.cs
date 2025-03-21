using stockapplocation.Dtos;
using stockapplocation.Helper;
using stockapplocation.Models;

namespace stockapplocation.Interface
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAll(QueryObject query);
        Task<Stock> GetById(int id);
        Task<Stock> GetBySymbol(string symbol);
        Task<Stock> CreateStock(Stock stockModel);
        Task<Stock> UpdateStock(int id,CreateStockDTO stockDto);
        Task<Stock> DeleteStock(int id);

    }
}
