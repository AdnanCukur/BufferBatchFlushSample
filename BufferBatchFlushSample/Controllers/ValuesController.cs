using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BufferBatchFlushSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IBufferBatchFlushService bufferBatchFlushService;
        public ValuesController(IBufferBatchFlushService bbfs)
        {
            this.bufferBatchFlushService = bbfs;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            this.bufferBatchFlushService.Add("Hola message");
            return "value";
        }

    }
}
