using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace sarapi.Controllers
{
    [Route("api/[controller]")]
   
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;


        public AccountController(IUnitOfWork unitOfWork, UserManager<User> userManager,
            ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;

        }


        // now first thing is to add our registration endpoint
        // ake this a post
        [HttpPost]
        [Route("register")]
        // frombbody we refer to UserDto
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email} ");
            //    _logger.LogInformation($"Registration Attempt for {userApiDto.PhoneNumber} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       //     try
        //    {
                var user = _mapper.Map<User>(userDto);
                user.UserName = userDto.Email;
                //    user.PhoneNumber = userApiDto.PhoneNumber;
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDto.Roles);
                return Accepted();
            }
       //     catch (Exception ex)
      //      {
            //    _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
         //       return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
           // }
    //    }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            _logger.LogInformation($"Login Attempt for {userDto.Email} ");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          /*  try
            {*/
                if (!await _authManager.ValidateUser(userDto))
                {
                    return Unauthorized();
                } 

                return Accepted(new { Token = await _authManager.CreateToken() });
            }
         /*   catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            } 
        }*/

        [HttpGet]
       // [Authorize]
       // we implement cache to improve perormance of our api
       // we set this for 60 seconds
       // [ResponseCache(CacheProfileName = "120SecondsDuration")]
       [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

         public async Task<IActionResult> GetUsers([FromQuery] RequestParams requestParams)
         {
            
           
            //     var users = await _unitOfWork.users.GetAll();
                 var users = await _unitOfWork.users.GetPagedList(requestParams);
                 var results = _mapper.Map<IList<UserDto>>(users);
                 // we return ok when every thing is correct
                 return Ok(results);
    
            


         } 
        /* public async Task<IActionResult> GetUsers([FromQuery] RequestParams requestParams)
         {
             // now use try catch exception to handle exception
             try
             {
            //     var users = await _unitOfWork.users.GetAll();
                 var users = await _unitOfWork.users.GetPagedList(requestParams);
                 var results = _mapper.Map<IList<UserDto>>(users);
                 // we return ok when every thing is correct
                 return Ok(results);
                 // send back data to the calling client whatever is needed 
             }
             // in catch block we handle exception if any error occour
             catch (Exception ex)
             {
                 // at this point logger becomes very important

                 _logger.LogError(ex, $"something went wrong {nameof(GetUsers)}");
                 return StatusCode(500, "Interval server error. please try again later ");
             }


         } */
        [HttpGet("{id:int}")]     // ("{id:string}")
     /*  [ Authorize] */
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       

        public async Task<IActionResult> GetUser(string id)
        {
            // now use try catch exception to handle exception
      //     try
        //    {
                var user = await _unitOfWork.users.Get(q => q.Id ==id.ToString()); //, new List<string> { "Users" });
                var results = _mapper.Map<IList<User>>(user);
              
                // we return ok when every thing is correct
                return Ok(results);
                // send back data to the calling client whatever is needed 
            }
            // in catch block we handle exception if any error occour
      /*      catch (Exception ex)
          {
                // at this point logger becomes very important

                _logger.LogError(ex, $"something went wrong {nameof(GetUser)}");
                return StatusCode(500, "Interval server error. please try again later ");
            }

        
        }*/
  
   

    } 
    
}
