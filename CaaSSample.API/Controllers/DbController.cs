using CaaSSample.API.Services;
using CaaSSample.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CaaSSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly DBOptions _dbOptions;

        public DbController(IOptionsSnapshot<DBOptions> dbOptionsSnapshot)
        {
            _dbOptions = dbOptionsSnapshot.Value;
        }

        [HttpGet]
        public async Task<dynamic> GetString()
        {
            using (var connection = new SqlConnection(_dbOptions.IFSDB))
            {
                return await connection.QueryAsync<dynamic>("SELECT Top 10  [cltrl_guid] as CollateralGuid,[proj_guid] as ProjectGuid FROM [IFS].[ucm].[adhoc_cltrl]");
            }
        }
    }
}
