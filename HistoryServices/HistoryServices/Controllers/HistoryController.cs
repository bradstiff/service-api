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
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Key is required.");
            }

            var result = await HistoryService.GetAll(key, cancellationToken);
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/History/Calculator[1]/last
        [HttpGet("last")]
        public async Task<ActionResult<HistoryViewModel>> GetLast(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Key is required.");
            }

            var result = await HistoryService.GetLast(key, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/History/Calculator[1]
        [HttpPost]
        public async Task<ActionResult<HistoryViewModel>> Add(string key, [FromBody] HistoryViewModel value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Key is required.");
            }

            if (string.IsNullOrWhiteSpace(value.Operation))
            {
                return BadRequest("Operation is required.");
            }

            return Ok(await HistoryService.Add(key, value, cancellationToken));
        }
    }
}
