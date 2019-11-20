using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xpo.LastMile.TestSupportTool.Data;

namespace Xpo.LastMile.TestSupportTool.Client.Controllers
{
    [Route("IntegritySupport")]
    [ApiController]
    public class IntegritySupportController : ControllerBase
    {
        private readonly IRepository _repository;

        public IntegritySupportController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetValue/{referenceNumber}")]
        public string Compare(string referenceNumber)
        {
            _repository.testmethod(referenceNumber);
            return referenceNumber;
        }  
    }
}
