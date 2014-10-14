using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
new Product {ProductID = 1, Name = "P1", Category = "Apples"},
new Product {ProductID = 2, Name = "P2", Category = "Apples"},
new Product {ProductID = 3, Name = "P3", Category = "Plums"},
new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
                    }.AsQueryable());
            /*create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;
            // Assert
            Product[] prodArray = result.Products.ToArray();

            Assert.AreEqual(prodArray.Length, 2);
            Assert.IsTrue(prodArray[0].Name == "P2" && prodArray[0].Category == "Cat2");
            Assert.IsTrue(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");
            */
            NavController target = new NavController(mock.Object);
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");

        }
        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
                }.AsQueryable());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);

        }
    }
}
