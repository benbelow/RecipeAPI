using System.Collections.Generic;
using FakeItEasy;

namespace RecipeAPITests.TestHelpers.FakeItEasy
{
    public static class Dummy
    {
        public static IEnumerable<T> CollectionOf<T>(int count)
        {
            var collection = new List<T>();
            for (var i = 0; i < count; i++)
            {
                collection.Add(A.Dummy<T>());
            }
            return collection;
        }
    }
}
