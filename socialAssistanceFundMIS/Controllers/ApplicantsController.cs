using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialAssistanceFundMIS.Common;
using socialAssistanceFundMIS.DTOs;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Services.Applicants;

namespace socialAssistanceFundMIS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicantsController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private readonly ILogger<ApplicantsController> _logger;

        public ApplicantsController(IApplicantService applicantService, ILogger<ApplicantsController> logger)
        {
            _applicantService = applicantService;
            _logger = logger;
        }

        /// <summary>
        /// Get all applicants with pagination
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "RequireOfficerRole")]
        public async Task<ActionResult<PaginatedResult<ApplicantDTO>>> GetApplicants(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 20;

                var result = await _applicantService.SearchApplicantsAsync(searchTerm ?? "", page, pageSize);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                // Convert to DTOs and create paginated result
                var applicants = result.Value;
                var totalCount = applicants.Count; // In a real implementation, you'd get total count from service

                var paginatedResult = new PaginatedResult<ApplicantDTO>
                {
                    Data = applicants.Select(a => new ApplicantDTO
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        MiddleName = a.MiddleName,
                        LastName = a.LastName,
                        FullName = a.FullName,
                        Email = a.Email,
                        DateOfBirth = a.DateOfBirth,
                        Age = a.Age,
                        SexId = a.SexId,
                        SexName = a.SexName,
                        MaritalStatusId = a.MaritalStatusId,
                        MaritalStatusName = a.MaritalStatusName,
                        VillageId = a.VillageId,
                        VillageName = a.VillageName,
                        IdentityCardNumber = a.IdentityCardNumber,
                        PostalAddress = a.PostalAddress,
                        PhysicalAddress = a.PhysicalAddress,
                        PhoneNumbers = a.PhoneNumbers.Select(pn => new ApplicantPhoneNumberDTO
                        {
                            Id = pn.Id,
                            PhoneNumber = pn.PhoneNumber,
                            PhoneNumberTypeId = pn.PhoneNumberTypeId,
                            PhoneNumberTypeName = pn.PhoneNumberType?.Name
                        }).ToList()
                    }).ToList(),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applicants");
                return StatusCode(500, "An error occurred while retrieving applicants");
            }
        }

        /// <summary>
        /// Get applicant by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireOfficerRole")]
        public async Task<ActionResult<ApplicantDTO>> GetApplicant(int id)
        {
            try
            {
                var result = await _applicantService.GetApplicantByIdAsync(id);
                
                if (!result.IsSuccess)
                    return NotFound(result.Error);

                var applicant = result.Value;
                var dto = new ApplicantDTO
                {
                    Id = applicant.Id,
                    FirstName = applicant.FirstName,
                    MiddleName = applicant.MiddleName,
                    LastName = applicant.LastName,
                    FullName = applicant.FullName,
                    Email = applicant.Email,
                    DateOfBirth = applicant.Dob,
                    Age = applicant.Age,
                    SexId = applicant.SexId,
                    SexName = applicant.SexName,
                    MaritalStatusId = applicant.MaritalStatusId,
                    MaritalStatusName = applicant.MaritalStatusName,
                    VillageId = applicant.VillageId,
                    VillageName = applicant.VillageName,
                    IdentityCardNumber = applicant.IdentityCardNumber,
                    PostalAddress = applicant.PostalAddress,
                    PhysicalAddress = applicant.PhysicalAddress,
                    PhoneNumbers = applicant.PhoneNumbers.Select(pn => new ApplicantPhoneNumberDTO
                    {
                        Id = pn.Id,
                        PhoneNumber = pn.PhoneNumber,
                        PhoneNumberTypeId = pn.PhoneNumberTypeId,
                        PhoneNumberTypeName = pn.PhoneNumberType?.Name
                    }).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applicant with ID {ApplicantId}", id);
                return StatusCode(500, "An error occurred while retrieving the applicant");
            }
        }

        /// <summary>
        /// Create a new applicant
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "RequireOfficerRole")]
        public async Task<ActionResult<ApplicantDTO>> CreateApplicant([FromBody] CreateApplicantRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var applicant = new Applicant
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    SexId = request.SexId,
                    MaritalStatusId = request.MaritalStatusId,
                    VillageId = request.VillageId,
                    IdentityCardNumber = request.IdentityCardNumber,
                    PostalAddress = request.PostalAddress,
                    PhysicalAddress = request.PhysicalAddress
                };

                var result = await _applicantService.CreateApplicantAsync(applicant, request.PhoneNumbers);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                var createdApplicant = result.Value;
                var dto = new ApplicantDTO
                {
                    Id = createdApplicant.Id,
                    FirstName = createdApplicant.FirstName,
                    MiddleName = createdApplicant.MiddleName,
                    LastName = createdApplicant.LastName,
                    FullName = createdApplicant.FullName,
                    Email = createdApplicant.Email,
                    DateOfBirth = createdApplicant.Dob,
                    Age = createdApplicant.Age,
                    SexId = createdApplicant.SexId,
                    SexName = createdApplicant.SexName,
                    MaritalStatusId = createdApplicant.MaritalStatusId,
                    MaritalStatusName = createdApplicant.MaritalStatusName,
                    VillageId = createdApplicant.VillageId,
                    VillageName = createdApplicant.VillageName,
                    IdentityCardNumber = createdApplicant.IdentityCardNumber,
                    PostalAddress = createdApplicant.PostalAddress,
                    PhysicalAddress = createdApplicant.PhysicalAddress,
                    PhoneNumbers = createdApplicant.PhoneNumbers.Select(pn => new ApplicantPhoneNumberDTO
                    {
                        Id = pn.Id,
                        PhoneNumber = pn.PhoneNumber,
                        PhoneNumberTypeId = pn.PhoneNumberTypeId,
                        PhoneNumberTypeName = pn.PhoneNumberType?.Name
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetApplicant), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating applicant");
                return StatusCode(500, "An error occurred while creating the applicant");
            }
        }

        /// <summary>
        /// Update an existing applicant
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireOfficerRole")]
        public async Task<ActionResult<ApplicantDTO>> UpdateApplicant(int id, [FromBody] UpdateApplicantRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var applicant = new Applicant
                {
                    Id = id,
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    SexId = request.SexId,
                    MaritalStatusId = request.MaritalStatusId,
                    VillageId = request.VillageId,
                    IdentityCardNumber = request.IdentityCardNumber,
                    PostalAddress = request.PostalAddress,
                    PhysicalAddress = request.PhysicalAddress
                };

                var result = await _applicantService.UpdateApplicantAsync(id, applicant, request.PhoneNumbers);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                var updatedApplicant = result.Value;
                var dto = new ApplicantDTO
                {
                    Id = updatedApplicant.Id,
                    FirstName = updatedApplicant.FirstName,
                    MiddleName = updatedApplicant.MiddleName,
                    LastName = updatedApplicant.LastName,
                    FullName = updatedApplicant.FullName,
                    Email = updatedApplicant.Email,
                    DateOfBirth = updatedApplicant.Dob,
                    Age = updatedApplicant.Age,
                    SexId = updatedApplicant.SexId,
                    SexName = updatedApplicant.SexName,
                    MaritalStatusId = updatedApplicant.MaritalStatusId,
                    MaritalStatusName = updatedApplicant.MaritalStatusName,
                    VillageId = updatedApplicant.VillageId,
                    VillageName = updatedApplicant.VillageName,
                    IdentityCardNumber = updatedApplicant.IdentityCardNumber,
                    PostalAddress = updatedApplicant.PostalAddress,
                    PhysicalAddress = updatedApplicant.PhysicalAddress,
                    PhoneNumbers = updatedApplicant.PhoneNumbers.Select(pn => new ApplicantPhoneNumberDTO
                    {
                        Id = pn.Id,
                        PhoneNumber = pn.PhoneNumber,
                        PhoneNumberTypeId = pn.PhoneNumberTypeId,
                        PhoneNumberTypeName = pn.PhoneNumberType?.Name
                    }).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating applicant with ID {ApplicantId}", id);
                return StatusCode(500, "An error occurred while updating the applicant");
            }
        }

        /// <summary>
        /// Delete an applicant
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteApplicant(int id)
        {
            try
            {
                var result = await _applicantService.DeleteApplicantAsync(id);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting applicant with ID {ApplicantId}", id);
                return StatusCode(500, "An error occurred while deleting the applicant");
            }
        }

        /// <summary>
        /// Check if identity card number is unique
        /// </summary>
        [HttpGet("check-identity-card/{identityCardNumber}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CheckIdentityCardNumber(string identityCardNumber, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _applicantService.IsIdentityCardNumberUniqueAsync(identityCardNumber, excludeId);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking identity card number uniqueness");
                return StatusCode(500, "An error occurred while checking identity card number uniqueness");
            }
        }

        /// <summary>
        /// Check if email is unique
        /// </summary>
        [HttpGet("check-email/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CheckEmail(string email, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _applicantService.IsEmailUniqueAsync(email, excludeId);
                
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email uniqueness");
                return StatusCode(500, "An error occurred while checking email uniqueness");
            }
        }
    }

    public class CreateApplicantRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int SexId { get; set; }
        public int MaritalStatusId { get; set; }
        public int? VillageId { get; set; }
        public string IdentityCardNumber { get; set; } = string.Empty;
        public string? PostalAddress { get; set; }
        public string PhysicalAddress { get; set; } = string.Empty;
        public List<(string phoneNumber, int phoneNumberTypeId)> PhoneNumbers { get; set; } = new();
    }

    public class UpdateApplicantRequest : CreateApplicantRequest
    {
        public int Id { get; set; }
    }

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}

