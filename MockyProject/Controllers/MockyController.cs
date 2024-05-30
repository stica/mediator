using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mocky.Features.Products;

namespace MockyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MockyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get products from mocked database
        /// </summary>
        /// <param name="maxPrice">Maximum price for filter products</param>
        /// <param name="minPrice">Minimum price for filter products</param>
        /// <param name="size">Filter products by selected size</param>
        /// <param name="highlight">Filter products by selected colors</param>
        /// <returns>List of filtered products</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetProducts(decimal maxPrice, decimal minPrice, string size, string highlight)
        {
            try
            {
                var result = await _mediator.Send(new GetProducts.Query
                {
                    HighlightColors = string.IsNullOrEmpty(highlight) ? new List<string>() : highlight.Split(',').ToList(),
                    MaxPrice = maxPrice,
                    MinPrice = minPrice,
                    Size = size
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
