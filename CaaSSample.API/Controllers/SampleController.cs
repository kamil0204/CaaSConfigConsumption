using CaaSSample.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CaaSSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IDBConfig _dBConfig;

        public SampleController(IDBConfig dBConfig)
        {
            _dBConfig = dBConfig;
        }

        [HttpGet]
        public async Task<dynamic> GetString()
        {
            using (var connection = new SqlConnection(_dBConfig.ConnectionString))
            {
                return await connection.QueryAsync<dynamic>("SELECT Top 10  [cltrl_guid] as CollateralGuid,[proj_guid] as ProjectGuid FROM [IFS].[ucm].[adhoc_cltrl]");
            }
        } 
    }
}
