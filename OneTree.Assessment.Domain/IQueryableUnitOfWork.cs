﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace OneTree.Assessment.Domain
{
    public interface IQueryableUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        void DetachLocal<TEntity>(TEntity entity, EntityState state) where TEntity : class;
        DbContext GetContext();
        DbSet<TEntity> GetSet<TEntity>() where TEntity : class;
    }
}
