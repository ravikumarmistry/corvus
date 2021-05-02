using Corvus.Core.Services;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Csdl;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Text;

namespace Corvus.Core.Test.Services
{
    [TestFixture]
    public class Tests
    {
        private EDMService testSubject = null;

        [SetUp]
        public void Setup()
        {
            testSubject = new EDMService();

        }

        [Test]
        public async Task Test1()
        {
            var customerModel = new EntityType()
            {
                Name = "Customer",
                Properties = new List<EntityProperty>()
                {
                    new EntityProperty()
                    {
                        Name = "Id",
                        Type = "Guid",
                        IsNullable = false,
                        IsKey = true,
                    },
                    new EntityProperty()
                    {
                        Name = "Id2",
                        Type = "Guid",
                        IsNullable = false,
                        IsKey = false,
                    },
                    new EntityProperty()
                    {
                        Name = "Name",
                        Type = "String",
                        IsNullable = false,
                        Maxlength = 30
                    },
                    new EntityProperty()
                    {
                        Name = "Mail",
                        Type = "String",
                        IsNullable = false,
                        Maxlength = 50
                    },
                    new EntityProperty()
                    {
                        Name = "ZipCode",
                        Type = "String",
                        Maxlength = 20
                    },
                    new EntityProperty()
                    {
                        Name = "Address",
                        Type = "String",
                        Maxlength = 20
                    }
                },
            };
            var productModel = new EntityType()
            {
                Name = "Product",
                Properties = new List<EntityProperty>()
                {
                    new EntityProperty()
                    {
                        Name = "Id",
                        Type = "Guid",
                        IsNullable = false,
                        IsKey = true,
                    },
                    new EntityProperty()
                    {
                        Name = "Name",
                        Type = "String",
                        IsNullable = false,
                        Maxlength = 30
                    },
                    new EntityProperty()
                    {
                        Name = "Kind",
                        Type = "String",
                        IsNullable = false,
                        Maxlength = 50
                    },
                    new EntityProperty()
                    {
                        Name = "Price",
                        Type = "Int32",
                        Maxlength = 20
                    }
                }

            };
            var orderModel = new EntityType()
            {
                Name = "Order",
                Properties = new List<EntityProperty>()
                {
                    new EntityProperty()
                    {
                        Name = "Id",
                        Type = "Guid",
                        IsNullable = false,
                        IsKey = true,
                    },
                    new EntityProperty()
                    {
                        Name = "CustomerId",
                        Type = "Guid",
                        IsNullable = false,
                        Maxlength = 30
                    },
                    new EntityProperty()
                    {
                        Name = "OrderDate",
                        Type = "DateTimeOffset",
                        IsNullable = false,
                        Maxlength = 50
                    },
                    new EntityProperty()
                    {
                        Name = "TotalPrice",
                        Type = "Decimal",
                    }
                },
            };
            var orderDetailModel = new EntityType()
            {
                Name = "OrderDetail",
                Properties = new List<EntityProperty>()
                {
                    new EntityProperty()
                    {
                        Name = "Id",
                        Type = "Guid",
                        IsNullable = false,
                        IsKey = true,
                    },
                    new EntityProperty()
                    {
                        Name = "OrderId",
                        Type = "Guid",
                        IsNullable = false,
                    },
                   new EntityProperty()
                    {
                        Name = "ProductId",
                        Type = "Guid",
                        IsNullable = false,
                    },
                    new EntityProperty()
                    {
                        Name = "Price",
                        Type = "Decimal",
                    }
                }

            };

            await testSubject.AddEntityType(customerModel);
            await testSubject.AddEntityType(productModel);
            await testSubject.AddEntityType(orderModel);
            await testSubject.AddEntityType(orderDetailModel);
            //await testSubject.AddEntityNavigation(new EntityNavigation()
            //{
            //    Name = "Customer_Have_Many_Orders",
            //    SourceEntity = "Customer",
            //    TargateEntity = "Order",
            //    TargetMultiplicity = "Many"
            //});

            //await testSubject.AddEntityNavigation(new EntityNavigation()
            //{
            //    Name = "Order_belongs_to_one_customer",
            //    SourceEntity = "Order",
            //    TargateEntity = "Customer",
            //    TargetMultiplicity = "One",
            //    SourceProperty = "CustomerId",
            //    TargateProperty = "Id2"
            //});

            await testSubject.AddEntityNavigation(new EntityNavigation()
            {
                SourceEntity = "Order",
                TargateEntity = "Customer",
                ForwordNavigation = new NavigationData()
                {
                    Name = "Customer",
                    DependentProperty = "CustomerId",
                    Multiplicity = "One",
                    PrincipleProperty = "Id2"
                },
                BackwordNavigation = new NavigationData()
                {
                    Name = "Orders",
                    Multiplicity = "Many"
                }
            });

            await testSubject.AddEntityNavigation(new EntityNavigation()
            {
                SourceEntity = "OrderDetail",
                TargateEntity = "Order",
                ForwordNavigation = new NavigationData()
                {
                    Name = "Order",
                    DependentProperty = "OrderId",
                    Multiplicity = "One",
                    PrincipleProperty = "Id"
                },
                BackwordNavigation = new NavigationData()
                {
                    Name = "OrderDetails",
                    Multiplicity = "Many"
                }
            });

            await testSubject.AddEntityNavigation(new EntityNavigation()
            {
                SourceEntity = "OrderDetail",
                TargateEntity = "Product",
                ForwordNavigation = new NavigationData()
                {
                    Name = "Product",
                    DependentProperty = "ProductId",
                    Multiplicity = "One",
                    PrincipleProperty = "Id"
                },
                BackwordNavigation = new NavigationData()
                {
                    Name = "OrderDetails",
                    Multiplicity = "Many"
                }
            });


            await testSubject.AddService(new EntitySet()
            {
                EntityType = "Customer",
                Name = "Customers"
            });

            await testSubject.AddService(new EntitySet()
            {
                EntityType = "Order",
                Name = "Orders",
            });

            await testSubject.AddService(new EntitySet()
            {
                EntityType = "OrderDetail",
                Name = "OrderDetails",
            });

            await testSubject.AddService(new EntitySet()
            {
                EntityType = "Product",
                Name = "Products",
                
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "OrderDetails",
                ServiceName = "Products",
                TargateServiceName = "OrderDetails"
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "Order",
                ServiceName = "OrderDetails",
                TargateServiceName = "Orders"
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "Product",
                ServiceName = "OrderDetails",
                TargateServiceName = "Products"
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "Orders",
                ServiceName = "Customers",
                TargateServiceName = "Orders"
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "Customer",
                ServiceName = "Orders",
                TargateServiceName = "Customers"
            });

            await testSubject.AddServiceNavigation(new NavigationPropertyBinding()
            {
                EntityNavigation = "OrderDetails",
                ServiceName = "Orders",
                TargateServiceName = "OrderDetails"
            });

            var edm = await testSubject.GetEdmModel();

            // Assert.AreEqual(0, serializationErrors.Count(), "Error on serialization");
            var stringBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(stringBuilder))
            {
                edm.TryWriteSchema(xmlWriter, out var errors);
            }

            File.WriteAllText("../../../TestModel.xml", stringBuilder.ToString());
            edm.Validate(out var errorss);
            Assert.IsEmpty(errorss);
        }
    }
}