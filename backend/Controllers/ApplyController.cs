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
        [HttpPost("ProViewApps")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApplyItemResponse>> ProViewApps([FromBody]ViewAppRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "ProViewApps is not for students."});
            }

            var apps = new ApplyItemResponse();
            //apps.totalpage=0;
            apps.pagenum=request.pagenum;
            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize;
            if (start < 0)
            {
                return BadRequest(new { message = "Page num error"});
            }

            apps.applist = new System.Collections.Generic.List<ApplyResponse>();
            var provide_list = await ApplyAccessor.ProViewApps(pro_id);

            if(provide_list != null)
            {
                for(int i=start; i<end && i<provide_list.total; i++)
                {
                    ApplyResponse ar = _mapper.Map<ApplyResponse>(provide_list.ApplyItem[i]);
                    // 未检查id是否存在
                    ar.student_name = await UserAccessor.GetUserName(provide_list.ApplyItem[i].student_id);
                    ar.teacher_name = await UserAccessor.GetUserName(provide_list.ApplyItem[i].teacher_id);
                    ar.work_name = await WorkAccessor.GetWorkName(provide_list.ApplyItem[i].work_id);

                    apps.applist.Add(ar);
                }
                apps.totalpage = provide_list.total;
                return Ok(apps);
            }
            return Ok(-1); // Never arrive there
        }

        ///<summery>
        /// (非学生用户)处理申请, 录用/拒录
        /// 若录用，则增添takes
        /// dumei 08.24
        ///</summery>
        [HttpPost("ProManageApp")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApplyResponse>> ProManageApp(ManageApplyRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "Students cannot manage application."});
            }

            ApplyEntity ae = await ApplyAccessor.Read(request.apply_id);
            if (ae == null) 
            {
                return BadRequest(new {message = "Apply id not found."});
            }
            if (ae.status != Constants.ApplyStatus.Applying)
            {
                return BadRequest(new {message = "Application have been managed."});
            }
            var success_change = await ApplyAccessor.SetApplyStatus(request.apply_id, request.status);
            if (success_change > 0)
            {  
                ae.status = request.status; //减少查询
                ApplyResponse ar = _mapper.Map<ApplyResponse>(ae);
                // 未检查id是否存在
                ar.student_name = await UserAccessor.GetUserName(ae.student_id);
                ar.teacher_name = await UserAccessor.GetUserName(ae.teacher_id);
                ar.work_name = await WorkAccessor.GetWorkName(ae.work_id);

                if (request.status == Constants.ApplyStatus.Accepted)
                {
                    // 录用，创建takes
                    var take = new TakesEntity
                    {
                        work_name = ar.work_name,
                        student_id = ae.student_id,
                        work_id = ae.work_id,
                        work_time = await WorkAccessor.GetWorkTotalTime(ae.work_id)
                    };
                    await TakesAccessor.Create(take);
                }
                
                return ar;
            }

            return BadRequest(new {message = "Update failed"});
        }

        ///<summery>
        /// 学生用户提交工作申请
        /// chuckle 08.26
        ///</summery>
        [HttpPost("CreateApply")]
        [AllowAnonymous]
         public async Task<ActionResult<int>> CreateApply(ApplyRequest request)
         {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) != Role.Student)
            {
                return BadRequest(new { message = "CreateApply is for students."});
            }
            var temp=_mapper.Map<ApplyEntity>(request);
            temp.student_id=pro_id;
            temp.teacher_id= await WorkAccessor.GetTeacher(temp.work_id);
            temp.resume_id= await ResumeAccessor.GetResume(temp.student_id);

            var ans=await ApplyAccessor.Create(temp);
            if(ans>0)
            {  
               return Ok(ans); 
            }
            return Ok(-1);
         }


    }

}