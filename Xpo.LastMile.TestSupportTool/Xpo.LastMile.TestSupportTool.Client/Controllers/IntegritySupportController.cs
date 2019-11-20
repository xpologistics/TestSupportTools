using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Xpo.LastMile.TestSupportTool.Client.Controllers
{
    [Route("IntegritySupport")]
    [ApiController]
    public class IntegritySupportController : ControllerBase
    {
        [HttpGet("GetValue/{referenceNumber}")]
        public string Compare(string referenceNumber)
        {
            return referenceNumber;
        }  
    }
}
