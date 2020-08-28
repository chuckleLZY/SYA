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
    public class AnnounceController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public AnnounceController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }


        [HttpPost("CreateAnnounce")]
        [AllowAnonymous]
        public async Task<int> CreateAnnounce([FromBody] AnnounceRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            // if (await UserAccessor.CheckRole(user_id) == Role.Admin)
            // {
            //     return -10;
            // }
            //生成annouce实例
            var announcement = _mapper.Map<AnnounceEntity>(request);
            announcement.user_id=user_id;
            //新建annouce
            var num=await AnnounceAccessor.Create(announcement);
           
            return user_id;
        }

        [HttpPost("GetAnnounce")]
        [AllowAnonymous]
        public async Task<ActionResult<AnnounceItemResponse>> GetAnnounce([FromBody]ViewAnnounceRequest request)
        {
            AnnounceItemResponse ans=new AnnounceItemResponse();
            ans.totalpage=0;
            ans.pagenum=request.pagenum;
            ans.AnnounceItem=new List<AnnounceResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            var temp=await AnnounceAccessor.Find();
            for(int i=0;i<temp.totalpage;i++)
            {                    
                ans.totalpage++;
                if(i>=start&&i<=end)
                {
                    var response =_mapper.Map<AnnounceResponse>(temp.AnnounceItem[i]);
                    ans.AnnounceItem.Add(response);
                }
            }
            return Ok(ans);
        }


        [HttpPost("ViewedAnnounce")]
        [AllowAnonymous]
        public async Task<int> ViewedAnnounce([FromBody] AnnounceStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var temp_entity = _mapper.Map<AnnounceEntity>(request);
            var temp =await AnnounceAccessor.Change(temp_entity);

            return temp;
        }


        [HttpPost("DeleteAnnounce")]
        [AllowAnonymous]
        public async Task<int> DeleteAnnounce([FromBody] AnnounceStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var temp_entity = _mapper.Map<AnnounceEntity>(request);
            var temp =await AnnounceAccessor.Delete(temp_entity);

            return temp;
        }
    }
}