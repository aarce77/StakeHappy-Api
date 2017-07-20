using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController<M, L> : Controller
        where M : class, Core.Data.Model.IEntity, new()
        where L : Core.Logic.LogicBase<M>, new()
    {
        public readonly Guid UserId;
        public readonly L Logic;
        public BaseController()
        {
            //Guid.TryParse(this.Request.Headers["UserId"], out UserId);
            Logic = new L();

            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext == null || httpContext.Request == null)
                return;

            Microsoft.Extensions.Primitives.StringValues values;
            if (httpContext.Request.Headers.TryGetValue("UserId", out values))
                Guid.TryParse(values.FirstOrDefault(), out UserId);
        }

        // GET api/controller
        [HttpGet]
        public virtual IQueryable<M> Get()
        {
            return Logic.GetAll();
        }

        // GET api/controller/5
        [HttpGet("{id}")]
        public virtual M Get(Guid id)
        {
            return Logic.Get(id);
        }

        // POST api/controller
        [HttpPost]
        public void Post([FromBody]M entity)
        {
            Logic.Save(entity);
        }

        // PUT api/controller/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]M entity)
        {
            Logic.Save(entity);
        }

        // DELETE api/controller/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            Logic.Delete(id);
        }

        // GET api/controller/GetNew
        [HttpGet("New")]
        public virtual M GetNew()
        {
            return Logic.GetNewModelIntance();
        }
    }
}
