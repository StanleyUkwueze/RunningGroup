using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Models;
using RaceClub.Repository.Interfaces;

namespace RaceClub.Repository.Implementation
{
    public class ClubRepo : IClubRepo
    {
        private readonly ApplicationDbContext _context;

        public ClubRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
          _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            var response = await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
            return response;
        }

        public bool Save()
        {
         return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
