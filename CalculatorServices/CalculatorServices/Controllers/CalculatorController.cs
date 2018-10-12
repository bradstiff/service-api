using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Services;
using CalculatorServices.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly IAdditionService _additionService;
        private readonly ISubtractionService _subtractionService;
        private readonly IHistoryClient _historyService;

        public CalculatorController(IAdditionService additionService,
            ISubtractionService subtractionService,
            IHistoryClient historyService)
        {
            _additionService = additionService;
            _subtractionService = subtractionService;
            _historyService = historyService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<CalculatorResultViewModel>> Add(Guid? id, decimal? value, CancellationToken cancellationToken)
        {
            if (!value.HasValue)
            {
                return BadRequest("Must provide value");
            }

            return Ok(await _additionService.Add(id, value.Value, cancellationToken));
        }

        [HttpPost("subtract")]
        public async Task<ActionResult<CalculatorResultViewModel>> Subtract(Guid? id, decimal? value, CancellationToken cancellationToken)
        {
            if (!value.HasValue)
            {
                return BadRequest("Must provide value");
            }

            return Ok(await _subtractionService.Subtract(id, value.Value, cancellationToken));
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult<CalculatorHistoryViewModel>> History(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _historyService.Get(id, cancellationToken));
        }
    }
}
