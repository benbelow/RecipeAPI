using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Helpers.Comparers
{
    public class StoreCupboardEquipmentComparer : IEqualityComparer<StoreCupboardEquipment>
    {

        public bool Equals(StoreCupboardEquipment x, StoreCupboardEquipment y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.UserID == y.UserID && x.EquipmentID == y.EquipmentID;
        }

        public int GetHashCode(StoreCupboardEquipment storeCupboardEquipment)
        {
            if (Object.ReferenceEquals(storeCupboardEquipment, null)) return 0;

            int hashProductUser = storeCupboardEquipment.UserID.GetHashCode();

            int hashProductEquipment = storeCupboardEquipment.EquipmentID.GetHashCode();

            return hashProductUser ^ hashProductEquipment;
        }
    }
}