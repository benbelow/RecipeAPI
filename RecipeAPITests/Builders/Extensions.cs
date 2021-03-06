﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPITests.Builders
{
    public static class Extensions
    {
        public static IEnumerable<T> Plus<T>(this IEnumerable<T> head, T tail)
        {
            foreach (var item in head)
            {
                yield return item;
            }

            yield return tail;
        }

        public static IEnumerable<T> Times<T>(this int number, Func<T> generator)
        {
            return Enumerable.Range(1, number).Select(_ => generator());
        }

        /// <summary>
        /// Enumerates the supplied sequence continually to simulate an infinite sequence.
        /// Note that this only works if the sequence supports Enumerator.Reset().
        /// </summary>
        public static IEnumerable<T> Infinite<T>(this IEnumerable<T> items)
        {
            while (true)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                foreach (var item in items)
                {
                    yield return item;
                }

                // ReSharper disable once PossibleMultipleEnumeration
                items.GetEnumerator().Reset();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        /// Uses the supplied factory function to create an infinite sequence of values
        /// by iterating over a new collection every time it runs out of items.
        /// </summary>
        public static IEnumerable<T> Infinite<T>(this Func<IEnumerable<T>> getItems)
        {
            while (true)
            {
                var items = getItems();

                foreach (var item in items)
                {
                    yield return item;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public static Func<T> GetAccessor<T>(this IEnumerable<T> items)
        {
            var enumerator = items.GetEnumerator();

            return () =>
            {
                enumerator.MoveNext();
                return enumerator.Current;
            };
        }
    }
}