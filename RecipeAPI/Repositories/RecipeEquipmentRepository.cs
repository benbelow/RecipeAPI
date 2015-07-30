using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IRecipeEquipmentRepository : IRepository<RecipeEquipment>
    {
    }

    public class RecipeEquipmentRepository : Repository<RecipeEquipment>, IRecipeEquipmentRepository
    {
        public RecipeEquipmentRepository(DbContext context) : base(context) { }
    }
}