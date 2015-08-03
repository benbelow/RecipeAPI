using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeAPITests.Builders
{
    public sealed class Builder<TInstance> where TInstance : class, new()
    {
        private Builder()
        {
            Blueprint = Enumerable.Empty<Action<TInstance>>();
        }

        private Builder(IEnumerable<Action<TInstance>> blueprint)
            : this()
        {
            Blueprint = blueprint;
        }

        public static Builder<TInstance> New
        {
            get { return new Builder<TInstance>(); }
        }

        private IEnumerable<Action<TInstance>> Blueprint { get; set; }

        public static implicit operator TInstance(Builder<TInstance> source)
        {
            return source.Build();
        }

        public TInstance Build()
        {
            return Build(new TInstance());
        }

        public TInstance Build(TInstance baseInstance)
        {
            return ApplyBlueprint(new TInstance());
        }

        /// <summary>
        ///     Sets a property on the constructed instance, generated using the default builder for the type of the property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, TProp>> selector) where TProp : class, new()
        {
            var propBuilder = BuilderRegistry.Resolve<TProp>();
            return With(selector, propBuilder.Build());
        }

        /// <summary>
        ///     Sets a collection property on the constructed instance, generating a list using the defact builder for the type of
        ///     the property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="quantity">The number of instances to create and add to the collection.</param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, ICollection<TProp>>> selector, int quantity = 2) where TProp : class, new()
        {
            var propBuilder = BuilderRegistry.Resolve<TProp>();
            return With(selector, quantity.Times(propBuilder.Build).ToList());
        }

        /// <summary>
        ///     Sets a collection property on the constructed instance, generating a list using the defact builder for the type of
        ///     the property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="builder">A builder which can create instances matching the type of the collection.</param>
        /// <param name="quantity">The number of instances to create and add to the collection.</param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, ICollection<TProp>>> selector, Builder<TProp> builder, int quantity = 2) where TProp : class, new()
        {
            return With(selector, quantity.Times(builder.Build).ToList());
        }

        /// <summary>
        ///     Sets a property on the constructed instance.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="value">
        ///     The value to assign. Note that this value will be shared between all instances constructed by this
        ///     builder
        /// </param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, TProp>> selector, TProp value)
        {
            var selectorBody = (MemberExpression)selector.Body;
            var instance = Expression.Parameter(typeof(TInstance), typeof(TInstance).FullName);
            var prop = Expression.Property(instance, selectorBody.Member.Name);
            var val = Expression.Constant(value, typeof(TProp));
            var assign = Expression.Assign(prop, val);

            var result = Expression.Lambda<Action<TInstance>>(assign, instance).Compile();
            return new Builder<TInstance>(Blueprint.Plus(result));
        }

        /// <summary>
        ///     Sets a property on the constructed instance.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="values">An enumerable which will be iterated over to obtain a value to assign.</param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, TProp>> selector, IEnumerable<TProp> values)
        {
            var factory = values.Infinite().GetAccessor();
            return WithFactory(selector, factory);
        }

        /// <summary>
        ///     Sets a property on the constructed instance.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="values">An function which generates a set of values to be iterated and assigned sequentially.</param>
        public Builder<TInstance> With<TProp>(Expression<Func<TInstance, TProp>> selector, Func<IEnumerable<TProp>> values)
        {
            var factory = values.Infinite().GetAccessor();
            return WithFactory(selector, factory);
        }

        /// <summary>
        ///     Sets a property of type ICollection on the constructed instance.
        /// </summary>
        /// <typeparam name="TProp">The ICollection type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="values">An enumerable which will be used to construct a Collection to assign to the property.</param>
        public Builder<TInstance> WithCollection<TProp>(Expression<Func<TInstance, ICollection<TProp>>> selector, IEnumerable<TProp> values)
        {
            var collection = new Collection<TProp>(values.ToList());
            return With(selector, collection);
        }

        /// <summary>
        ///     Sets a factory to generate the value of a property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property to set.</typeparam>
        /// <param name="selector">A delegate which specifies the property to set.</param>
        /// <param name="value">A factory method to generate an value for each constructed instance.</param>
        public Builder<TInstance> WithFactory<TProp>(Expression<Func<TInstance, TProp>> selector, Func<TProp> value)
        {
            var selectorBody = (MemberExpression)selector.Body;
            var instance = Expression.Parameter(typeof(TInstance), typeof(TInstance).FullName);
            var prop = Expression.Property(instance, selectorBody.Member.Name);
            Expression<Func<TProp>> valueInvoker = () => value();

            var setExpression = Expression.Assign(prop, Expression.Invoke(valueInvoker));
            var setLambda = Expression.Lambda<Action<TInstance>>(setExpression, instance).Compile();

            return new Builder<TInstance>(Blueprint.Plus(setLambda));
        }

        /// <summary>
        ///     Adds an item to a collection on the constructed object, generated using the default builder for the type of the
        ///     collection.
        /// </summary>
        /// <typeparam name="TProp">The type of the object to add.</typeparam>
        /// <param name="selector">A delegate which specifies the target collection.</param>
        public Builder<TInstance> Add<TProp>(Expression<Func<TInstance, ICollection<TProp>>> selector) where TProp : class, new()
        {
            var propBuilder = BuilderRegistry.Resolve<TProp>();
            return Add(selector, propBuilder.Build());
        }

        /// <summary>
        ///     Adds an item to a collection on the constructed object.
        /// </summary>
        /// <typeparam name="TProp">The type of the object to add.</typeparam>
        /// <param name="selector">A delegate which specifies the target collection.</param>
        /// <param name="value">The value which will be added to the collection</param>
        public Builder<TInstance> Add<TProp>(Expression<Func<TInstance, ICollection<TProp>>> selector, TProp value) where TProp : class, new()
        {
            var selectorBody = (MemberExpression)selector.Body;
            var instance = Expression.Parameter(typeof(TInstance), typeof(TInstance).FullName);
            var collectionGetter = typeof(TInstance).GetProperty(selectorBody.Member.Name).GetGetMethod();
            var addMethod = typeof(ICollection<TProp>).GetMethod("Add");
            var addParameter = Expression.Constant(value, typeof(TProp));

            var getCollection = Expression.Call(instance, collectionGetter);
            // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            var addItem = Expression.Call(getCollection, addMethod, addParameter);
            var addItemToCollection = Expression.Lambda<Action<TInstance>>(addItem, instance).Compile();

            return new Builder<TInstance>(Blueprint.Plus(addItemToCollection));
        }

        private TInstance ApplyBlueprint(TInstance result)
        {
            foreach (var action in Blueprint)
            {
                action(result);
            }

            return result;
        }
    }
}