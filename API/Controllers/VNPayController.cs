using API.DTOs.Requests.Users;
using API.DTOs.VNPay;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Controllers;

[Route("/v1/auction/VNPay")]
public class VNPayController : BaseController
{
    //private readonly double _exchangeRate;
    private readonly IConfiguration _configuration;
    private readonly IRepositoryBase<Transaction> _transactionRepository;
    private readonly IPaymentService _paymentService;
    private readonly IRepositoryBase<TransactionType> _tranTypeRepository;
    private readonly IUserAuctionService _userAuctionService;


    public VNPayController(IConfiguration configuration, IRepositoryBase<Transaction> transactionRepository,
        IPaymentService paymentService, IRepositoryBase<TransactionType> tranTypeRepository, IUserAuctionService userAuctionService)
    {
        _configuration = configuration;
        _transactionRepository = transactionRepository;
        _paymentService = paymentService;
        _tranTypeRepository = tranTypeRepository;
        _userAuctionService = userAuctionService;
        //_exchangeRate = double.Parse(configuration["SystemConfiguration:ExchangeRate"]);
    }


    [HttpGet("Payment-For-Joining")]
    [ProducesResponseType(typeof(BaseResponse<ResponsePaymentUrlModel>), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetJoining([FromQuery] BusinessPayment businessPayment)
    {
        string url = _configuration["VnPay:Url"]!;
        string returnUrl = _configuration["VnPay:ReturnPathJoining"]!;
        string tmnCode = _configuration["VnPay:TmnCode"]!;
        string hashSecret = _configuration["VnPay:HashSecret"]!;
        VnPayLibrary pay = new();

        pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
        pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
        pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
        pay.AddRequestData("vnp_Amount", businessPayment.Amount + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
        pay.AddRequestData("vnp_CreateDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).ToString("yyyyMMddHHmmss")
        ); //ngày thanh toán theo định dạng yyyyMMddHHmmss
        pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
        pay.AddRequestData("vnp_IpAddr", "0.0.0.0"); //Địa chỉ IP của khách hàng thực hiện giao dịch
        pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
        pay.AddRequestData("vnp_OrderInfo", $"{businessPayment.UserId},{businessPayment.AuctionId}"); //Thông tin mô tả nội dung thanh toán
        pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
        pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
        pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
        pay.AddRequestData("vnp_ExpireDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).AddHours(1).ToString("yyyyMMddHHmmss")
        ); //Thời gian kết thúc thanh toán
        var paymentUrl = pay.CreateRequestUrl(url, hashSecret);
        return Ok(paymentUrl);
    }


    [HttpGet("Confirm-Joining")]
    [ProducesResponseType(typeof(BaseResponse<ResponseStatusPaymentModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmJoinging()
    {
        //string returnUrl = _configuration["VnPay:ReturnPath"];
        double amount = 0;
        string status = "FAILED";
        if (Request.Query.Count > 0)
        {
            string vnp_HashSecret = _configuration["VnPay:HashSecret"]!;
            var vnpayData = Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                // Lấy dữ liệu từ query string
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            //long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            amount = vnp_Amount;
            //long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            //String vnp_SecureHash = Request.Query["vnp_SecureHash"];
            //bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            string vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var splitVnpOrderInfo = vnp_OrderInfo.Split(",");
            int userId = int.Parse(splitVnpOrderInfo[0]);
            int auctionId = int.Parse(splitVnpOrderInfo[1]);
            

            // Kiểm tra dữ liệu trả về từ VNPAY
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                // Thanh toán thành công
                status = "SUCCESS";
                await _paymentService.PayJoiningFeeAuction(userId, auctionId);
                await  _userAuctionService.JoinAuction(userId, auctionId);
            }

            // Đoạn này đã bị loại bỏ vì không còn sử dụng dịch vụ ví
        }

        return Redirect($"amount={amount}&status={status}");

    }


    [HttpGet("Payment-For-Deposit")]
    [ProducesResponseType(typeof(BaseResponse<ResponsePaymentUrlModel>), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetDeposit([FromQuery] BusinessPayment businessPayment)
    {
        string url = _configuration["VnPay:Url"]!;
        string returnUrl = _configuration["VnPay:ReturnPathDeposit"]!;
        string tmnCode = _configuration["VnPay:TmnCode"]!;
        string hashSecret = _configuration["VnPay:HashSecret"]!;
        VnPayLibrary pay = new();

        pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
        pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
        pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
        pay.AddRequestData("vnp_Amount", businessPayment.Amount + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
        pay.AddRequestData("vnp_CreateDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).ToString("yyyyMMddHHmmss")
        ); //ngày thanh toán theo định dạng yyyyMMddHHmmss
        pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
        pay.AddRequestData("vnp_IpAddr", "0.0.0.0"); //Địa chỉ IP của khách hàng thực hiện giao dịch
        pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
        pay.AddRequestData("vnp_OrderInfo", $"{businessPayment.UserId},{businessPayment.AuctionId}"); //Thông tin mô tả nội dung thanh toán
        pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
        pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
        pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
        pay.AddRequestData("vnp_ExpireDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).AddHours(1).ToString("yyyyMMddHHmmss")
        ); //Thời gian kết thúc thanh toán
        var paymentUrl = pay.CreateRequestUrl(url, hashSecret);
        return Ok(paymentUrl);
    }


    [HttpGet("Confirm-Deposit")]
    [ProducesResponseType(typeof(BaseResponse<ResponseStatusPaymentModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmDeposit()
    {
        //string returnUrl = _configuration["VnPay:ReturnPath"];
        double amount = 0;
        string status = "FAILED";
        if (Request.Query.Count > 0)
        {
            string vnp_HashSecret = _configuration["VnPay:HashSecret"]!;
            var vnpayData = Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                // Lấy dữ liệu từ query string
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            //long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            amount = vnp_Amount;
            //long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            //String vnp_SecureHash = Request.Query["vnp_SecureHash"];
            //bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            string vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var splitVnpOrderInfo = vnp_OrderInfo.Split(",");
            int userId = int.Parse(splitVnpOrderInfo[0]);
            int auctionId = int.Parse(splitVnpOrderInfo[1]);
            // Kiểm tra dữ liệu trả về từ VNPAY
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                // Thanh toán thành công
                status = "SUCCESS";
                await _paymentService.PayDepositFeeAuction(userId, auctionId);

            }

            // Đoạn này đã bị loại bỏ vì không còn sử dụng dịch vụ ví
        }

        return Redirect($"amount={amount}&status={status}");

    }

    [HttpGet("Payment-For-Back-Deposit")]
    [ProducesResponseType(typeof(BaseResponse<ResponsePaymentUrlModel>), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetPayBackDeposit([FromQuery] BusinessPayment businessPayment)
    {
        string url = _configuration["VnPay:Url"]!;
        string returnUrl = _configuration["VnPay:ReturnPathBackDeposit"]!;
        string tmnCode = _configuration["VnPay:TmnCode"]!;
        string hashSecret = _configuration["VnPay:HashSecret"]!;
        VnPayLibrary pay = new();

        pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
        pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
        pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
        pay.AddRequestData("vnp_Amount", businessPayment.Amount + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
        pay.AddRequestData("vnp_CreateDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).ToString("yyyyMMddHHmmss")
        ); //ngày thanh toán theo định dạng yyyyMMddHHmmss
        pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
        pay.AddRequestData("vnp_IpAddr", "0.0.0.0"); //Địa chỉ IP của khách hàng thực hiện giao dịch
        pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
        pay.AddRequestData("vnp_OrderInfo", $"{businessPayment.UserId},{businessPayment.AuctionId}"); //Thông tin mô tả nội dung thanh toán
        pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
        pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
        pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
        pay.AddRequestData("vnp_ExpireDate", TimeZoneInfo
            .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).AddHours(1).ToString("yyyyMMddHHmmss")
        ); //Thời gian kết thúc thanh toán
        var paymentUrl = pay.CreateRequestUrl(url, hashSecret);
        return Ok(paymentUrl);
    }


    [HttpGet("Confirm-Back-Deposit")]
    [ProducesResponseType(typeof(BaseResponse<ResponseStatusPaymentModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmBackDeposit()
    {
        //string returnUrl = _configuration["VnPay:ReturnPath"];
        double amount = 0;
        string status = "FAILED";
        if (Request.Query.Count > 0)
        {
            string vnp_HashSecret = _configuration["VnPay:HashSecret"]!;
            var vnpayData = Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                // Lấy dữ liệu từ query string
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            //long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            amount = vnp_Amount;
            //long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            //String vnp_SecureHash = Request.Query["vnp_SecureHash"];
            //bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            string vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var splitVnpOrderInfo = vnp_OrderInfo.Split(",");
            int userId = int.Parse(splitVnpOrderInfo[0]);
            int auctionId = int.Parse(splitVnpOrderInfo[1]);
            // Kiểm tra dữ liệu trả về từ VNPAY
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                // Thanh toán thành công
                status = "SUCCESS";
                await _paymentService.PayBackDepositFeeAuction(userId, auctionId);

            }

            // Đoạn này đã bị loại bỏ vì không còn sử dụng dịch vụ ví
        }

        return Redirect($"amount={amount}&status={status}");

    }


    public class ResponsePaymentUrlModel
    {public string? Url { get; set; }
    }

    public class ResponseStatusPaymentModel
    {
        public string? Status { get; set; }
        public double? Amount { get; set; }
    }
}