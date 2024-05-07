using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Store.Infrastructure;
using Store.Models;

namespace Store.Pages;

public class CartModel : PageModel
{
    private IStoreRepository _repository;
    public CartModel(IStoreRepository repo) {
        _repository = repo;
    }
    public Cart? Cart { get; set; }
    public string ReturnUrl { get; set; } = "/";
    public void OnGet(string returnUrl) {
        ReturnUrl = returnUrl ?? "/";
        Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
    }
    public IActionResult OnPost(long productId, string returnUrl) {
        Product? product = _repository.Products
            .FirstOrDefault(p => p.ProductId == productId);
        if (product != null) {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddItem(product, 1);
            HttpContext.Session.SetJson("cart", Cart);
        }
        return RedirectToPage(new { returnUrl = returnUrl });
    }
}