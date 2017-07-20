using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<Core.Data.Model.User, Core.Logic.UserLogic>
    {
        
    }
}
