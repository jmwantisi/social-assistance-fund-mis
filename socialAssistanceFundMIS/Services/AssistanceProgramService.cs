using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IProgramService
    {
        Task<List<AssistanceProgram>> GetAllPrograms();
        Task<AssistanceProgram?> GetProgramById(int id);
        Task<bool> AddProgram(AssistanceProgram program);
        Task<bool> UpdateProgram(AssistanceProgram program);
        Task<bool> DeleteProgram(int id);
    }

    public class AssistanceProgramService : IProgramService
    {
        private readonly ApplicationDbContext _context;

        public AssistanceProgramService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssistanceProgram>> GetAllPrograms()
        {
            return await _context.AssistancePrograms.ToListAsync();
        }

        public async Task<AssistanceProgram?> GetProgramById(int id)
        {
            return await _context.AssistancePrograms.FindAsync(id);
        }

        public async Task<bool> AddProgram(AssistanceProgram program)
        {
            _context.AssistancePrograms.Add(program);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProgram(AssistanceProgram program)
        {
            _context.AssistancePrograms.Update(program);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProgram(int id)
        {
            var program = await _context.AssistancePrograms.FindAsync(id);
            if (program == null) return false;

            _context.AssistancePrograms.Remove(program);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
