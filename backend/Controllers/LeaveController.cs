using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SyaApi.Plugins;
using SyaApi.DataAccessors;
using SyaApi.Entities;
using SyaApi.Requests;
using SyaApi.Responses;

namespace SyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LeaveController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public LeaveController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        [HttpPost("RequestRest")]
        [AllowAnonymous]
        public async Task<int>RequestRest([FromBody] LeaveRequest request)
        {
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp = _mapper.Map<LeaveEntity>(request);           
            temp.student_id=stu_id;

            var ans =await LeaveAccessor.Create(temp);
            
            return ans;
        } 


    }
}