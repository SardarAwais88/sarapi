using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sarapi.Data;
using sarapi.IRepository;
using sarapi.Models;
using sarapi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi.Controllers
{
 
    [ApiVersion("2.0")]//,Deprecated =true
    [Route("api/{v:apiversion}/account")]
    [ApiController]
    public class AccountV2Controller : ControllerBase
    {
        private DatabaseContext _context;

        public AccountV2Controller(DatabaseContext context)
        {
            _context = context;

        }


        // now first thing is to add our registration endpoint
        // ake this a post
        
     
        [HttpGet]
      //  [MapToApiVersion("2.0")]
       
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetUsers()
        {


            //     var users = await _unitOfWork.users.GetAll();
  
            return Ok(_context.Users);




        }
       
    }
}