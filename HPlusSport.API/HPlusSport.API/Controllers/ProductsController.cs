using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPlusSport.API.Classes;
using HPlusSport.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{
    [ApiVersion("1.0")]
    //[Route("v{v:apiVersion}/products")] //[Route("api/[controller]")]
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            IQueryable<Product> products = _context.Products;

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null) // Filtering.
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value && p.Price <= queryParameters.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name)) // Searching.
            {
                products = products.Where(
                    p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy)) // Sorting.
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            products = products // Paging.
              .Skip(queryParameters.Size * (queryParameters.Page - 1))
              .Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = _context.Products.Where(x => x.Id == id);
            if (products == null)
            {
                return NotFound("Id Not Found");
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { Id = product.Id },
                product
                );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }
        [HttpPost] // Deleting Several Itams.
        [Route("Delete")]
        public async Task<IActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }
        [ApiVersion("2.0")]
        //[Route("v{v:apiVersion}/products")]
        [Route("products")]
        [ApiController]
        public class ProductsV2_0Controller : ControllerBase
        {
            private readonly ShopContext _context;

            public ProductsV2_0Controller(ShopContext context)
            {
                _context = context;

                _context.Database.EnsureCreated();
            }

            [HttpGet]
            public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
            {
                IQueryable<Product> products = _context.Products.Where(p => p.IsAvailable == true);

                if (queryParameters.MinPrice != null &&
                    queryParameters.MaxPrice != null)
                {
                    products = products.Where(
                        p => p.Price >= queryParameters.MinPrice.Value &&
                             p.Price <= queryParameters.MaxPrice.Value);
                }

                if (!string.IsNullOrEmpty(queryParameters.Sku))
                {
                    products = products.Where(p => p.Sku == queryParameters.Sku);
                }

                if (!string.IsNullOrEmpty(queryParameters.Name)) // Searching.
                {
                    products = products.Where(
                        p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(queryParameters.Sku))
                {
                    products = products.Where(p => p.Sku == queryParameters.Sku);
                }

                if (!string.IsNullOrEmpty(queryParameters.Name))
                {
                    products = products.Where(
                        p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(queryParameters.SortBy))
                {
                    if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                    {
                        products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }

                products = products
                    .Skip(queryParameters.Size * (queryParameters.Page - 1))
                    .Take(queryParameters.Size);

                return Ok(await products.ToArrayAsync());
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }

            [HttpPost]
            public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetProduct",
                    new { id = product.Id },
                    product
                );
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }

                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Products.Find(id) == null)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult<Product>> DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return product;
            }

            [HttpPost]
            [Route("Delete")]
            public async Task<IActionResult> DeleteMultiple([FromQuery] int[] ids)
            {
                var products = new List<Product>();
                foreach (var id in ids)
                {
                    var product = await _context.Products.FindAsync(id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    products.Add(product);
                }

                _context.Products.RemoveRange(products);
                await _context.SaveChangesAsync();

                return Ok(products);
            }

        }

    }
}

