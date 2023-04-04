using HR_Plus.Domain.Entities;
using HR_PLUS.Application.Models.DTOs;
using HR_PLUS.Application.Services.CompanyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_PLUS.Presentation.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<AppUser> _userManager;
        public CompanyController(ICompanyService companyService, UserManager<AppUser> userManager)
        {
            _companyService = companyService;
            _userManager = userManager;
        }
        [Authorize(Roles = "Manager")]       
        public async Task<IActionResult> Update(int id) => View(await _companyService.GetById(id));
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCompanyDTO model)
        {
            AppUser appUser = await _userManager.GetUserAsync(User);
            model.CompanyId = appUser.CompanyId;
            if (ModelState.IsValid)
            {
                await _companyService.Update(model);
                TempData["Success"] = "Company has been updated!";
                return RedirectToAction("AppUserIndex","AppUser");
            }
            else
            {
                TempData["Error"] = "Company has not been updated!";
                return View(model);
            }
        }
    }
}
