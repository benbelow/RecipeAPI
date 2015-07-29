using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IInstructionRepository : IRepository<Instruction>
    {
    }

    public class InstructionRepository : Repository<Instruction>, IInstructionRepository
    {
        public InstructionRepository(DbContext context) : base(context) { }

        public List<Instruction> GetInstructionsForRecipe(Recipe recipe)
        {
            return Entities.Where(i => i.RecipeID == recipe.RecipeID).ToList();
        }
    }
}