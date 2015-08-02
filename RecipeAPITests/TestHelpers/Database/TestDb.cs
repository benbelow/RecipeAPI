using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using NUnit.Framework;
using RecipeAPITests.TestHelpers.FakeItEasy;

namespace RecipeAPITests.TestHelpers.Database
{
    public static class TestDb
    {
        public static string ConnectionString
        {
            get
            {
                var dbFileName = String.Concat(TestContext.CurrentContext.Test.FullName.Split(Path.GetInvalidFileNameChars()));
                return String.Format("Data Source={0}.sdf", dbFileName);
            }
        }

        public static void Delete()
        {
            using (var context = new TestRecipesContext(ConnectionString))
            {
                context.Database.Delete();
            }
        }

        public static List<T> GetAll<T>() where T : class
        {
            using (var context = new TestRecipesContext(ConnectionString))
            {
                return context.Set<T>().ToList();
            }
        }

        public static IEnumerable<T> SeedMany<T>(int count) where T : class
        {
            return Seed(Dummy.CollectionOf<T>(count).ToArray());
        }

        public static IEnumerable<T> Seed<T>(params T[] entities) where T : class
        {
            return Seed(typeof(T), entities).Cast<T>();
        }

        private static IEnumerable<object> Seed(Type entityType, IEnumerable<object> entities)
        {
            var seededEntities = new List<object>();

            using (var context = new TestRecipesContext(ConnectionString))
            {
                seededEntities.AddRange(entities.Select(e => SeedEntity(context, entityType, e)));
                context.SaveChanges();
            }

            return seededEntities;
        }

        private static object SeedEntity(DbContext context, Type entityType, object entity)
        {
            SeedChildEntities(context, entityType, entity);
            return context.Set(entityType).Add(entity);
        }

        private static void SeedChildEntities(DbContext context, Type entityType, object entity)
        {
            var navigationProperties = GetNavigationProperties(context, entityType);
            foreach (var navigationProperty in navigationProperties)
            {
                SeedChildEntitiesForNavigationProperty(context, entityType, entity, navigationProperty);
            }
        }

        private static void SeedChildEntitiesForNavigationProperty(DbContext context, Type entityType, object entity, NavigationProperty navigationProperty)
        {
            var property = entityType.GetProperty(navigationProperty.Name);
            var dependentProperty = navigationProperty.GetDependentProperties().SingleOrDefault();
            var value = property.GetValue(entity);

            if (value is IEnumerable)
            {
                SeedCollectionEntities(context, value as IEnumerable);
            }
            else if (value != null && dependentProperty != null)
            {
                var keyProperty = entityType.GetProperty(dependentProperty.Name);
                var seededEntity = SeedEntity(context, property.PropertyType, value);
                var primaryKeyValue = GetPrimaryKeyValue(context, seededEntity);
                keyProperty.SetValue(entity, primaryKeyValue);
            }
        }

        private static void SeedCollectionEntities(DbContext context, IEnumerable collection)
        {
            var collectionType = collection.GetType().GenericTypeArguments.SingleOrDefault();
            foreach (var item in collection)
            {
                SeedChildEntities(context, collectionType ?? item.GetType(), item);
            }
        }

        private static IEnumerable<NavigationProperty> GetNavigationProperties(IObjectContextAdapter context, Type entityType)
        {
            var elementType = GetElementType(context, entityType);
            return elementType.NavigationProperties;
        }

        private static object GetPrimaryKeyValue(IObjectContextAdapter context, object entity)
        {
            var entityType = entity.GetType();
            var elementType = GetElementType(context, entityType);
            var propertyName = elementType.KeyProperties.Single().Name;
            return entityType.GetProperty(propertyName).GetValue(entity);
        }

        private static EntityType GetElementType(IObjectContextAdapter context, Type entityType)
        {
            var createObjectSetMethod = context.ObjectContext.GetType().GetMethod("CreateObjectSet", new Type[0]).MakeGenericMethod(entityType);
            dynamic objectSet = createObjectSetMethod.Invoke(context.ObjectContext, new object[0]);
            var entitySet = (EntitySet)objectSet.EntitySet;
            return entitySet.ElementType;
        }
    }
}
