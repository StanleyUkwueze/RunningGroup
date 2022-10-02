using RaceClub.Models;

namespace RaceClub.Repository.Interfaces
{
    public interface IDashboardRepo
    {
        Task<List<Race>> GetAllUserRaces();
          
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id)
    }
}
