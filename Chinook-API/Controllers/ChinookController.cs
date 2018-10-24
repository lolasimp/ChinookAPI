using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chinook_API.DataAccess;
using Chinook_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Chinook_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private readonly SalesStorage _storage;

        public ChinookController()
        {
            _storage = new SalesStorage();
        }

        [HttpGet()]
        public IActionResult GetSalesAgentsbyId()
        {
            return Ok(_storage.GetById());
        }

        [HttpGet("total")]
        public IActionResult GetByTotalId()
        {
            return Ok(_storage.GetByTotalId());
        }

        [HttpGet("{id}/line")]
        public IActionResult GetByLineId(int id)
        {
            return Ok(_storage.GetByLineId(id));
        }
    }
}