using FluentValidation;
using MediatR;
using Mocky.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mocky.Features.Products
{
    public class GetProducts
    {
        public class Handler : IRequestHandler<Query, List<Product>>
        {
            public Handler()
            {
            }

            public async Task<List<Product>> Handle(Query query, CancellationToken cancellationToken)
            {
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

                try
                {
                    var jsonData = new WebClient().DownloadString("http://www.mocky.io/v2/5e307edf3200005d00858b49");
                    var productsWrapper = JsonSerializer.Deserialize<ProductsWrapper>(jsonData);


                    if (query.MinPrice != null)
                    {
                        logger.Info("Filtered by min price");
                        productsWrapper.products = productsWrapper.products.Where(x => x.price >= query.MinPrice).ToList();
                    }

                    if (query.MaxPrice != null && query.MaxPrice != 0)
                    {
                        logger.Info("Filtered by max price");
                        productsWrapper.products = productsWrapper.products.Where(x => x.price <= query.MaxPrice).ToList();
                    }

                    if (query.Size != null)
                    {
                        logger.Info("Filtered by size");
                        productsWrapper.products = productsWrapper.products.Where(x => x.sizes.Contains(query.Size.ToLower())).ToList();
                    }

                    if (query.HighlightColors != null && query.HighlightColors.Count > 0)
                    {
                        logger.Info("Filtered by colors");

                        var productsHelperList = productsWrapper.products;
                        productsWrapper.products = new List<Product>();

                        query.HighlightColors.ForEach(color =>
                        {
                            productsWrapper.products.AddRange(productsHelperList.Where(x => x.description.ToLower().Contains(color.ToLower())));
                        });
                    }

                    return await Task.FromResult(productsWrapper.products);
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                    throw;
                }

            }
        }

        public class Query : IRequest<List<Product>>
        {
            public decimal? MaxPrice { get; set; }

            public decimal? MinPrice { get; set; }

            public string Size { get; set; }

            public List<string> HighlightColors { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                // this you should validate query
            }
        }
    }
}
