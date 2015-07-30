using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
    }

    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(DbContext context) : base(context) { }
    }
}