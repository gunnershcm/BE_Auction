using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.TransferForms;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;
using System.Net.Sockets;


namespace API.Controllers
{

    [Route("/v1/auction/TransferForm")]
    public class TransferFormController : BaseController
    {
        private readonly ITransferFormService _tranferFormService;
        private readonly IRepositoryBase<TransferForm> _transferFormRepository;

        public TransferFormController(ITransferFormService tranferFormService, IRepositoryBase<TransferForm> transferFormRepository)
        {
            _tranferFormService = tranferFormService;
            _transferFormRepository = transferFormRepository;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetAllTransferForm()
        {
            var result = await _tranferFormService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetTransferForms(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _tranferFormService.Get();
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
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetTransferFormById(int id)
        {
            try
            {
                var result = await _tranferFormService.GetById(id);
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
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetFormByUser(int userId)
        {
            try
            {
                var result = await _tranferFormService.GetByUser(userId);
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
        [HttpGet("user/available")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetFormByUserAvailable()
        {
            try
            {
                var result = await _tranferFormService.GetByUser(CurrentUserID);
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
        [HttpGet("approve/user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetFormApproveByUser(int userId)
        {
            try
            {
                var result = await _tranferFormService.GetFormApproveByUser(userId);
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
        [HttpGet("reject/user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> GetFormRejectByUser(int userId)
        {
            try
            {
                var result = await _tranferFormService.GetFormRejectByUser(userId);
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
        [HttpPost("member/new")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> CreateFormByMember([FromBody] CreateTransferFormRequest model)
        {
            try
            {
                var entity = await _tranferFormService.CreateFormByMember(CurrentUserID, model);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut("admin/{formId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> UpdateFormByAdmin(int formId, [FromBody] UpdateTransferFormRequest model)
        {
            try
            {
                var result = await _tranferFormService.UpdateByAdmin(formId, model);
                return Ok("Update Form Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Form is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpDelete("staff/{formId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> DeleteForm(int formId)
        {
            try
            {
                var form = await _transferFormRepository.FirstOrDefaultAsync(x => x.Id == formId);
                await _tranferFormService.Remove(formId);
                return Ok("Remove Form Successfully");
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

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPatch("approve")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> ApproveForm(int formId)
        {
            try
            {
                await _tranferFormService.Approve(formId); 
                return Ok("Approve Form Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Form is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPatch("reject")]
        [ProducesResponseType(typeof(IEnumerable<GetTransferFormResponse>), 200)]
        public async Task<IActionResult> RejectForm(int formId, [FromBody] UpdateRejectReasonForm model)
        {
            try
            {
                await _tranferFormService.Reject(formId, model);
                return Ok("Reject Form Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Form is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("modify-status")]
        public async Task<IActionResult> ModifyFormStatus(int formId, TranferFormStatus newStatus)
        {
            try
            {
                var updated = await _tranferFormService.ModifyFormStatus(formId, newStatus);
                return Ok("Status Updated Successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
