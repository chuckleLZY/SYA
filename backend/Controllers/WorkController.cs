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
using SyaApi.Constants;

namespace SyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WorkController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public WorkController(IMapper mapper)
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
        /// 学生用户查看个人拥有工作
        /// chuckle 8.25
        ///</summery>
        [HttpPost("ViewOwnWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewOwnWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp=await WorkAccessor.FindOwnWork(stu_id);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    
                    var work_info=await WorkAccessor.FindWorkInfo(temp.TakesItem[i].work_id);
                    var status = await TakesAccessor.FindInfo(stu_id,temp.TakesItem[i].work_id);
                    workItem.totalpage++;
                    if(i>=start&&i<=end&&work_info!=null)
                    {
                        WorkResponse a=_mapper.Map<WorkResponse>(work_info);
                        a.status=status.status;
                        workItem.worklist.Add(a);
                        
                    }
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        ///<summery>
        /// 学生搜索个人拥有工作
        /// chuckle 8.25
        ///</summery>
        [HttpPost("FindOwnWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> FindOwnWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);
            string search='%'+request.query+'%';
            var temp=await WorkAccessor.FindOwnWork(search,stu_id);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    workItem.totalpage++;
                    if(i>=start&&i<=end)
                    {
                        WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);  
                        var s=await TakesAccessor.FindInfo(stu_id,a.work_id);         
                        a.status=s.status;            
                        workItem.worklist.Add(a);
                    }
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        ///<summery>
        /// 用户查看工作详细信息
        /// chuckle 8.25
        ///</summery>
        [HttpPost("ViewWorkInfo")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkResponse>> ViewWorkInfo([FromBody]FindworkRequest temp)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var work_info=await WorkAccessor.FindWorkInfo(temp.work_id);

            if(work_info!=null)
            {
                return Ok(_mapper.Map<WorkResponse>(work_info));
            }
                return Ok(-1);
        }

        ///<summery>
        /// 用户查看所有工作
        /// chuckle 8.25
        ///</summery>
        [HttpPost("ViewAllWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewAllWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;
            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp=await WorkAccessor.FindAllWork();

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    workItem.totalpage++;
                    if(i>=start&&i<=end)
                    {
                        WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);                       
                        workItem.worklist.Add(a);
                    }
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        ///<summery>
        /// 用户搜索工作
        /// chuckle 8.25
        ///</summery>
        [HttpPost("FindWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> FindWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);
            string search='%'+request.query+'%';
            var temp=await WorkAccessor.FindWork(search);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    workItem.totalpage++;
                    if(i>=start&&i<=end)
                    {
                        WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);                       
                        workItem.worklist.Add(a);
                    }
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        ///<summery>
        /// 计算总小时数
        /// dumei 09.08
        ///</summery>
        ///<summery>
        /// 更新：取消秒
        /// dumei 09.11
        ///</summery>
        static private double CalTotalTime(string start_day, string end_day, string start_time, string end_time, int week_day)
        {
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            
            dtFormat.ShortDatePattern = "HH:mm";
            DateTime stTime = Convert.ToDateTime(start_time, dtFormat);
            DateTime edTime = Convert.ToDateTime(end_time, dtFormat);
            double interval_time = edTime.Subtract(stTime).TotalHours;
            
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            DateTime stDay = Convert.ToDateTime(start_day, dtFormat);
            DateTime edDay = Convert.ToDateTime(end_day, dtFormat);
            int interval_day = (edDay.Subtract(stDay).Days + 1)/7; //end_day也算1天

            int stWeek = Convert.ToInt32(stDay.DayOfWeek);
            int edWeek = Convert.ToInt32(edDay.DayOfWeek);
            if (stWeek <= week_day && week_day <= edWeek) interval_day += 1;
            else if (edWeek < stWeek-1 && week_day <= edWeek) interval_day += 1;

            return interval_day * interval_time;
        }

        ///<summery>
        /// (非学生用户)创建工作
        /// 检查user.role
        /// dumei 08.23
        ///</summery>
        ///<summery>
        /// 增添start_day,end_day,start_time,end_time
        /// 删除work_time
        /// 计算总小时数total_time
        /// dumei 09.08
        ///</summery>
        [HttpPost("CreateWork")]
        public async Task<ActionResult<WorkResponse>> CreateWork([FromBody] ProvideWorkRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var provider_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(provider_id) == Role.Student)
            {
                return BadRequest(new { message = "Student cannot create work"});
            }
            if (request.week_day<1 || request.week_day>7)
            {
                return BadRequest(new { message = "week day must between 1 to 7."});
            }
            var work = _mapper.Map<WorkEntity>(request);
            work.teacher_id = provider_id;
            //work.work_time = "none"; //tobe delete

            // 计算工作总小时数total_time
            work.total_time = CalTotalTime(request.start_day, request.end_day, request.start_time, request.end_time, request.week_day);
            if (work.total_time == 0)
            {
                return Ok("Duration time is 0.");
            }
            int work_id = await WorkAccessor.Create(work);
            if (work_id == 0)
            {
                return Ok("Fail to create work.");
            }
            work.work_id = work_id;
            return Ok(_mapper.Map<WorkResponse>(work));
        }

        ///<summery>
        /// (非学生用户)查看历史发布工作
        /// 检查user.role
        /// dumei 08.23
        [HttpPost("ViewHistoryWork")]
        //[AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewHistoryWork([FromBody] ViewWorkRequest request)
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "ViewHistoryWork is not for students. Students please try ViewOwnWork."});
            }

            WorkItemResponse workItem = new WorkItemResponse();
            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize;
            workItem.totalpage=start;
            workItem.pagenum=request.pagenum;
            workItem.worklist = new List<WorkResponse>();
            var provide_list = await WorkAccessor.FindHistoryWork(pro_id);

            if(provide_list != null)
            {
                for (int i=start; i<end && i<provide_list.total; ++i)
                {
                    WorkResponse wr = _mapper.Map<WorkResponse>(provide_list.workItem[i]);           
                    workItem.worklist.Add(wr);
                }
                workItem.totalpage = provide_list.total;
                return Ok(workItem);
            }
            return Ok(-1); // Never arrive there
        }

        ///<summery>
        /// 老师修改工作信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("ChangeWorkInfo")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkResponse>> ChangeWorkInfo([FromBody] UpdateWorkRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var provider_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(provider_id) == Role.Student)
            {
                return BadRequest(new { message = "Student cannot update work"});
            }
            var work = _mapper.Map<WorkEntity>(request);
            work.teacher_id = provider_id;
            work.work_id=request.work_id;
            work.total_time = CalTotalTime(request.start_day, request.end_day, request.start_time, request.end_time, request.week_day);
            if (work.total_time == 0)
            {
                return Ok("Duration time is 0.");
            }
            await WorkAccessor.Update(work); //return work_id

            return Ok(_mapper.Map<WorkResponse>(work));
        }


        ///<summery>
        /// 用户点赞 0是取消点赞，1是已点赞
        /// chuckle 9.9
        ///</summery>
        [HttpPost("GetLike")]
        [AllowAnonymous]
        public async Task<int> GetLike([FromBody] FindworkRequest request)
        {
            var provider_id = Int32.Parse(User.Identity.Name);
            //status
            var temp = await LikeAccessor.Find(provider_id,request.work_id);

            //-1是首次点赞
            if(temp==-1)
            {
                var temp_a=await LikeAccessor.Create(provider_id,request.work_id);
                if(temp_a<0)//点赞失败
                {
                    return -1;
                }
                
            }
            else
            {
                var temp_b= await LikeAccessor.Change(provider_id,request.work_id,temp);
                if(temp_b==-1)
                {
                    return -1;
                }
                else if(temp_b==0)
                {
                    await WorkAccessor.getnolike(request.work_id);
                    return 0;
                }
            }
            await WorkAccessor.getlike(request.work_id);
            return 1;
        }



        ///<summery>
        /// 用户点赞 0是取消点赞，1是已点赞
        /// chuckle 9.9
        ///</summery>
        [HttpPost("ShowLike")]
        [AllowAnonymous]
        public async Task<int> ShowLike([FromBody] FindworkRequest request)
        {
            var provider_id = Int32.Parse(User.Identity.Name);
            //status
            var temp = await LikeAccessor.Find(provider_id,request.work_id);

            //-1是首次点赞
            if(temp==-1)
            {
                return 0;
                
            }
            else
            {
                return temp;
            }

        }






        ///<summery>
        /// 学生辞职，并向老师发出信息提示
        /// chuckle 9.9
        ///</summery>
        [HttpPost("Getresign")]
        [AllowAnonymous]
        public async Task<int> Getresign([FromBody] FindworkRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -1;
            }
            var user_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(user_id) != Role.Student)
            {
                return -2;
            }
            TakesEntity entity =new TakesEntity();
            entity.work_id=request.work_id;
            entity.student_id=user_id;
            MessageEntity temp_entity= new MessageEntity();
            var work_name =await WorkAccessor.GetWorkName(request.work_id);
            var student_name= await UserAccessor.GetUserName(user_id);
            temp_entity.message_type=0;
            temp_entity.content=student_name+"同学辞去"+work_name+"工作。";
            temp_entity.sender_id=user_id;
            temp_entity.receiver_id=await WorkAccessor.GetTeacher(request.work_id);

            var num=await TakesAccessor.GetRegion(entity);
            await MessageAccessor.Create(temp_entity,1);
            return num;
        }

        ///<summery>
        /// 学生删除已辞职工作
        /// chuckle 9.9
        ///</summery>
        [HttpPost("Deleteresign")]
        [AllowAnonymous]
        public async Task<int> Deleteresign([FromBody] FindworkRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -1;
            }
            var user_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(user_id) != Role.Student)
            {
                return -2;
            }
            TakesEntity entity =new TakesEntity();
            entity.work_id=request.work_id;
            entity.student_id=user_id;
            var num=await TakesAccessor.Delete(entity);
            return num;
        }


        [HttpPost("DeleteWork")]
        [AllowAnonymous]
        public async Task<int> DeleteWork([FromBody]FindworkRequest request)
        {
            //取得存在cookie的当前账户id
            var pro_id = Int32.Parse(User.Identity.Name);
            var temp = await WorkAccessor.FindWorkInfo(request.work_id);
            if(temp!=null)
            {
                var num = await WorkAccessor.DeleteWork(request.work_id);
                return num;
            }          
            return -1;
        }
    }
}