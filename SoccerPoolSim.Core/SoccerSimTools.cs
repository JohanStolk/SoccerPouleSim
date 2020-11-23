using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// static class with utility methods
    /// </summary>
    public static class SoccerSimTools
    {
        /// <summary>
        /// helper method to find derived types thru reflection for a specific type
        /// </summary>
        /// <typeparam name="T">the type in question</typeparam>
        /// <returns></returns>
        public static List<Type> FindAllDerivedTypes<T>()
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(T));
            if (assembly == null)
                throw new SoccerPoolSimException("couldn't find the assembly of type " + typeof(T));

            return FindAllDerivedTypes<T>(assembly);
        }

        /// <summary>
        /// generic helper method to find derived types thru reflection for a specific assembly
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="assembly">it's assembly</param>
        /// <returns></returns>
        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList();
        }

        /// <summary>
        /// generic helper method to avoid ugly casting
        /// </summary>
        /// <typeparam name="T">base type at compile time</typeparam>
        /// <param name="type">actual type at runtime</param>
        /// <returns></returns>
        public static T CreateInstanceOfType<T>(Type type)
        {
            object? obj = Activator.CreateInstance(type: type);
            if (obj == null)
                throw new SoccerPoolSimException("couldn't create instance of type " + type);

            return (T)obj;
        }
    }
}
