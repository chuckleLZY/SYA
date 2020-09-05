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
    public class FavoriteController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public FavoriteController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }



        //新建收藏夹
        [HttpPost("CreateFavorite")]
        [AllowAnonymous]
        public async Task<int> CreateFavorite([FromBody] FavoriteRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            //生成favorite实例
            var favorite = _mapper.Map<FavoriteEntity>(request);
            favorite.user_id=user_id;
            //新建favorite
            var num=await FavoriteAccessor.Create(favorite);
         
            return  num;
        }

        //删除收藏夹
        [HttpPost("DeleteFavorite")]
        [AllowAnonymous]
        public async Task<int> DeleteFavorite(FavoriteRequest request)
        {

           
            var temp=await FavoriteAccessor.Find(request.favorite_id);

            if(temp!=null)
            {
                var num =FavoriteAccessor.Delete(request.favorite_id);
                return num.Result;
            }
            return 0;
        }

        //修改收藏夹名字
        [HttpPut("UpdateFavorite")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> UpdateFavorite([FromBody] FavoriteRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            //生成favorite实例
            var temp_favorite = _mapper.Map<FavoriteEntity>(request);
           
            //查找是否存在favorite
            var temp=await FavoriteAccessor.Find(request.favorite_id);

            if(temp!=null)
            {
                var num=FavoriteAccessor.Change(temp);
                temp.user_id=user_id;
                temp.favorite_name=request.favorite_name;
                return Ok(_mapper.Map<FavoriteResponse>(temp));
            }
            return Ok(-1);
        }

        //分页查看个人收藏夹
        [HttpPost("GetFavorite")]
        [AllowAnonymous]
        public async Task<ActionResult<FavoriteItemResponse>> GetFavorite([FromBody] ViewFavoriteRequest request)
        {
            FavoriteItemResponse ans=new FavoriteItemResponse();
            ans.totalpage=0;
            ans.pagenum=request.pagenum;
            ans.FavoriteItem=new List<FavoriteResponse>();
            
            var start=(request.pagenum-1)*request.pagesize;
            var end=request.pagenum*request.pagesize-1;

            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            
            var temp=await FavoriteAccessor.ViewFavorite(user_id);

            for(int i=0;i<temp.total;i++)
            {                    
                ans.totalpage++;
                if(i>=start&&i<=end)
                {
                    var response =_mapper.Map<FavoriteResponse>(temp.FavoriteItem[i]);

                    ans.FavoriteItem.Add(response);
                }

            }
            return Ok(ans);
        }

        //查看收藏夹内的工作
        [HttpPost("GetFavoriteInfo")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkItemResponse>> GetFavoriteInfo([FromBody] InfoFavoriteRequest request)
        {
            WorkItemResponse workItem=new WorkItemResponse();
            workItem.totalpage=0;
            workItem.pagenum=0;
            workItem.worklist=new List<WorkResponse>();
            
            var temp=await FavoriteAccessor.ViewInfo(request.favorite_id);

            if(temp!=null)
            {
                for(int i=0;i<temp.total;i++)
                {
                    workItem.totalpage++;

                    WorkResponse a=_mapper.Map<WorkResponse>(temp.workItem[i]);                       
                    workItem.worklist.Add(a);
                    
                    
                }

                return Ok(workItem);
            }
            return Ok(-1);
        }

        //工作添加到收藏夹中
        [HttpPost("AddFavoriteWork")]
        [AllowAnonymous]
        public async Task<int> AddFavoriteWork([FromBody]HasFavoriteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            var favorite = _mapper.Map<HasFavoriteEntity>(request);
            var temp=await FavoriteAccessor.AddFavorWork(favorite);
            var ans=await FavoriteAccessor.ChangeNum(request.favorite_id);
            if(temp>0)
            {
                return 1;
            }
            return 0;
        }

        //从收藏夹中删除工作
        [HttpDelete("DeleteFavoriteWork")]
        [AllowAnonymous]
        public async Task<int> DeleteFavoriteWork([FromBody]HasFavoriteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = Int32.Parse(User.Identity.Name);
            var favorite = _mapper.Map<HasFavoriteEntity>(request);
            var temp=await FavoriteAccessor.DeleteFavorWork(favorite);
            var ans=await FavoriteAccessor.ChangeNum(request.favorite_id);
            if(temp>0)
            {
                return 1;
            }
            return 0;
        }

    }
}