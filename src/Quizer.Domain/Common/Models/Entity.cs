namespace Quizer.Domain.Common.Models
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : notnull
    {
        public TId Id { get; protected set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var entity = (Entity<TId>)obj;

            return Id.Equals(entity.Id);
        }

        public bool Equals(Entity<TId>? other)
        {
            return Equals(other);
        }

        public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
        {
            return !Equals(a, b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

#pragma warning disable CS8618
        protected Entity()
        {
        }
#pragma warning restore CS8618
    }
}
