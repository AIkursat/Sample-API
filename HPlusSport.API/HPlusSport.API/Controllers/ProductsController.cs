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
    [Route("[controller]")] //[Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllProducts([FromQuery] QueryParameters queryParameters)
        {
            IQueryable<Product> products = _context.Products;

          /*  products = products
               .Skip()
               .Take(); buraları incele
                */ 

            return Ok(await products.ToArrayAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = _context.Products.Where(x => x.Id == id);
            if(products == null)
            {
                return NotFound("Id Not Found");
            }
            return Ok(products);
        }

    }

    }

