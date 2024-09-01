﻿using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class FavoriteServices : Repository<Favorite>, IFavorite
    {
        private readonly MidprojectDbContext _context;

        public FavoriteServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
