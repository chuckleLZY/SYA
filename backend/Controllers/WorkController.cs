using System;
using System.Threading.Tasks;
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
        /// 学生用户创建工作
        /// chuckle 8.25
        ///</summery>
        [HttpGet("ViewOwnWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewOwnWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp=await WorkAccessor.FindOwnWork(stu_id);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    
                    var work_info=await WorkAccessor.FindWorkInfo(temp.TakesItem[i].work_id);
                    if(work_info!=null)
                    {
                        WorkResponse a=_mapper.Map<WorkResponse>(work_info);
                        
                        workItem.worklist.Add(a);
                        workItem.totalpage++;
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
        [HttpGet("ViewWorkInfo")]
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
        [HttpGet("ViewAllWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewAllWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);

            var temp=await WorkAccessor.FindAllWork();

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                        WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);
                        
                        workItem.worklist.Add(a);
                        workItem.totalpage++;
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        ///<summery>
        /// 用户搜索工作
        /// chuckle 8.25
        ///</summery>
        [HttpGet("FindWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> FindWork([FromBody] ViewWorkRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=request.pagenum;
            workItem.worklist=new List<WorkResponse>();

            //取得存在cookie的当前账户id
            var stu_id = Int32.Parse(User.Identity.Name);
            string search='%'+request.query+'%';
            var temp=await WorkAccessor.FindWork(search);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);
                        
                    workItem.worklist.Add(a);
                    workItem.totalpage++;
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }




        ///<summery>
        /// (非学生用户)创建工作
        /// 检查user.role
        /// dumei 08.23
        ///</summery>
        [HttpPost("CreateWork")]
        //[Authorize(Roles = "Provider")]
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
            var work = _mapper.Map<WorkEntity>(request);
            work.teacher_id = provider_id;
            await WorkAccessor.Create(work); //return work_id

            return Ok(_mapper.Map<WorkResponse>(work));
        }

        ///<summery>
        /// (非学生用户)查看历史发布工作
        /// 检查user.role
        /// dumei 08.23
        [HttpGet("ViewHistoryWork")]
        //[AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewHistoryWork()
        {
            var pro_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(pro_id) == Role.Student)
            {
                return BadRequest(new { message = "ViewHistoryWork is not for students. Students please try ViewOwnWork."});
            }

            WorkItemResponse workItem = new WorkItemResponse();
            workItem.totalpage=0;
            workItem.worklist = new System.Collections.Generic.List<WorkResponse>();

            var provide_list = await WorkAccessor.FindHistoryWork(pro_id);

            if(provide_list != null)
            {
                for(int i=0; i<provide_list.total; i++)
                {
                    WorkResponse wr = _mapper.Map<WorkResponse>(provide_list.workItem[i]);           
                    workItem.worklist.Add(wr);
                    workItem.totalpage++;
                }
                return Ok(workItem);
            }
            return Ok(-1); // Never arrive there
        }

    }
}