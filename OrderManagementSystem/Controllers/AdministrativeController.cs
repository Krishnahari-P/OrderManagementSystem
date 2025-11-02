using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OrderManagementSystem.Entity.ViewModel;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrativeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _nToastNotify;

        public AdministrativeController(RoleManager<IdentityRole> roleManager, IToastNotification nToastNotify)
        {
            _roleManager = roleManager;
            _nToastNotify = nToastNotify;
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
        public async Task<IActionResult> EditRole(String id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _nToastNotify.AddErrorToastMessage("Role not found!");
                return RedirectToAction(nameof(ListRoles));
            }
            EditRoleViewModel editRoleViewModel = new EditRoleViewModel
            {
                Id=role.Id,
                RoleName=role.Name
            };
            return View(editRoleViewModel);
        }
        [HttpPost]
        [ActionName("EditRole")]
        public async Task<IActionResult> ConfirmEditRole(EditRoleViewModel editRoleViewModel)
        {

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);
                role.Name = editRoleViewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, err.Description);
                }
            }
            return View(editRoleViewModel);
        }
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _nToastNotify.AddErrorToastMessage("Role not found!");
                return RedirectToAction(nameof(ListRoles));
            }

            var deleteRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(deleteRoleViewModel);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        public async Task<IActionResult> ConfirmDeleteRole(EditRoleViewModel deleteRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(deleteRoleViewModel.Id);
            if (role == null)
            {
                _nToastNotify.AddErrorToastMessage("Role not found!");
                return RedirectToAction(nameof(ListRoles));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _nToastNotify.AddSuccessToastMessage("Role deleted successfully!");
                return RedirectToAction(nameof(ListRoles));
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }

            return View(deleteRoleViewModel);
        }

    }
}
