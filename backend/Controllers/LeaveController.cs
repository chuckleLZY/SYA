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

        ///<summery>
        ///学生发布请假请求
        /// chuckle 8.22
        ///</summery>
        [HttpPost("RequestRest")]
        [AllowAnonymous]
        public async Task<ActionResult<LeaveResponse>>RequestRest([FromBody] LeaveRequest request)
        {
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp = _mapper.Map<LeaveEntity>(request);           
            temp.student_id=stu_id;

            var ans =await LeaveAccessor.Create(temp);

            if(ans>0)
            {
                var leavere=await LeaveAccessor.Find(ans);
                if(leavere!=null)
                    return Ok(_mapper.Map<LeaveResponse>(leavere));
                return Ok(-1);
            }            
            return Ok(-1);
        } 

        ///<summery>
        /// (非学生用户)查看请假申请
        /// dumei 08.24
        [HttpPost("ProViewLeaves")]
        //[AllowAnonymous]
        public async Task<ActionResult<LeaveItemResponse>> ProViewLeaves()
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Constants.Role.Student)
            {
                return BadRequest(new { message = "ProViewLeaves is not for students."});
            }

            LeaveItemResponse leaves = new LeaveItemResponse();
            leaves.total = 0;
            leaves.leavelist = new System.Collections.Generic.List<LeaveResponse>();

            var leave_list = await LeaveAccessor.ProViewLeaves(pro_id);

            if(leave_list != null)
            {
                for(int i=0; i<leave_list.total; i++)
                {
                    LeaveResponse lr = _mapper.Map<LeaveResponse>(leave_list.leaveItem[i]);
                    lr.student_name = await UserAccessor.GetUserName(lr.student_id);
                    lr.work_name = await WorkAccessor.GetWorkName(lr.work_id);        
                    leaves.leavelist.Add(lr);
                    leaves.total++;
                }
                return Ok(leaves);
            }
            return Ok(-1); // Never arrive there
        }

        ///<summery>
        /// (非学生用户)同意/拒绝请假申请
        /// 若同意，则takes.absent_num +1, takes.absent_time+leave_info.leave_duration
        /// dumei 08.24
        ///</summery>
        [HttpPost("ProManageLeave")]
        //[AllowAnonymous]
        public async Task<ActionResult<LeaveResponse>> ProManageLeave(ManageLeaveRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Constants.Role.Student)
            {
                return BadRequest(new { message = "Students cannot manage leave application."});
            }

            LeaveEntity le = await LeaveAccessor.Read(request.leave_id);
            if (le == null) 
            {
                return BadRequest(new {message = "Leave id not found."});
            }
            if (le.status != Constants.ApplyStatus.Applying)
            {
                return BadRequest(new {message = "The application have been managed."});
            }
            var success_change = await LeaveAccessor.SetLeaveStatus(request.leave_id, request.status);
            if (success_change > 0)
            {  
                le.status = request.status; //减少查询
                LeaveResponse lr = _mapper.Map<LeaveResponse>(le);
                // 未检查id是否存在
                lr.student_name = await UserAccessor.GetUserName(le.student_id);
                //lr.teacher_name = await UserAccessor.GetUserName(le.teacher_id);
                lr.work_name = await WorkAccessor.GetWorkName(le.work_id);

                if (request.status == Constants.ApplyStatus.Accepted)
                {
                    // 录用，更新takes
                    await TakesAccessor.UpdateAbsent(le.student_id, le.work_id, le.leave_duration);
                }
                
                return lr;
            }

            return BadRequest(new {message = "Update failed"});
        }


    }
}