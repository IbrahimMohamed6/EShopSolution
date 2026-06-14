using Discount.Api.Data.Contexts;
using Discount.Api.Models;
using Discount.Grpc;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Api.Services
{
    public class DiscountService(
        ApplicationDbContext _dbContext,
        ILogger<DiscountService> _logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(
     GetDiscountRequest request,
     ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, "ProductName is required"));
            }

            var productName = request.ProductName.Trim();

            var coupon = await _dbContext.Coupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.ProductName.ToLower() == productName.ToLower());

            if (coupon is null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "Coupon not found for this product"));
            }

            return new CouponModel
            {
                Id = coupon.ID,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };
        }

        public override async Task<CouponModel> CreateDiscount(
            CreateDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = new Coupon
            {
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };

            _dbContext.Coupons.Add(coupon);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Discount created for ProductName: {ProductName}",
                coupon.ProductName);

            return new CouponModel
            {
                Id = coupon.ID,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };
        }

        public override async Task<CouponModel> UpdateDiscount(
            UpdateDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = await _dbContext.Coupons
                .FirstOrDefaultAsync(x => x.ID == request.Coupon.Id);

            if (coupon == null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "Coupon not found"));
            }

            coupon.ProductName = request.Coupon.ProductName;
            coupon.Description = request.Coupon.Description;
            coupon.Amount = request.Coupon.Amount;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Discount updated for ProductName: {ProductName}",
                coupon.ProductName);

            return new CouponModel
            {
                Id = coupon.ID,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(
            DeleteDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = await _dbContext.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
            {
                return new DeleteDiscountResponse
                {
                    Success = false
                };
            }

            _dbContext.Coupons.Remove(coupon);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Discount deleted for ProductName: {ProductName}",
                request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = true
            };
        }
    }
}