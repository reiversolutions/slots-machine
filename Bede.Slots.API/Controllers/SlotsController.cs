using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bede.Slots.Core.Builders;
using Bede.Slots.Core.Models;
using Bede.Slots.Core.Services;
using Bede.Slots.Core.Validators;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bede.Slots.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private ISlotsService Service { get; }

        public SlotsController()
        {
            var validator = new SpinSlotsValidator();
            var builder = new SpinSlotsBuilder(new RowBuilder(), new OutcomeBuilder(), new SpinSlotsResponseBuilder());
            Service = new SlotsService(validator, builder);
        }

        // POST api/
        [EnableCors("SlotsApi")]
        [HttpPost]
        public IActionResult Spin([FromBody] SpinSlotsRequest request)
        {
            var result = Service.Spin(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
