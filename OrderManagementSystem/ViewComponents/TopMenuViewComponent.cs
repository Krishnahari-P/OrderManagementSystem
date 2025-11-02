using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.ViewComponents
{
    public class TopMenuViewComponent:ViewComponent
    {
        private readonly IMenuRepository _menuRepository;

        public TopMenuViewComponent(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus=await _menuRepository.GetAllMenuAsync();
            return View("Default",menus);
        }
    }
}
