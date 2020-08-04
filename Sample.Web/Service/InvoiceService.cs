using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Web.Infrastructure;
using Sample.Web.Models;

namespace Sample.Web.Service
{
    public class InvoiceService : Service<InvoiceViewModel>, IInvoiceService
    {

        public InvoiceService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            IHttpContextAccessor httpContextAccesor,
            ILogger<Service<InvoiceViewModel>> logger)
            : base(settings, httpClient, httpContextAccesor, logger)
        {
            this.Entity = "Invoice";
        }

       
    }
}
