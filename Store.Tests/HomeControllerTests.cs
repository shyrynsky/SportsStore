using Microsoft.AspNetCore.Mvc;
using Moq;
using Store.Controllers;
using Store.Models;
using Store.Models.ViewModels;

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
        ProductsListViewModel result =
            controller.Index(null)?.ViewData.Model as ProductsListViewModel ?? new();
        
        //Assert
        Product[] products = result?.Products.ToArray() ?? Array.Empty<Product>();
        Assert.True(products.Length == 2);
        Assert.Equal("P1", products[0].Name);
        Assert.Equal("P2", products[1].Name);
    }

    [Fact]
    public void CanPaginate()
    {
        //Arrange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new Product(){ ProductId = 1, Name = "P1" },
            new Product() { ProductId = 2, Name = "P2" },
            new Product() { ProductId = 3, Name = "P3" },
            new Product() { ProductId = 4, Name = "P4" },
            new Product() { ProductId = 5, Name = "P5" }
        }.AsQueryable());

        HomeController controller = new HomeController(mock.Object);
        controller.PageSize = 3;
        
        //Act
        ProductsListViewModel result =
            controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();
        
        //Assert
        Product[] products = result?.Products.ToArray() ?? Array.Empty<Product>();
        Assert.True(products.Length == 2);
        Assert.Equal("P4", products[0].Name);
        Assert.Equal("P5", products[1].Name);
        
    }

    [Fact]
    public void CanSendPaginationViewModel()
    {
        // Arrange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {ProductId = 1, Name = "P1"},
            new Product {ProductId = 2, Name = "P2"},
            new Product {ProductId = 3, Name = "P3"},
            new Product {ProductId = 4, Name = "P4"},
            new Product {ProductId = 5, Name = "P5"}
        }).AsQueryable<Product>());
        // Arrange
        HomeController controller =
            new HomeController(mock.Object) { PageSize = 3 };
        // Act
        ProductsListViewModel result =
            controller.Index(null, 2)?.Model as ProductsListViewModel
            ?? new();
        // Assert
        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void CanFilterProducts()
    {
        // Arrange
        // - create the mock repository
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
            new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
            new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
            new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
            new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
        }).AsQueryable<Product>());
        // Arrange - create a controller and make the page size 3 items
        HomeController controller = new HomeController(mock.Object);
        controller.PageSize = 3;
        // Action
        Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model
            as ProductsListViewModel ?? new()).Products.ToArray();
        // Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
    }
}