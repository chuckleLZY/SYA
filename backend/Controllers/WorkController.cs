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

        [HttpPost("ViewOwnWork")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> ViewOwnWork()
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.total=0;
            workItem.worklist=new System.Collections.Generic.List<WorkResponse>();
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
                        workItem.total++;
                    }
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        [HttpGet("ViewWorkInfo")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkResponse>> ViewWorkInfo([FromBody]FindworkRequest temp)
        {

            var work_info=await WorkAccessor.FindWorkInfo(temp.work_id);

            if(work_info!=null)
            {
                return Ok(_mapper.Map<WorkResponse>(work_info));
            }
                return Ok(-1);
        }

        




    }
}