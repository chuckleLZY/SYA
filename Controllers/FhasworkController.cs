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
    public class FhasworkController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public FhasworkController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        [HttpPost("CreateFhaswork")]
        [AllowAnonymous]
        public async Task<int> CreateFhaswork([FromBody] FhasworkRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            
            //var user_id = Int32.Parse(User.Identity.Name);
            //生成fhaswork实例
            var fhaswork = _mapper.Map<FhasworkEntity>(request);
            
            //新建fhaswork
            var num=await FhasworkAccessor.Create(fhaswork);
            
            return fhaswork.favorite_id;
        }

        [HttpPost("GetFhaswork")]
        [AllowAnonymous]
        public async Task<ActionResult<FhasworkResponse>> GetFhaswork(FhasworkRequest request)
        {
            //取得存在cookie的当前账户id
            //var user_id = Int32.Parse(User.Identity.Name);
            
            var temp=await FhasworkAccessor.Find(request.favorite_id,request.work_id);

            if(temp!=null)
            {           
                return Ok(_mapper.Map<FhasworkResponse>(temp));
            }
            return Ok(-1);
        }

        [HttpPost("ViewFhaswork")]
        [AllowAnonymous]
        public async Task<ActionResult<FhasworkResponse>> ViewFhaswork([FromBody] ViewRequest request)
        {
            FhasworkItemResponse FhasworkItem=new FhasworkItemResponse();
            
            FhasworkItem.fhasworklist=new List<FhasworkResponse>();

            //取得存在cookie的当前账户id
            //var user_id =Int32.Parse(User.Identity.Name);

            var temp=await FhasworkAccessor.ViewFhaswork(request.favorite_id);

             if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    var list=await FhasworkAccessor.Find(temp.FhasworkItem[i].favorite_id,temp.FhasworkItem[i].work_id);
                   
                    FhasworkResponse a=_mapper.Map<FhasworkResponse>(list);
                        
                    FhasworkItem.fhasworklist.Add(a);
                    
                }

                return Ok(FhasworkItem);
            }
            return Ok(-1);
        }




    }
}