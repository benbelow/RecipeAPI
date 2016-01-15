using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace RecipeAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Remove(T entity);
        void SaveContext();
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQuery();
        void UpdateEntity(T entity);
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext context;

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

        public void Remove(T entity)
        {
            Entities.Remove(entity);
        }

        public void SaveContext()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                throw dbEx;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.AsEnumerable();
        }

        public IQueryable<T> GetAllQuery()
        {
            return Entities;
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