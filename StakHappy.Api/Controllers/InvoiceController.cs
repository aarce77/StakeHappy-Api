using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : BaseController<Core.Data.Model.Invoice, Core.Logic.InvoiceLogic>
    {
        [HttpGet]
        public override IQueryable<Core.Data.Model.Invoice> Get()
        {
            var criteria = GetInvoiceSearchCriteria(Guid.Empty);
            criteria.UserId = UserId;

            return Logic.Search(criteria);
        }

        [HttpGet("{id}")]
        public override Core.Data.Model.Invoice Get(Guid id)
        {
            var invoice = base.Get(id);
            // TODO: verify that the Client object is loaded and that the User_Id property is set.
            return invoice.Client.User_Id != UserId ? null : invoice;
        }

        [HttpPost]
        public IQueryable<Core.Data.Model.Invoice> Search([FromBody]Core.Data.Search.InvoiceCriteria criteria)
        {
            criteria.UserId = UserId;

            return Logic.Search(criteria);
        }

        internal virtual Core.Data.Search.InvoiceCriteria GetInvoiceSearchCriteria(Guid clientId)
        {
            return new Core.Data.Search.InvoiceCriteria { UserId = UserId, ClientId = clientId };
        }

        [HttpGet("{id}")]
        [Route("Item/New")]
        public Core.Data.Model.InvoiceItem GetNewInvoiceItem(Guid id)
        {
            var item = Logic.GetNewInvoiceItemModel();
            item.Invoice_Id = id;

            return item;
        }

        [HttpPut]
        [Route("Item/Save")]
        public void SaveInvoiceItem([FromBody] Core.Data.Model.InvoiceItem item)
        {
            Logic.SaveInvoiceItem(item);
        }

        [HttpDelete("{id}")]
        [Route("Item/Delete")]
        public ActionResult DeleteInvoiceItem(Guid id)
        {
            // TODO: consider checking if the invoice item belongs to the user before deleting
            return Ok(Logic.DeleteInvoiceItem(id));
        }
    }
}
