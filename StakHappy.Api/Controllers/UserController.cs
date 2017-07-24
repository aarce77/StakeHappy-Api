using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<Core.Data.Model.User, Core.Logic.UserLogic>
    {
        [HttpGet("{username}")]
        [Route("GetByUsername")]
        public Core.Data.Model.User GetByUsername(string username)
        {
            return Logic.GetByUsername(username);
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        [Route("IsUsernameInUse")]
        public ActionResult IsUsernameInUse(string username)
        {
            return Ok(Logic.IsUserNameInUse(username));
        }

        [HttpPost("{id}")]
        [Route("Deactivate")]
        public void Deactivate(Guid id)
        {
            Logic.Deactivate(id);
        }
    }
}
