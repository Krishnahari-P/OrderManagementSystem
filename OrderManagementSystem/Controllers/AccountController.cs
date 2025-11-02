using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Security;
using OrderManagementSystem.Entity.ViewModel;

namespace OrderManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                var user=await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user != null)
                {
                    var result=await _signInManager.PasswordSignInAsync(user, loginViewModel.Password,false,false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                ModelState.AddModelError(String.Empty, "Invalid Username Or Password");
            }
            return View(loginViewModel);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                { 
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.UserName,
                    IsActive = true
                };
                var result=await _userManager.CreateAsync(user,registerViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach(var err in  result.Errors)
                {
                    ModelState.AddModelError(String.Empty, err.Description);
                }
            }
            return View(registerViewModel);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByNameAsync(User.Identity.Name);
                if(user!=null)
                {
                    var result = await _userManager.ChangePasswordAsync(user,changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login","Account");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, err.Description);
                    }
                }
                
            }
            return View(changePasswordViewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrRemoveRoles(String id)
        {
            List<AddOrRemoveRolesViewModel> models = new List<AddOrRemoveRolesViewModel>();
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                models.Add(new AddOrRemoveRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }
            return View(models);
        }
        [HttpPost]
        [ActionName("AddOrRemoveRoles")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddOrRemoveRoles(List<AddOrRemoveRolesViewModel> addOrRemoveRolesViewModel, String userId)
        {
            if (ModelState.IsValid)
            {
                bool flag = true;
                var user = await _userManager.FindByIdAsync(userId);
                IdentityResult result = new();
                foreach (var role in addOrRemoveRolesViewModel)
                {
                    if (role.IsSelected && !await _userManager.IsInRoleAsync(user, role.RoleName))
                    {
                        result = await _userManager.AddToRoleAsync(user, role.RoleName);
                        flag = result.Succeeded && flag;
                    }
                    else if (!role.IsSelected && await _userManager.IsInRoleAsync(user, role.RoleName))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                        flag = result.Succeeded && flag;
                    }
                }
                if (flag)
                {
                    return RedirectToAction(nameof(ListAllUsers));
                }
            }

            return View(addOrRemoveRolesViewModel);
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
