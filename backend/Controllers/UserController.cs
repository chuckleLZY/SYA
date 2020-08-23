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

        [HttpGet("GetUserInfo")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
        {
            //取得存在cookie的当前账户id
            var u_id = Int32.Parse(User.Identity.Name);
            //查找当前id是否存在user
            var temp = await UserAccessor.Read(u_id);

            if(temp != null)
            {    
            var user_res = new UserInfoResponse {
                user_id = temp.user_id,
                user_role = temp.user_role,
                user_name = temp.user_name,
                gender = temp.gender,
                avatar = temp.avatar,
                email = temp.email,
                tel = temp.tel,
                bank = temp.bank,
                
                /* /// 申请总次数
                nof_apply = count(apply.work_id)

                /// 请假总次数
                nof_absent = takes.absent_num

                /// 工作总时长
                work_time = sum(takes.work_time)

                /// 已获得薪水总量
                income = sum(salary.salary_id)
                */
                nof_apply = 0,
                nof_absent = 0,
                work_time = 0,
                income = 0,
            };
                return Ok(user_res);
            }

            //未找到user
            return BadRequest(new {message = "cannot find the user"}); 
        }
    }
}