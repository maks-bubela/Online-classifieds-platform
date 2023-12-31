﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.DAL.Interfaces
{
    public interface IGenericRepository
    {
        Task<T> GetAsync<T>(long id) where T : class, IEntity;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
        Task<IList<T>> ListAsync<T>() where T : class, IEntity;
        Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;

        Task AddAsync<T>(T entity) where T : class, IEntity;
        Task UpdateAsync<T>(T entity) where T : class, IEntity;
        Task DeleteAsync<T>(T entity) where T : class, IEntity;
    }
}
