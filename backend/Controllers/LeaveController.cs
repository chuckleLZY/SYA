using System;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
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
        /// 计算请假时长
        /// dumei 09.12
        ///</summery>
        static private double CalDurationTime(string start_time, string end_time)
        {
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "HH:mm";
            DateTime stTime = Convert.ToDateTime(start_time, dtFormat);
            DateTime edTime = Convert.ToDateTime(end_time, dtFormat);

            double interval_time = edTime.Subtract(stTime).TotalHours;
            return interval_time;
        }

        ///<summery>
        ///学生发布请假请求
        /// chuckle 8.22
        ///</summery>
        ///<summery>
        /// 更改请假时间，计算请假时长
        /// dumei 09.12
        ///</summery>
        [HttpPost("RequestRest")]
        [AllowAnonymous]
        public async Task<ActionResult<LeaveResponse>> RequestRest([FromBody] LeaveRequest request)
        {
            var stu_id = Int32.Parse(User.Identity.Name);
            var temp = _mapper.Map<LeaveEntity>(request);

            /* 检查请假时间是否在工作时间内 */
            WorkTimeEntity work_time = await WorkAccessor.GetWorkTime(temp.work_id);
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();

            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            DateTime stDay = Convert.ToDateTime(work_time.start_day, dtFormat);
            DateTime edDay = Convert.ToDateTime(work_time.end_day, dtFormat);
            DateTime lvDay = Convert.ToDateTime(temp.leave_day);
            if (lvDay < stDay || lvDay > edDay || Convert.ToInt32(lvDay.DayOfWeek) != work_time.week_day)
            {
                return Ok(-10); //"The date of leave is not included in the work date."
            }
            
            dtFormat.ShortDatePattern = "HH:mm";
            DateTime stTime = Convert.ToDateTime(work_time.start_time, dtFormat);
            DateTime edTime = Convert.ToDateTime(work_time.end_time, dtFormat);
            DateTime lvStTime = Convert.ToDateTime(temp.leave_start, dtFormat);
            DateTime lvEdTime = Convert.ToDateTime(temp.leave_end, dtFormat);
            if (lvStTime < stTime || lvEdTime > edTime)
            {
                return Ok(-11); //"The time of leave is not included in the work time."
            }

            temp.leave_duration = CalDurationTime(temp.leave_start, temp.leave_end);
            temp.student_id = stu_id;

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
        public async Task<ActionResult<LeaveItemResponse>> ProViewLeaves([FromBody] ViewLeavesRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Constants.Role.Student)
            {
                return BadRequest(new { message = "ProViewLeaves is not for students."});
            }

            var start = (request.pagenum-1) * request.pagesize;
            var end = request.pagenum * request.pagesize;
            LeaveItemResponse leaves = new LeaveItemResponse();
            leaves.pagenum = request.pagenum;
            leaves.leavelist = new System.Collections.Generic.List<LeaveResponse>();

            var leave_list = await LeaveAccessor.ProViewLeaves(pro_id);

            if(leave_list != null)
            {
                for(int i=start; i < end && i<leave_list.total; i++)
                {
                    LeaveResponse lr = _mapper.Map<LeaveResponse>(leave_list.leaveItem[i]);
                    lr.student_name = await UserAccessor.GetUserName(lr.student_id);
                    lr.work_name = await WorkAccessor.GetWorkName(lr.work_id);        
                    leaves.leavelist.Add(lr);
                }
                leaves.total = leave_list.total;
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


        [HttpPost("ViewLeave")]
        [AllowAnonymous]
        public async Task<ActionResult<LeaveResponse>> ViewLeave([FromBody] ViewLeavesRequest request)
        {
            ViewLeaveResponse LeaveItem=new ViewLeaveResponse();
            LeaveItem.total=0;
            LeaveItem.pagenum=request.pagenum;
            LeaveItem.leavelist=new List<LeaveInfoResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var user_id =Int32.Parse(User.Identity.Name);

            var temp=await LeaveAccessor.ViewLeave(user_id);

             if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    LeaveItem.total++;
                    if(i>=start&&i<=end)
                    {
                        var list=await LeaveAccessor.Find(temp.leaveItem[i].leave_id);
                   
                        LeaveInfoResponse a=_mapper.Map<LeaveInfoResponse>(list);
                        
                        LeaveItem.leavelist.Add(a);
                    }
                }

                return Ok(LeaveItem);
            }
            return Ok(-1);
        }


    }
}