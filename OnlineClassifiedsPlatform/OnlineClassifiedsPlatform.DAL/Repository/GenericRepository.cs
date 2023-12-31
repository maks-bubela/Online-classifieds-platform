﻿using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.DAL.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly OnlineClassifiedsPlatformContext _ctx;

        public GenericRepository(OnlineClassifiedsPlatformContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException();
        }

        public async Task<T> GetAsync<T>(long id) where T : class, IEntity
        {
            return await _ctx.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return await _ctx.Set<T>().Where(filter).SingleOrDefaultAsync();
        }

        public async Task<IList<T>> ListAsync<T>() where T : class, IEntity
        {
            return await _ctx.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return await _ctx.Set<T>().Where(filter).ToListAsync();
        }

        public async Task AddAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }
    }
}
