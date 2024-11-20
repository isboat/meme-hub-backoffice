using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Partners.Management.Web.Models;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    public class MemePagesController : CustomBaseController
    {
        private readonly IMemePageService _memePageService;
        private readonly IEmailSender _emailSender;

        public MemePagesController(
            IMemePageService tenantService,
            IEmailSender emailSender)
        {
            _memePageService = tenantService;
            _emailSender = emailSender;
        }

        // GET: TenantController
        public async Task<ActionResult> Index()
        {
            var partnerId = GetRequestPartnerId();
            var list = await _memePageService.GetByOwnerIdAsync(partnerId);

            return View(list);
        }

        [HttpGet("/Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var tenant = await _memePageService.GetAsync(id);

            return View(tenant);
        }

        // GET: TenantController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TenantController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm]MemePageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //model.Id = Guid.NewGuid().ToString("N");
                    model.Created = DateTime.Now;
                    model.OwnerIds ??= [];

                    model.OwnerIds.Add(GetRequestPartnerId());

                    await _memePageService.CreateAsync(model);

                    //await _emailSender.SendEmailAsync(model.Email!, "onScreenSync platform created", EmailTemplates.GetTenantCreatedEmailBody(model));
                    //await _emailSender.SendEmailAsync(model.Email!, "General Data Protection Regulation (UK GDPR)", EmailTemplates.GetTenantGdprEmailBody(model));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TenantController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _memePageService.GetAsync(id);
            return View(model);
        }

        // POST: TenantController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [FromForm] MemePageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _memePageService.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("/Tenants/Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                //await _tenantService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
