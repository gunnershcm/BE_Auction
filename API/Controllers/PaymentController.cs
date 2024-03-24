using API.DTOs.Requests.Auctions;
using API.DTOs.Requests.Posts;
using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.Auctions;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.TransferForms;
using API.DTOs.Responses.UserAuctions;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Controllers
{
    [Route("/v1/auction/payment")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepositoryBase<Transaction> _tranRepository;

        public PaymentController(IPaymentService paymentService, IRepositoryBase<Transaction> tranRepository)
        {
            _paymentService = paymentService;
            _tranRepository = tranRepository;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetPaymentResponse>), 200)]
        public async Task<IActionResult> GetAllPayment()
        {
            var result = await _paymentService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPaymentResponse>), 200)]
        public async Task<IActionResult> GetPayments(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _paymentService.Get();
                var pagedResponse = result.AsQueryable().GetPagedData(page, pageSize, filter, sort);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetPaymentResponse>), 200)]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var result = await _paymentService.GetById(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<GetUserPaymentResponse>), 200)]
        public async Task<IActionResult> GetPaymentAvailable()
        {
            try
            {
                var result = await _paymentService.GetPaymentAvailable(CurrentUserID);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPost("member/pay-joining-auction")]
        public async Task<IActionResult> PayJoiningFeeAuction(int auctionId)
        {
            try
            {
                await _paymentService.PayJoiningFeeAuction(CurrentUserID, auctionId);
                return Ok("Payment successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Payment is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPost("member/pay-deposit-auction")]
        public async Task<IActionResult> PayDepositFeeAuction(int auctionId)
        {
            try
            {
                await _paymentService.PayDepositFeeAuction(CurrentUserID, auctionId);
                return Ok("Payment successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Payment is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost("member/payback-deposit-auction")]
        public async Task<IActionResult> PayBackDepositFeeAuction(int auctionId)
        {
            try
            {
                await _paymentService.PayDepositFeeAuction(CurrentUserID, auctionId);
                return Ok("Payment successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Payment is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpDelete("admin/{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var form = await _tranRepository.FirstOrDefaultAsync(x => x.Id == id);
                await _paymentService.Remove(id);
                return Ok("Remove Transaction Successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
