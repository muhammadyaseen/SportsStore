using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestFixture]
    class ProductControllerTests
    {
        private Mock<IProductRepository> mock;
        private ProductController controller;

        [SetUp]
        public void InitTestData()
        {
            this.mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                    new Product {ProductID = 1, Name = "P1", Category = "Watersport" },
                    new Product {ProductID = 2, Name = "P2", Category = "Chess" },
                    new Product {ProductID = 3, Name = "P3", Category = "Chess" },
                    new Product {ProductID = 4, Name = "P4", Category = "Air" },
                    new Product {ProductID = 5, Name = "P5", Category = "Air" }
                    }.AsQueryable());

            this.controller = new ProductController(mock.Object);
        }


        [TestCase]
        public void Can_Paginate()
        {
            // Arrange

            // - done in init func

            // create a controller and make the page size 3 items
            controller.PageSize = 3;

            // Action
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestCase]
        public void Sends_Proper_Data_To_View()
        {
            //Act
            controller.PageSize = 2;

            ProductListViewModel vm = (ProductListViewModel)controller.List(null, 3).Model;

            PagingInfo info = vm.PagingInfo;

            //Assert
            Assert.AreEqual(info.CurrentPage, 3);
            Assert.AreEqual(info.ItemsPerPage, 2);
            Assert.AreEqual(info.TotalItems, 5);
            Assert.AreEqual(info.TotalPages, 3);
        }

        [TestCase]
        public void Select_Correct_Category()
        {

            controller.PageSize = 3;

            ProductListViewModel vm = (ProductListViewModel)controller.List("Chess", 1).Model;
            Product[] result = vm.Products.ToArray();

            PagingInfo info = vm.PagingInfo;

            //Assert
            Assert.IsTrue(result[0].Category == "Chess" && result[0].Name == "P2");
            Assert.IsTrue(result[1].Category == "Chess" && result[1].Name == "P3");

            Assert.AreEqual(vm.CurrentCategory, "Chess");

        }

        [TestCase]
        public void Distinct_And_Ordered_Nav_Links()
        {
            NavController controller = new NavController(mock.Object);
            List<string> categories = ((IEnumerable<string>)controller.Menu("Chess").Model).ToList();


            //Assert
            Assert.AreEqual(categories.Count, 3);

            Assert.IsTrue(categories[0] == "Air");

            Assert.IsTrue(categories[1] == "Chess");

            Assert.IsTrue(categories[2] == "Watersport");


        }

        [TestCase]
        public void Indicates_Correct_Selected_Category()
        {

            NavController controller = new NavController(mock.Object);

            string selectedCategory = controller.Menu("Chess").ViewBag.SelectedCategory;

            Assert.AreEqual("Chess", selectedCategory);

        }
    }
}
