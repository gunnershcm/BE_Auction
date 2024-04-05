namespace API.DTOs.VNPay;

public class BusinessPayment
{
    public int UserId { get; set; }
    public double Amount { get; set; }
    public int AuctionId { get; set; }
    public int? FormId { get; set; }
}