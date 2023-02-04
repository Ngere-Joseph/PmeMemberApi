using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PmeMemberApi.Core.IDao;
using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PmeMemberApiController : ControllerBase
    {
        private readonly ILogger<PmeMemberApiController> _logger;
        private readonly IPmeMemberApiDao _pmeMemberDao;

        public PmeMemberApiController(ILogger<PmeMemberApiController> logger, IPmeMemberApiDao pmeMemberDao)
        {
            _logger = logger;
			_pmeMemberDao = pmeMemberDao;
        }

        [Authorize]
        [HttpGet(Name = "GetMembers")]
        public async Task<IActionResult> Get()
        {
            var forecasts = await _pmeMemberDao.GetAllMembers();
            return Ok(forecasts);
        }

        [Authorize]
        [HttpGet, Route("{id:long}")]
        public async Task<IActionResult> GetMembertById([FromRoute] long id)
        {
            var forecast = await _pmeMemberDao.GetMember(id);
            return Ok(forecast);
        }

        [Authorize(Roles = AppUserRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] PmeMemberApi member)
        {
            await _pmeMemberDao.AddMember(member);
            return Ok();
        }
        
        [Authorize(Roles = AppUserRole.Admin)]
        [HttpDelete, Route("{id:long}")]
        public async Task<IActionResult> DeleteMember([FromRoute] long id)
        {
            await _pmeMemberDao.DeleteMember(id);
            return Ok();
        }

        [Authorize(Roles = AppUserRole.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateMember([FromBody] PmeMemberApi member)
        {
            await _pmeMemberDao.AddMember(member);
            return Ok();
        }
    }
}