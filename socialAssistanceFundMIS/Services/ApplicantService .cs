using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationDbContext _context;

        public ApplicantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Applicant>> GetAllApplicantsAsync()
        {
            return await _context.Applicants.ToListAsync();
        }

        public async Task<Applicant?> GetApplicantByIdAsync(int id)
        {
            return await _context.Applicants.FindAsync(id);
        }

        public async Task<bool> AddApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Add(applicant);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await GetApplicantByIdAsync(id);
            if (applicant == null) return false;

            _context.Applicants.Remove(applicant);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
