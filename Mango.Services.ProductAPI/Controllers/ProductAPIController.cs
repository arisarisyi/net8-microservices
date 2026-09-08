using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        
        public ProductAPIController(AppDbContext context, IMapper mapper)
            {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
            }
        
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> products = _context.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
                _response.Message = "Success";
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
                Console.WriteLine(e);
                throw;
            }

            return _response;
        }
        
        [HttpGet("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product product = _context.Products.FirstOrDefault(x => x.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.Message = "Success";
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
                Console.WriteLine(e);
                throw;
            }

            return _response;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Add(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.Message = "Success";
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
                Console.WriteLine(e);
                throw;
            }

            return _response;
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Update(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<Product>(product);
                _response.Message = "Success";
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
                Console.WriteLine(e);
                throw;
            }

            return _response;
        }
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _context.Products.FirstOrDefault(x => x.ProductId == id);
                _context.Products.Remove(product);
                _context.SaveChanges();
                _response.Message = "Success";
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
                Console.WriteLine(e);
                throw;
            }

            return _response;
        }
    }
}
