﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MyASP.Model;

namespace MyASP.Services
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAllAsync();
        Task<Album> GetByIdAsync(int id);
        Task<Album> AddAsync(Album model);
        Task UpdateAsync(Album model);
        Task DeleteAsync(Album model);
    }
}
