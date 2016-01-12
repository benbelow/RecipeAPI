using System.Linq;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        Equipment GetEquipmentByName(string name);
        Equipment GetOrCreateEquipment(string name);
    }

    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(DbContext context) : base(context) { }

        public Equipment GetEquipmentByName(string name)
        {
            return Entities.FirstOrDefault(e => e.Name == name);
        }

        public Equipment GetOrCreateEquipment(string name)
        {
            var equipment = GetEquipmentByName(name);
            if (equipment != null)
            {
                return equipment;
            }

            equipment = new Equipment
            {
                Name = name
            };

            Add(equipment);
            SaveContext();

            return equipment;
        }
    }
}