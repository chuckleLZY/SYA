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

// User cannot delete

namespace SyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        /* NO NEED TO CREATE & DELETE */
        
        [HttpPost("UpdateUser")]
        public async Task<ActionResult<UserResponse>> UpdateUser([FromBody] UserRequest request)
        {
            //request是否满足格式
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //当前账户id
            var u_id = Int32.Parse(User.Identity.Name);
            
            //查找当前id是否存在user
            var temp = await UserAccessor.Find(u_id);

            if(temp == 1) // exists
            {
                await UserAccessor.Change(u_id, request);
                //成功更新
                var user = await UserAccessor.Read(u_id);
                return Ok(_mapper.Map<UserResponse>(user));
            };

            // user不存在
            return BadRequest(new { message = "The user donot exist"});
        }

        [HttpPost("GetUserInfo")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
        {
            //取得存在cookie的当前账户id
            var u_id = Int32.Parse(User.Identity.Name);
            //查找当前id是否存在user
            var temp = await UserAccessor.Read(u_id);

            if(temp != null)
            {    
                var user_res = _mapper.Map<UserInfoResponse>(temp);
                if (temp.user_role == Constants.Role.Student)
                {
                    //is student
                    user_res.nof_apply = await ApplyAccessor.GetNumOfApp(u_id); //count(apply.apply_id)
                    user_res.income = await SalaryAccessor.GetSumOfSalary(u_id); //sum(salary.num)
                    var waa = await TakesAccessor.GetSumOfWorkAndAbsent(u_id);
                    user_res.nof_absent = waa.sum_absent_num; //sum(takes.absent_num)
                    user_res.work_time = waa.sum_work_time; //sum(takes.work_time)
                    user_res.absent_time = waa.sum_absent_time; //sume(takes.absent_time)
                }
                else
                {
                    //not student
                    user_res.nof_apply = 0;
                    user_res.nof_absent = 0;
                    user_res.work_time = 0;
                    user_res.income = 0;
                    user_res.absent_time = 0;
                }
           
                return Ok(user_res);
            }

            //未找到user
            return BadRequest(new {message = "cannot find the user"}); 
        }
    }
}