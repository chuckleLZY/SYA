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
            var role=await UserAccessor.CheckRole(user_id);
            if (role == Role.Student ||role==Role.Provider )
            {
                return -10;
            }
            //生成annouce实例
            var announcement = _mapper.Map<AnnounceEntity>(request);
            announcement.user_id=user_id;
            //新建annouce
            var num=await AnnounceAccessor.Create(announcement);
            var s= await AnnounceAccessor.Createsend(num);
            return num;
        }

        //获取用户未删除的所有公告
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

            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);

            var temp=await AnnounceAccessor.Find(user_id);
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

        //查看公告详细信息
        [HttpPost("GetAnnounceContent")]
        [AllowAnonymous]
        public async Task<AnnounceResponse> GetAnnounceContent([FromBody] AnnounceStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var ans=await AnnounceAccessor.FindInfo(request.announcement_id);
            
            var temp_entity = _mapper.Map<AnnounceEntity>(request);
            temp_entity.user_id=temp_id;
            var temp =await AnnounceAccessor.Change(temp_entity);
            ans.status=1;
            return _mapper.Map<AnnounceResponse>(ans);
        }

        //非管理员删除公告
        [HttpPost("DeleteAnnounce")]
        [AllowAnonymous]
        public async Task<int> DeleteAnnounce([FromBody] AnnounceStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var temp_entity = _mapper.Map<AnnounceEntity>(request);
            temp_entity.user_id=temp_id;
            var temp =await AnnounceAccessor.Delete(temp_entity);

            return temp;
        }
    
        //管理员删除公告
        [HttpPost("DeleteAnnounceAll")]
        [AllowAnonymous]
        public async Task<int> DeleteAnnounceAll([FromBody] AnnounceStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);
            var role=await UserAccessor.CheckRole(temp_id);
            if (role == Role.Student ||role==Role.Provider )
            {
                return -10;
            }
            var temp_entity = _mapper.Map<AnnounceEntity>(request);
            var temp =await AnnounceAccessor.DeleteAll(temp_entity);

            return temp;
        }

        //管理员查看自己发布的所有公告
        [HttpPost("GetSendAnnounce")]
        [AllowAnonymous]
        public async Task<ActionResult<AnnounceItemResponse>> GetSendAnnounce([FromBody]ViewAnnounceRequest request)
        {
            AnnounceItemResponse ans=new AnnounceItemResponse();
            ans.totalpage=0;
            ans.pagenum=request.pagenum;
            ans.AnnounceItem=new List<AnnounceResponse>();

            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            // var role=await UserAccessor.CheckRole(user_id);
            // if (role == Role.Student ||role==Role.Provider )
            // {
            //     return -10;
            // }
            var temp=await AnnounceAccessor.FindSend(user_id);
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
    
    
    }
}