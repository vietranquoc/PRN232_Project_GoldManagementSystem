using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using BusinessObjects.DTOs;

namespace Services.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, ITransactionRepository transactionRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Cart> GetOrCreateActiveCartAsync()
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync();
            if (cart == null)
            {
                cart = new Cart 
                { 
                    UserId = _cartRepository.GetCurrentUserId(),
                    CreatedDate = DateTime.UtcNow 
                };
                _cartRepository.Insert(cart);
                await _cartRepository.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _cartItemRepository.GetItemsByCartIdAsync(cartId);
        }

        public async Task AddOrUpdateCartItemAsync(int cartId, int productId, int quantity, decimal price)
        {
            // Kiểm tra stock của sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Sản phẩm không tồn tại");
            
            if (product.Quantity <= 0)
                throw new InvalidOperationException("Sản phẩm đã hết hàng");
            
            var items = await _cartItemRepository.GetItemsByCartIdAsync(cartId);
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            
            int newQuantity;
            if (item != null)
            {
                newQuantity = item.Quantity + quantity;
            }
            else
            {
                newQuantity = quantity;
            }
            
            // Kiểm tra xem tổng số lượng có vượt quá stock không
            if (newQuantity > product.Quantity)
                throw new InvalidOperationException($"Chỉ còn {product.Quantity} sản phẩm trong kho");
            
            if (item != null)
            {
                item.Quantity = newQuantity;
                _cartItemRepository.Update(item);
            }
            else
            {
                var newItem = new CartItem 
                { 
                    CartId = cartId, 
                    ProductId = productId, 
                    Quantity = quantity, 
                    Price = price 
                };
                _cartItemRepository.Insert(newItem);
            }
            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var item = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (item != null)
            {
                _cartItemRepository.Remove(item);
                await _cartItemRepository.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var items = await _cartItemRepository.GetItemsByCartIdAsync(cartId);
            foreach (var item in items)
            {
                _cartItemRepository.Remove(item);
            }
            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task<Transaction> CheckoutAsync(CartCheckoutDTO dto)
        {
            if (dto.DeliveryMethod == "pickup")
            {
                dto.Province = "Nhận tại cửa hàng";
                dto.District = "Nhận tại cửa hàng";
                dto.Address = "Nhận tại cửa hàng";
            }
            
            if (string.IsNullOrWhiteSpace(dto.Address))
                dto.Address = "Không xác định";
            if (string.IsNullOrWhiteSpace(dto.Province))
                dto.Province = "Không xác định";
            if (string.IsNullOrWhiteSpace(dto.District))
                dto.District = "Không xác định";

            var cart = await _cartRepository.GetActiveCartByUserIdAsync();
            if (cart == null) return null;
            var items = await _cartItemRepository.GetItemsByCartIdAsync(cart.Id);
            if (!items.Any()) return null;

            decimal shippingFee = 0;
            if (dto.ShippingMethod == "shipping2")
                shippingFee = 100000;

            var transaction = new Transaction
            {
                UserId = _cartRepository.GetCurrentUserId(),
                UnitPrice = 0, // hoặc logic phù hợp
                TotalAmount = 0,
                TransactionDate = DateTime.UtcNow,
                Status = "PENDING", // Thay đổi status thành PENDING
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                ReceiverEmail = dto.ReceiverEmail,
                Province = dto.Province,
                District = dto.District,
                Address = dto.Address,
                Note = dto.Note,
                TransactionDetails = new List<TransactionDetail>()
            };

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                    throw new InvalidOperationException($"Sản phẩm {item.ProductId} không đủ hàng");
                product.Quantity -= item.Quantity;
                _productRepository.Update(product);

                var detail = new TransactionDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalAmount = item.Price * item.Quantity
                };
                transaction.TotalAmount += detail.TotalAmount;
                transaction.TransactionDetails.Add(detail);
            }

            transaction.TotalAmount += shippingFee; // Cộng phí ship vào tổng tiền

            _transactionRepository.Insert(transaction);
            await _transactionRepository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
            await ClearCartAsync(cart.Id);
            return transaction;
        }
    }
} 