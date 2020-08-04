using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Web.Models;
using Sample.Web.Service;

namespace Sample.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(ILogger<HomeController> logger, IInvoiceService invoiceService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(InvoiceViewModel model)
        {
            _invoiceService.Insert(model);
            return View();
        }

    }
}
