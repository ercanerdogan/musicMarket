using Microsoft.EntityFrameworkCore;
using MusicMarket.Core.Models;
using MusicMarket.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicMarket.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        //private MusicMarketDbContext _context
        //{
        //    get { return _context; }
        //}
        private MusicMarketDbContext _context;
        public ArtistRepository(MusicMarketDbContext context)
            : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicsAsync()
        {
            return await _context.Artists
                .Include(a => a.Musics)
                .ToListAsync();
        }

        public Task<Artist> GetWithMusicsByIdAsync(int id)
        {
            return _context.Artists
                .Include(a => a.Musics)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

    }
        
}
