using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.ViewModel;

namespace OrderManagementSystem.Controllers
{
    public class AdministrativeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrativeController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = new()
                {
                    Name = createRoleViewModel.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, err.Description);
                }
            }
            return View(createRoleViewModel);
        }

    }
}
