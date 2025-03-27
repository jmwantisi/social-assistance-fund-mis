using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create an Application
        public async Task<ApplicationDTO> CreateApplicationAsync(ApplicationDTO applicationDTO)
        {
            // Validate incoming application data (if needed)
            if (applicationDTO == null)
                throw new ArgumentNullException(nameof(applicationDTO));

            var application = new Application
            {
                ApplicationDate = applicationDTO.ApplicationDate,
                ApplicantId = applicationDTO.ApplicantId,
                ProgramId = applicationDTO.ProgramId,
                StatusId = applicationDTO.StatusId,
                OfficialRecordId = applicationDTO.OfficialRecordId,
                DeclarationDate = applicationDTO.DeclarationDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Mapping to DTO
            return new ApplicationDTO
            {
                Id = application.Id,
                ApplicationDate = application.ApplicationDate,
                ApplicantId = application.ApplicantId,
                ApplicantFirstName = application.Applicant?.FirstName,
                ApplicantMiddleName = application.Applicant?.MiddleName,
                ApplicantLastName = application.Applicant?.LastName,
                ProgramId = application.ProgramId,
                ProgramName = application.Program?.Name,
                StatusId = application.StatusId,
                StatusName = application.Status?.Name,
                OfficialRecordId = application.OfficialRecordId,
                OfficalFirstName = application.OfficialRecord?.Officer?.FirstName,
                OfficialMiddleName = application.OfficialRecord?.Officer?.MiddleName,
                OfficialLastName = application.OfficialRecord?.Officer?.LastName,
                DeclarationDate = application.DeclarationDate,
                Removed = application.Removed,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            };
        }

        // Get an Application by ID
        public async Task<ApplicationDTO?> GetApplicationByIdAsync(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Applicant)  // Include related data like Applicant
                .Include(a => a.Program)    // Include related data like Program
                .Include(a => a.Status)     // Include related data like Status
                .Include(a => a.OfficialRecord) // Include related data like OfficialRecord
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);  // Ensure it's not marked as removed

            if (application == null)
                return null;

            // Mapping to DTO
            return new ApplicationDTO
            {
                Id = application.Id,
                ApplicationDate = application.ApplicationDate,
                ApplicantId = application.ApplicantId,
                ApplicantFirstName = application.Applicant?.FirstName,
                ApplicantMiddleName = application.Applicant?.MiddleName,
                ApplicantLastName = application.Applicant?.LastName,
                ProgramId = application.ProgramId,
                ProgramName = application.Program?.Name,
                StatusId = application.StatusId,
                StatusName = application.Status?.Name,
                OfficialRecordId = application.OfficialRecordId,
                OfficalFirstName = application.OfficialRecord?.Officer?.FirstName,
                OfficialMiddleName = application.OfficialRecord?.Officer?.MiddleName,
                OfficialLastName = application.OfficialRecord?.Officer?.LastName,
                DeclarationDate = application.DeclarationDate,
                Removed = application.Removed,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            };
        }

        // Get all Applications
        public async Task<List<ApplicationDTO>> GetAllApplicationsAsync()
        {
            var applications = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Program)
                .Include(a => a.Status)
                .Include(a => a.OfficialRecord)
                .Where(a => a.Removed == false)  // Ensure applications are not marked as removed
                .ToListAsync();

            return applications.Select(application => new ApplicationDTO
            {

                Id = application.Id,
                ApplicationDate = application.ApplicationDate,
                ApplicantId = application.ApplicantId,
                ApplicantFirstName = application.Applicant?.FirstName,
                ApplicantMiddleName = application.Applicant?.MiddleName,
                ApplicantLastName = application.Applicant?.LastName,
                ProgramId = application.ProgramId,
                ProgramName = application.Program?.Name,
                StatusId = application.StatusId,
                StatusName = application.Status?.Name,
                OfficialRecordId = application.OfficialRecordId,
                OfficalFirstName = application.OfficialRecord?.Officer?.FirstName,
                OfficialMiddleName = application.OfficialRecord?.Officer?.MiddleName,
                OfficialLastName = application.OfficialRecord?.Officer?.LastName,
                DeclarationDate = application.DeclarationDate,
                Removed = application.Removed,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            }).ToList();
        }

        // Update an Application
        public async Task<ApplicationDTO> UpdateApplicationAsync(int id, ApplicationDTO updatedApplicationDTO)
        {
            // Validate the incoming updated application data
            if (updatedApplicationDTO == null)
                throw new ArgumentNullException(nameof(updatedApplicationDTO));

            var existingApplication = await _context.Applications.FindAsync(id);

            if (existingApplication == null)
                throw new KeyNotFoundException("Application not found.");

            // Update properties
            existingApplication.ApplicationDate = updatedApplicationDTO.ApplicationDate;
            existingApplication.ApplicantId = updatedApplicationDTO.ApplicantId;
            existingApplication.ProgramId = updatedApplicationDTO.ProgramId;
            existingApplication.StatusId = updatedApplicationDTO.StatusId;
            existingApplication.OfficialRecordId = updatedApplicationDTO.OfficialRecordId;
            existingApplication.DeclarationDate = updatedApplicationDTO.DeclarationDate;
            existingApplication.UpdatedAt = DateTime.UtcNow;

            _context.Applications.Update(existingApplication);
            await _context.SaveChangesAsync();

            // Mapping to DTO
            return new ApplicationDTO
            {
                Id = existingApplication.Id,
                ApplicationDate = existingApplication.ApplicationDate,
                ApplicantId = existingApplication.ApplicantId,
                ApplicantFirstName = existingApplication.Applicant?.FirstName,
                ApplicantMiddleName = existingApplication.Applicant?.MiddleName,
                ApplicantLastName = existingApplication.Applicant?.LastName,
                ProgramId = existingApplication.ProgramId,
                ProgramName = existingApplication.Program?.Name,
                StatusId = existingApplication.StatusId,
                StatusName = existingApplication.Status?.Name,
                OfficialRecordId = existingApplication.OfficialRecordId,
                OfficalFirstName = existingApplication.OfficialRecord?.Officer?.FirstName,
                OfficialMiddleName = existingApplication.OfficialRecord?.Officer?.MiddleName,
                OfficialLastName = existingApplication.OfficialRecord?.Officer?.LastName,
                DeclarationDate = existingApplication.DeclarationDate,
                Removed = existingApplication.Removed,
                CreatedAt = existingApplication.CreatedAt,
                UpdatedAt = existingApplication.UpdatedAt
            };
        }

        // Soft Delete an Application
        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            application.Removed = true;  // Mark as removed
            application.UpdatedAt = DateTime.UtcNow;

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete an Application (if needed)
        public async Task<bool> PermanentlyDeleteApplicationAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
