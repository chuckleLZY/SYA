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




        [HttpGet("CreateFavorite")]
        [AllowAnonymous]
        public async Task<int> CreateFavorite([FromBody] FavoriteRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return -5;
            }
            //取得存在cookie的当前账户id
            var user_id = 3;//Int32.Parse(User.Identity.Name);
            //生成favorite实例
            var favorite = _mapper.Map<FavoriteEntity>(request);
            favorite.user_id=user_id;
            //新建favorite
            var num=await FavoriteAccessor.Create(favorite);
         
            return  favorite.favorite_id;
        }

        [HttpDelete("DeleteFavorite")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> DeleteFavorite(FavoriteRequest request)
        {
            //取得存在cookie的当前账户id
            //var user_id = Int32.Parse(User.Identity.Name);
           
            var temp=await FavoriteAccessor.Find(request.favorite_id);

            if(temp!=null)
            {
                var num =FavoriteAccessor.Delete(request.favorite_id);
                return Ok(_mapper.Map<FavoriteResponse>(temp));
            }
            return Ok(-1);
        }

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
            var user_id = 3;//Int32.Parse(User.Identity.Name);
            //生成favorite实例
            var temp_favorite = _mapper.Map<FavoriteEntity>(request);
           
            //查找是否存在favorite
            var temp=await FavoriteAccessor.Find(request.favorite_id);

            if(temp!=null)
            {
                var num=FavoriteAccessor.Change(temp);
                temp.user_id=user_id;
                return Ok(_mapper.Map<FavoriteResponse>(temp));
            }
            return Ok(-1);
        }



        [HttpGet("GetFavorite")]
        [AllowAnonymous]
        public async Task<ActionResult<FavoriteResponse>> GetFavorite(FavoriteRequest request)
        {
            //取得存在cookie的当前账户id
            var user_id =3;// Int32.Parse(User.Identity.Name);
            //查找当前id是否有favorite
            var temp=await FavoriteAccessor.Find(request.favorite_id);

            if(temp!=null)
            {
                temp.user_id=user_id;            
                return Ok(_mapper.Map<FavoriteResponse>(temp));
            }
            return Ok(-1);
        }
    }
}