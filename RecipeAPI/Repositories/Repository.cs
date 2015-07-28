using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace RecipeAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void SaveContext();
        IEnumerable<T> GetAll();
        void UpdateEntity(T entity);
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        protected Repository(DbContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Entities
        {
            get { return context.Set<T>(); }
        }

        public T Add(T entity)
        {
            Entities.Add(entity);
            return entity;
        }

        public void SaveContext()
        {
            context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.AsEnumerable();
        }

        public void UpdateEntity(T entity)
        {
            if (entity != null)
            {
                context.Entry(entity).Reload();
            }
        }
    }
}