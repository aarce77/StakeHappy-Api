using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StakHappy.Api.Controllers
{
    public class PaymentController : BaseController<Core.Data.Model.Payment, Core.Logic.PaymentLogic>
    {
        [HttpGet("{invoiceId}")]
        [Route("GetInvoicePayments")]
        public IEnumerable<Core.Data.Model.Payment> GetInvoicePayments(Guid invoiceId)
        {
            return Logic.Find(p => p.Invoice_Id == invoiceId);
        }

        [HttpGet]
        [Route("PaymentTypes")]
        public IEnumerable<Core.Data.Model.PaymentType> GetPaymentTypes()
        {
            return Logic.GetPaymentTypes();
        }
    }
}
