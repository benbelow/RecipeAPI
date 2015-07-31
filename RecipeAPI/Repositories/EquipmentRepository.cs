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
        Equipment GetEquipmentByName(string name);
    }

    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(DbContext context) : base(context) { }

        public Equipment GetEquipmentByName(string name)
        {
            return GetAll().SingleOrDefault(i => i.Name == name);
        }
    }
}