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
    public class HistoryController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public HistoryController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        [HttpPost("CreateHistory")]
        [AllowAnonymous]
        public async Task<int> CreateHistory([FromBody] HistoryRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var student_id = 3;//Int32.Parse(User.Identity.Name);
            //生成comment实例
            var history = _mapper.Map<HistoryEntity>(request);
            history.student_id=student_id;
            //新建comment
            var num=await HistoryAccessor.Create(history);
            
           return student_id;
        }

        [HttpPost("GetHistory")]
        [AllowAnonymous]
        public async Task<ActionResult<HistoryResponse>> GetHistory()
        {
            //取得存在cookie的当前账户id
            var student_id = 3;//Int32.Parse(User.Identity.Name);
            //查找当前id是否存在history
            var temp=await HistoryAccessor.Find(student_id);

            if(temp!=null)
            {            
                return Ok(_mapper.Map<HistoryResponse>(temp));
            }
            return Ok(-1);
        }
    }
}