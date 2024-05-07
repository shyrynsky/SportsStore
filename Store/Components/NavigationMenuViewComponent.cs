using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    private IStoreRepository _repository;

    public NavigationMenuViewComponent(IStoreRepository repo)
    {
        _repository = repo;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"]!;
        return View(_repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
    }
}