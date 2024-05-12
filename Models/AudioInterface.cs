

using System;

namespace Models
{
    public class AudioInterface : IEquatable<AudioInterface>
    {
        public string Name { get; set; }
        public string    Id   { get; set; }

        public bool Equals(AudioInterface? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AudioInterface)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id);
        }

        public static bool operator ==(AudioInterface? left, AudioInterface? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AudioInterface? left, AudioInterface? right)
        {
            return !Equals(left, right);
        }
    }
}