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
    public class MessageController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public MessageController(IMapper mapper)
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
        /// 老师发送信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("CreateMessage")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageResponse>> CreateMessage([FromBody] MessageRequest request)
        {
            //判断request里是否满足前置条件
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);
            if (await UserAccessor.CheckRole(temp_id) == Role.Student)
            {
                return BadRequest(new { message = "Student cannot send message"});
            }
            
            var temp_entity = _mapper.Map<MessageEntity>(request);
            temp_entity.sender_id=temp_id;
          
            var temp=await MessageAccessor.Create(temp_entity);
            temp_entity.message_id=temp;
            temp_entity.sender_name=await UserAccessor.GetUserName(temp_entity.sender_id);
            temp_entity.receiver_name=await UserAccessor.GetUserName(temp_entity.receiver_id);

            return Ok(_mapper.Map<MessageResponse>(temp_entity));

        }

        ///<summery>
        /// 用户删除信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("DeleteMessage")]
        [AllowAnonymous]
        public async Task<int> DeleteMessage([FromBody] MessageStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var temp_entity = _mapper.Map<MessageEntity>(request);
            var temp =await MessageAccessor.Delete(temp_entity);

            return temp;
        }

        ///<summery>
        /// 用户已查看信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("ViewedMessage")]
        [AllowAnonymous]
        public async Task<int> ViewedMessage([FromBody] MessageStatusRequest request)
        {
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);

            var temp_entity = _mapper.Map<MessageEntity>(request);
            var temp =await MessageAccessor.Change(temp_entity);

            return temp;
        }


        ///<summery>
        /// 查找用户发送的信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("FindSendMessage")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageItemResponse>> FindSendMessage()
        {
            MessageItemResponse ans=new MessageItemResponse();
            ans.total=0;
            ans.messageItem=new List<MessageResponse>();
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);
          
            var temp=await MessageAccessor.FindSend(temp_id);
            for(int i=0;i<temp.total;i++)
            {
                var response =_mapper.Map<MessageResponse>(temp.messageItem[i]);
                response.sender_name=await UserAccessor.GetUserName(response.sender_id);
                response.receiver_name=await UserAccessor.GetUserName(response.receiver_id);
                ans.total++;
                ans.messageItem.Add(response);
            }
            return Ok(ans);
        }

        ///<summery>
        /// 查找用户接收的信息
        /// chuckle 8.28
        ///</summery>
        [HttpPost("FindReceiveMessage")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageItemResponse>> FindReceiveMessage()
        {
            MessageItemResponse ans=new MessageItemResponse();
            ans.total=0;
            ans.messageItem=new List<MessageResponse>();
            //取得存在cookie的当前账户id
            var temp_id = Int32.Parse(User.Identity.Name);
          
            var temp=await MessageAccessor.FindReceive(temp_id);
            for(int i=0;i<temp.total;i++)
            {
                var response =_mapper.Map<MessageResponse>(temp.messageItem[i]);
                response.sender_name=await UserAccessor.GetUserName(response.sender_id);
                response.receiver_name=await UserAccessor.GetUserName(response.receiver_id);
                ans.total++;
                ans.messageItem.Add(response);
            }
            return Ok(ans);
        }


    }
}