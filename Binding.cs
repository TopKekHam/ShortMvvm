using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortMvvm
{
    class Binding<T>
    {

        T value;
        Action<T> subscribers;

        public static Binding<T> operator *(Binding<T> binding, T value)
        {
            binding.value = value;
            binding.subscribers?.Invoke(value);
            return binding;
        }

        public static Binding<T> operator +(Binding<T> binding, Action<T> sub)
        {
            binding.subscribers += sub;
            return binding;
        }

        public static Binding<T> operator -(Binding<T> binding, Action<T> sub)
        {
            binding.subscribers -= sub;
            return binding;
        }

        public static T operator ~(Binding<T> binding)
        {
            return binding.value;
        }

    }
}
