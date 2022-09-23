using RaceClub.Models;

namespace RaceClub.Repository.Interfaces
{
    public interface IClubRepo
    {
        Task<IEnumerable<Club>> GetAll();
    
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
