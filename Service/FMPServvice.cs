using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using AutoMapper;
using Newtonsoft.Json;
using stockapplocation.Dtos;
using stockapplocation.Interface;
using stockapplocation.Mapper;
using stockapplocation.Models;

namespace stockapplocation.Service
{
    public class FMPServvice : IFMPService
    {
        private HttpClient _httpClient;
        private IConfiguration _config;
        private readonly IMapper _mapper;

        public FMPServvice(HttpClient httpClient, IConfiguration config, IMapper mapper)
        {
            _httpClient = httpClient;
            _config = config;
            _mapper = mapper;
        }
        public async Task<Stock?> FindStockBySymbol(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content, settings);

                   
                    var stock = tasks[0];
                    if (stock != null)
                    {

                        return stock.ToStockFromFMP();
                    }
                    return null;
                }
                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}