using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using HistoryServices.ViewModels;
using HistoryServices.Services;
using System.Threading;

namespace HistoryServices.Controllers
{
    [Route("api/[controller]/{key}")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private IHistoryService HistoryService { get; }

        public HistoryController(IHistoryService HistoryService)
        {
            this.HistoryService = HistoryService;
        }

        // GET: api/History/Calculator[1]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoryViewModel>>> GetAll(string key, CancellationToken cancellationToken)
        {
            return Ok(await HistoryService.GetAll(key, cancellationToken));
        }

        // GET: api/History/Calculator[1]/last
        [HttpGet("last")]
        public async Task<ActionResult<HistoryViewModel>> GetLast(string key, CancellationToken cancellationToken)
        {
            return Ok(await HistoryService.GetLast(key, cancellationToken));
        }

        // POST: api/History/Calculator[1]
        [HttpPost]
        public async Task<ActionResult<HistoryViewModel>> Add(string key, [FromBody] HistoryViewModel value, CancellationToken cancellationToken)
        {
            return Ok(await HistoryService.Add(key, value, cancellationToken));
        }
    }
}
