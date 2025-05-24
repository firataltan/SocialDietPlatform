using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;

namespace SocialDietPlatform.Persistence.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Like> GetFirstOrDefaultAsync(Expression<Func<Like, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Like> AddAsync(Like entity, CancellationToken cancellationToken = default)
        {
            await _context.Likes.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<Like>> GetAllAsync(Expression<Func<Like, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Like> query = _context.Likes;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Like> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Likes.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task UpdateAsync(Like entity, CancellationToken cancellationToken = default)
        {
            _context.Likes.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Like entity, CancellationToken cancellationToken = default)
        {
            _context.Likes.Remove(entity);
            await Task.CompletedTask;
        }
    }
} 