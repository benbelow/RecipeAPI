using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedInstruction
    {
        public DetailedInstruction(Instruction instruction)
        {
            StepNumber = instruction.StepNumber.ToString();
            StepDescription = instruction.StepDescription;
        }

        public String StepNumber { get; set; }
        public String StepDescription { get; set; }
    }
}