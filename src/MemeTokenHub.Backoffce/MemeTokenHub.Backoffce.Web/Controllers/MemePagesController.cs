using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Meme.Domain.Models;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    [Route("memepages")]
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

        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            var partnerId = GetRequestPartnerId();
            var list = await _memePageService.GetByOwnerIdAsync(partnerId);

            return View(list.Where(x => x.Status != PageStatus.Deleted));
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) RedirectToAction(nameof(Index));

            var page = await _memePageService.GetAsync(id);
            if (page == null) 
            {
                RedirectToAction(nameof(Index));
            }

            return View(page);
        }

        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] MemePageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
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

        [HttpGet("edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _memePageService.GetAsync(id);
            return View(model);
        }

        // POST: TenantController/Edit/5
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [FromForm] MemePageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var page = await _memePageService.GetAsync(id);
                    if (page != null)
                    {

                        page.CoinAddress = model.CoinAddress;
                        page.BuyUrl = model.BuyUrl;
                        page.Name = model.Name;
                        await _memePageService.UpdateAsync(page.Id!, page);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("{id}/delete")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) RedirectToAction(nameof(Index));

                var page = await _memePageService.GetAsync(id);
                if (page != null)
                {

                    page.Status = PageStatus.Deleted;
                    await _memePageService.UpdateAsync(page.Id!, page);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
