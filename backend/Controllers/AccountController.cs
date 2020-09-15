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
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly AuthenticationProperties authProperties;

        public AccountController(IMapper mapper)
        {
            _mapper = mapper;
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
            };
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountResponse>> PostLogin([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = await AccountAccessor.Find(request.Username);
            if (account == null)
            {
                return BadRequest(new { message = "Incorrect username/email" });
            }

            if (!BCrypt.Net.BCrypt.Verify(Sha512Hmac.HashPassword(request.Password), account.Password))
            {
                return BadRequest(new { message = "Incorrect password" });
            }

            // issue cookie
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(_mapper.Map<AccountResponse>(account));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountResponse>> PostRegister([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = _mapper.Map<AccountEntity>(request);
            account.Password = BCrypt.Net.BCrypt.HashPassword(Sha512Hmac.HashPassword(request.Password), 10);
            account.Id = await AccountAccessor.Create(account);
            await UserAccessor.Create(account); // Create user at the same time
            await AnnounceAccessor.SetNewSend(account.Id);//create user's announce
            // issue cookie
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(_mapper.Map<AccountResponse>(account));
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> PostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [HttpPost("LoginStatus")]
        public async Task<ActionResult> GetLoginStatus()
        {
            var account = await AccountAccessor.Read(Int32.Parse(User.Identity.Name));
            if (account == null)
            {
                return Ok(-1);
            }

            return Ok(_mapper.Map<AccountResponse>(account));
        }
    }
}
