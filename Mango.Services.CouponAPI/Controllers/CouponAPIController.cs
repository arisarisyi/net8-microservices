using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        
        public CouponAPIController(AppDbContext context, IMapper mapper)
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
                IEnumerable<Coupon> coupons = _context.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
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
                Coupon coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
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
        
        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _context.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
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
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(coupon);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
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
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
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
                Coupon coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == id);
                _context.Coupons.Remove(coupon);
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
