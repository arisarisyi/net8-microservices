using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
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
    }
}
