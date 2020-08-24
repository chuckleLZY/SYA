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
    public class CommentController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public CommentController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }


        [HttpPost("CreateComment")]
        [AllowAnonymous]
        public async Task<int> CreateComment([FromBody] CommentRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = 6;//Int32.Parse(User.Identity.Name);
            //生成comment实例
            var comment = _mapper.Map<CommentEntity>(request);
            comment.sender_id=user_id;
            //新建comment
            var num=await CommentAccessor.Create(comment);
            
           return user_id;
        }

        
    }
}