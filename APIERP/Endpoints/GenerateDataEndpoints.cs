using APIERP.Entidades;
using APIERP.Repositorios;
using APIERP.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace APIERP.Endpoints
{
    public static class GenerateDataEndpoints
    {
        public static RouteGroupBuilder AddDataToDB(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IRepoCategories repoCategories, 
                IRepoProducts repoProducts, IRepoOrders repoOrders,
                IRepoOrderDetails repoOrderDetails, IRepoStores repoStores,
                IRepoRainchecks repoRainchecks, IMapper mapper) =>
            {
                await GenerateStores(repoStores);
                await GenerateCategories(repoCategories);
                await GenerateProducts(repoProducts);
                await GenerateOrders(repoOrders);
                await GenerateOrderDetails(repoOrderDetails);
                await GenerateRainChecks(repoRainchecks);

                return TypedResults.Ok();
            });

            return group;
        }

        static async Task GenerateStores(IRepoStores repoStores)
        {
            for (int i = 1; i <= 20; i++)
            {
                await repoStores.Add(new Store
                {
                    Name = $"Tienda {i}"
                });
            }
        }

        static async Task GenerateCategories(IRepoCategories repoCategories)
        {
            for (int i = 1; i <= 20; i++)
            {
                await repoCategories.Add(new Category
                {
                    Name = $"Categoria {i}"
                });
            }
        }
        
        static async Task GenerateProducts(IRepoProducts repoProducts)
        {
            Random random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                await repoProducts.Add(new Product
                {
                  SkuNumber = $"SKU{i}",
                  CategoryId = random.Next(3, 21),
                  RecommendationId = 0,
                  Title = $"Producto {i}",
                  Price = 0,
                  SalePrice = 0,
                  Created = DateTime.Now,
                  ProductArtUrl = "string",
                  Description = $"Descripción {i}",
                  ProductDetails = "string",
                  Inventory = 0,
                  LeadTime = 0
                });
            }
        }

        static async Task GenerateOrders(IRepoOrders repoOrders)
        {
            Random random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                await repoOrders.Add(new Order
                {
                    OrderDate = DateTime.Now,
                    Username = $"user{i}",
                    Name = $"Nombre {i}",
                    Address = $"direccion{i}",
                    City = "Málaga",
                    State = "Málaga",
                    PostalCode = "29000",
                    Country = "pais",
                    Phone = $"{i}{i}{i}{i}{i}{i}{i}{i}{i}",
                    Email = $"user{i}@localhost.com",
                    Total = 0
                });
            }
        }

        static async Task GenerateOrderDetails(IRepoOrderDetails repoOrderDetails)
        {
            Random random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                await repoOrderDetails.Add(new OrderDetail
                {
                    OrderId = random.Next(1, 21),
                    ProductId = random.Next(2, 11),
                    Count = random.Next(1, 3),
                    UnitPrice = GetRandomDecimal(3, 120, 2)
                });
            }
        }

        static async Task GenerateRainChecks(IRepoRainchecks repoRainChecks)
        {
            Random random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                await repoRainChecks.Add(new Raincheck
                {
                    Name = $"RainName {i}",
                    Count = random.Next(1, 3),
                    SalePrice = (double)GetRandomDecimal(3, 120, 2),
                    StoreId = random.Next(2, 11),
                    ProductId = random.Next(2, 11)
                });
            }
        }

        public static decimal GetRandomDecimal(int minValue, int maxValue, int decimalPlaces)
        {
            Random random = new Random();
            int integerPart = random.Next(minValue, maxValue);
            decimal fractionalPart = (decimal)random.NextDouble();

            // Combina las partes entera y fraccionaria
            decimal result = integerPart + fractionalPart;

            // Redondea a los decimales especificados
            return Math.Round(result, decimalPlaces);
        }
    }
}
