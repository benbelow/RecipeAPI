using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class Instruction
    {
        public int InstructionID { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
