using BusinessObjects.DTOs;

namespace Services.Services.Interfaces
{
    public interface IVNPayService
    {
        VNPayResponseDTO CreatePaymentUrl(VNPayPaymentDTO paymentDto);
        VNPayResponseDTO ProcessPaymentReturn(Dictionary<string, string> queryParams);
    }
} 