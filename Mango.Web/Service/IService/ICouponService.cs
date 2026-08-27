using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface ICouponService
{
    Task<ResponseDto?> GetCouponAsync(string couponCode);
    Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
    Task<ResponseDto?> DeleteCouponAsync(string couponCode);
    Task<ResponseDto?> GetAllCouponsAsync();
    Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
    Task<ResponseDto?> GetCouponByIdAsync(int id);
}