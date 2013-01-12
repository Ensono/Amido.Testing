using System;

namespace Amido.Testing.TestDataBuilders
{
    public class TestDataBuilder<TBuilder, TEntity> where TBuilder : TestDataBuilder<TBuilder, TEntity>, new() where TEntity : new()
    {
        private readonly TEntity entity = new TEntity();

        protected TestDataBuilder()
        {
        }

        public static TBuilder New
        {
            get { return new TBuilder(); }
        }

        protected TEntity Entity
        {
            get { return this.entity; }
        }

        public TBuilder With(Action<TEntity> expression)
        {
            expression(this.entity);
            return (TBuilder)this;
        }

        public TEntity Build()
        {
            return this.entity;
        }
    }
}