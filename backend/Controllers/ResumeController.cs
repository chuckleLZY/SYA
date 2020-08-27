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
    public class ResumeController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public ResumeController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }


        [HttpPost("CreateResume")]
        [AllowAnonymous]
        public async Task<ActionResult<ResumeResponse>> CreateResume([FromBody] ResumeRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //取得存在cookie的当前账户id
            var resume_stu_id = Int32.Parse(User.Identity.Name);
            //生成resume实例
            var temp_resume = _mapper.Map<ResumeEntity>(request);
            //查找当前id是否存在resume           
            var temp=await ResumeAccessor.Find(resume_stu_id);

            temp_resume.student_id=resume_stu_id;
            if(temp==null)
            {
                //新建resume
                var num = ResumeAccessor.Create(temp_resume);
                temp_resume.resume_id=num.Result;
                return Ok(_mapper.Map<ResumeResponse>(temp_resume));
            }
            return Ok(-1);
        }

        [HttpDelete("DeleteResume")]
        [AllowAnonymous]
        public async Task<ActionResult<ResumeResponse>> DeleteResume()
        {
            //取得存在cookie的当前账户id
            var resume_stu_id = Int32.Parse(User.Identity.Name);
            //查找当前id是否存在resume
            var temp=await ResumeAccessor.Find(resume_stu_id);

            if(temp!=null)
            {
                var num =ResumeAccessor.Delete(resume_stu_id);
                return Ok(_mapper.Map<ResumeResponse>(temp));
            }
            return Ok(-1);
        }

        [HttpPut("UpdateResume")]
        [AllowAnonymous]
        public async Task<ActionResult<ResumeResponse>> UpdateResume([FromBody] ResumeRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //取得存在cookie的当前账户id
            var resume_stu_id = Int32.Parse(User.Identity.Name);
            //生成resume实例
            var temp_resume = _mapper.Map<ResumeEntity>(request);
            //查找当前id是否存在resume
            var temp=await ResumeAccessor.Find(resume_stu_id);

            if(temp!=null)
            {
                var num=ResumeAccessor.Change(temp);
                return Ok(_mapper.Map<ResumeResponse>(temp));
            }
            return Ok(-1);
        }

        [HttpPost("GetResume")]
        [AllowAnonymous]
        public async Task<ActionResult<ResumeResponse>> GetResume()
        {
            //取得存在cookie的当前账户id
            var resume_stu_id = Int32.Parse(User.Identity.Name);
            //查找当前id是否存在resume
            var temp=await ResumeAccessor.Find(resume_stu_id);

            if(temp!=null)
            {            
                return Ok(_mapper.Map<ResumeResponse>(temp));
            }
            return Ok(-1);
        }

        [HttpPost("GetResumeInfo")]
        [AllowAnonymous]
        public async Task<ActionResult<ResumeResponse>> GetResumeInfo([FromBody] ViewResumeRequest request)
        {

            //查找当前id是否存在resume
            var temp=await ResumeAccessor.FindInfo(request.resume_id);

            if(temp!=null)
            {            
                return Ok(_mapper.Map<ResumeResponse>(temp));
            }
            return Ok(-1);
        }




    }
}