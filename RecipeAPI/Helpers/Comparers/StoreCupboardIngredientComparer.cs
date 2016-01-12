using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Helpers.Comparers
{
    public class StoreCupboardIngredientComparer : IEqualityComparer<StoreCupboardIngredient>
    {

        public bool Equals(StoreCupboardIngredient x, StoreCupboardIngredient y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.UserID == y.UserID && x.IngredientID == y.IngredientID;
        }

        public int GetHashCode(StoreCupboardIngredient storeCupboardIngredient)
        {
            if (Object.ReferenceEquals(storeCupboardIngredient, null)) return 0;

            int hashProductUser = storeCupboardIngredient.UserID.GetHashCode();

            int hashProductIngredient = storeCupboardIngredient.IngredientID.GetHashCode();

            return hashProductUser ^ hashProductIngredient;
        }
    }
}