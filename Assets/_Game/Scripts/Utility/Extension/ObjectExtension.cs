using System;
using System.Runtime.Serialization;

namespace _Game.Scripts.Utility.Extension
{
    public static class ObjectExtension
    {
        private static readonly ObjectIDGenerator idGenerator = new();
        
        public static bool TryCast<I, R>(this I input, out R result) where R : class
        {
            result = input as R;
            return result != null;
        }

        public static Box<T> Box<T>(this T value)
        {
            return new Box<T>(value);
        }

        public static T Unbox<T>(this Box<T> box)
        {
            return box.value;
        }

        public static long? GetId(this object value)
        {
            return value == null ? (long?) null : idGenerator.GetId(value, out var _);
        }

        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }

        public static bool TryGetValue<T>(this T? self, out T result) where T : struct
        {
            result = self.GetValueOrDefault();
            return self.HasValue;
        }
    }

    [Serializable]
    public class Box<T>
    {
        public readonly T value;

        public Box(T value)
        {
            this.value = value;
        }
    }
}