using Microsoft.AspNetCore.Mvc;
using Moq;
using Store.Controllers;
using Store.Models;

namespace Store.Tests;

public class HomeControllerTests
{
    [Fact]
    public void CanUseRepository()
    {
        //Arrange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new Product() { ProductId = 1, Name = "P1" },
            new Product() {ProductId = 2, Name = "P2"}
        }.AsQueryable<Product>);

        HomeController controller = new HomeController(mock.Object);
        
        //Act
        IEnumerable<Product>? result = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;
        
        //Assert
        Product[] products = result?.ToArray() ?? Array.Empty<Product>();
        Assert.True(products.Length == 2);
        Assert.Equal("P1", products[0].Name);
        Assert.Equal("P2", products[1].Name);
    }
}