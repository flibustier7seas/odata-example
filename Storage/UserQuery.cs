using System.Collections.Generic;
using System.Linq;
using ODataExample.Model;

namespace ODataExample.Storage
{
    public class UserQuery
    {
        private readonly List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "User_1",
                Orders = new[]
                {
                    new Order
                    {
                        Id = 11,
                        Name = "Order_11",
                        OrderPositions = new[]
                        {
                            new OrderPosition
                            {
                                Id = 111,
                                Name = "OrderPosition_111",
                                Products = new[]
                                {
                                    new Product
                                    {
                                        Id = 1111,
                                        Name = "Product_1111",
                                        Parameters = new[]
                                        {
                                            new ProductParameters
                                            {
                                                Id = 11111,
                                                Name = "Parameter_11111",
                                                Value = "Value_11111"
                                            },
                                            new ProductParameters
                                            {
                                                Id = 11112,
                                                Name = "Parameter_11112",
                                                Value = "Value_11112"
                                            }
                                        }
                                    },
                                    new Product
                                    {
                                        Id = 1112,
                                        Name = "Product_1112",
                                        Parameters = new[]
                                        {
                                            new ProductParameters
                                            {
                                                Id = 11121,
                                                Name = "Parameter_11121",
                                                Value = "Value_11121"
                                            },
                                            new ProductParameters
                                            {
                                                Id = 11122,
                                                Name = "Parameter_11122",
                                                Value = "Value_11122"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Order
                    {
                        Id = 12,
                        Name= "Order_12"
                    },
                    new Order
                    {
                        Id = 13,
                        Name= "Order_13"
                    }
                }
            },
            new User
            {
                Id = 2,
                Name = "User_2",
                Orders = new[]
                {
                    new Order
                    {
                        Id = 21,
                        Name = "Order_21",
                        OrderPositions = new[]
                        {
                            new OrderPosition
                            {
                                Id = 211,
                                Name = "OrderPosition_211",
                                Products = new[]
                                {
                                    new Product
                                    {
                                        Id = 2111,
                                        Name = "Product_2111",
                                        Parameters = new[]
                                        {
                                            new ProductParameters
                                            {
                                                Id = 21111,
                                                Name = "Parameter_21111",
                                                Value = "Value_21111"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        public IQueryable<User> GetUsers()
        {
            return users.AsQueryable();
        }
    }
}
