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
        public async Task<Application> CreateApplicationAsync(Application application)
        {
            // Validate incoming application data (if needed)
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            application.CreatedAt = DateTime.UtcNow;
            application.UpdatedAt = DateTime.UtcNow;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return application;
        }

        // Get an Application by ID
        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.Applicant)  // Include related data like Applicant
                .Include(a => a.Program)    // Include related data like Program
                .Include(a => a.Status)     // Include related data like Status
                .Include(a => a.OfficialRecord) // Include related data like OfficialRecord
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);  // Ensure it's not marked as removed
        }

        // Get all Applications
        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Program)
                .Include(a => a.Status)
                .Include(a => a.OfficialRecord)
                .Where(a => a.Removed == false)  // Ensure applications are not marked as removed
                .ToListAsync();
        }

        // Update an Application
        public async Task<Application> UpdateApplicationAsync(int id, Application updatedApplication)
        {
            // Validate the incoming updated application data
            if (updatedApplication == null)
                throw new ArgumentNullException(nameof(updatedApplication));

            var existingApplication = await _context.Applications.FindAsync(id);

            if (existingApplication == null)
                throw new KeyNotFoundException("Application not found.");

            // Update properties
            existingApplication.ApplicationDate = updatedApplication.ApplicationDate;
            existingApplication.ApplicantId = updatedApplication.ApplicantId;
            existingApplication.ProgramId = updatedApplication.ProgramId;
            existingApplication.StatusId = updatedApplication.StatusId;
            existingApplication.OfficialRecordId = updatedApplication.OfficialRecordId;
            existingApplication.DeclarationDate = updatedApplication.DeclarationDate;
            existingApplication.UpdatedAt = DateTime.UtcNow;

            _context.Applications.Update(existingApplication);
            await _context.SaveChangesAsync();

            return existingApplication;
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
