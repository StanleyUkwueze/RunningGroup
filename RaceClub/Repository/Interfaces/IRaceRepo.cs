﻿using RaceClub.Models;

namespace RaceClub.Repository.Interfaces
{
    public interface IRaceRepo
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAsync(int id);
        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}