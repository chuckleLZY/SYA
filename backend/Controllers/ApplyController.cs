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
using SyaApi.Constants;

namespace SyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ApplyController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public ApplyController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        ///<summery>
        /// (非学生用户)查看申请
        /// 检查user.role
        /// dumei 08.24
        ///</summery>
        [HttpGet("ProViewApps")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApplyItemResponse>> ProViewApps()
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "ProViewApps is not for students."});
            }

            var apps = new ApplyItemResponse();
            apps.total = 0;
            apps.applist = new System.Collections.Generic.List<ApplyResponse>();

            var provide_list = await ApplyAccessor.ProViewApps(pro_id);

            if(provide_list != null)
            {
                for(int i=0; i<provide_list.total; i++)
                {
                    ApplyResponse ar = _mapper.Map<ApplyResponse>(provide_list.ApplyItem[i]);
                    // 未检查id是否存在
                    ar.student_name = await UserAccessor.GetUserName(provide_list.ApplyItem[i].student_id);
                    ar.teacher_name = await UserAccessor.GetUserName(provide_list.ApplyItem[i].teacher_id);
                    ar.work_name = await WorkAccessor.GetWorkName(provide_list.ApplyItem[i].work_id);

                    apps.applist.Add(ar);
                    apps.total++;
                }
                return Ok(apps);
            }
            return Ok(-1); // Never arrive there
        }

        ///<summery>
        /// (非学生用户)处理申请, 录用/拒录
        /// 检查user.role
        /// dumei 08.24
        ///</summery>
        [HttpPost("ProManageApp")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApplyResponse>> ProManageApp(ManageApplyRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "ProViewApps is not for students."});
            }

            var success_change = await ApplyAccessor.SetApplyStatus(request.apply_id, request.status);
            if (success_change > 0)
            {
                ApplyEntity ae = await ApplyAccessor.Read(request.apply_id);
                ApplyResponse ar = _mapper.Map<ApplyResponse>(ae);
                // 未检查id是否存在
                ar.student_name = await UserAccessor.GetUserName(ae.student_id);
                ar.teacher_name = await UserAccessor.GetUserName(ae.teacher_id);
                ar.work_name = await WorkAccessor.GetWorkName(ae.work_id);

                return ar;
            }
            return BadRequest(new {message = "Apply id not found"});
        }

    }

}