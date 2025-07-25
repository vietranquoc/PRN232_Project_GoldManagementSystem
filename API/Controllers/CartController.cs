using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using BusinessObjects.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetOrCreateActiveCartAsync();
            var items = await _cartService.GetCartItemsAsync(cart.Id);

            var cartVm = new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedDate = cart.CreatedDate,
                Items = items.Select(i => new CartItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    ProductImage = i.Product?.ProductImages?.FirstOrDefault(img => img.IsMain)?.ImageUrl ?? string.Empty,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            return Ok(cartVm);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrUpdateCartItem(int productId, int quantity, decimal price)
        {
            try
            {
                var cart = await _cartService.GetOrCreateActiveCartAsync();
                await _cartService.AddOrUpdateCartItemAsync(cart.Id, productId, quantity, price);
                return Ok(new { message = "Đã thêm vào giỏ hàng thành công!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi thêm vào giỏ hàng!" });
            }
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            await _cartService.RemoveCartItemAsync(cartItemId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var cart = await _cartService.GetOrCreateActiveCartAsync();
            await _cartService.ClearCartAsync(cart.Id);
            return Ok();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CartCheckoutDTO dto)
        {
            var transaction = await _cartService.CheckoutAsync(dto);
            if (transaction == null)
                return BadRequest("Checkout thất bại hoặc giỏ hàng trống!");
            
            return Ok(new { 
                message = "Đã checkout thành công. Đơn hàng sẽ được xử lý!",
                orderId = transaction.Id.ToString(),
                totalAmount = transaction.TotalAmount
            });
        }
    }
} 