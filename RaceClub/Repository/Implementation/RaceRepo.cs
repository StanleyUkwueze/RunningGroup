using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Models;
using RaceClub.Repository.Interfaces;

namespace RaceClub.Repository.Implementation
{
   
    public class RaceRepo : IRaceRepo
    {
        private readonly ApplicationDbContext _context;

        public RaceRepo( ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Race race)
        {
            _context.Races.Add(race);
            return Save();
         }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races.Include(r => r.Address).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Include(i => i.Address).Where(r => r.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
          return _context.SaveChanges() > 0 ? true:false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
