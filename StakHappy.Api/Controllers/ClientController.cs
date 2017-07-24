using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : BaseController<Core.Data.Model.Client, Core.Logic.ClientLogic>
    {
        [HttpGet]
        [Route("GetNewClientContact")]
        public Core.Data.Model.ClientContact GetNewClientContact()
        {
            return Logic.GetNewClientContactModel();
        }

        [HttpPut("{clientId}")]
        [Route("SaveContact")]
        public void SaveClientContact(Guid clientId, [FromBody] Core.Data.Model.ClientContact clientContact)
        {
            clientContact.Client_Id = clientId;
            Logic.SaveContact(clientContact);
        }

        [HttpPost("{id}")]
        [Route("DeleteContact")]
        public void DeleteClientContact(Guid id)
        {
            // TODO consider checking if the client contact belongs to the correct client before deleting.
            Logic.DeleteContact(id);
        }
    }
}
