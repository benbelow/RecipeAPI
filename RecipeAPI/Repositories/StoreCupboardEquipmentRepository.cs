using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IStoreCupboardEquipmentRepository : IRepository<StoreCupboardEquipment>
    {
    }

    public class StoreCupboardEquipmentRepository : Repository<StoreCupboardEquipment>, IStoreCupboardEquipmentRepository
    {
        public StoreCupboardEquipmentRepository(DbContext context) : base(context) { }
    }
}