using System.Runtime.CompilerServices;
using System.Reflection;

namespace Tools;

/// <summary>
/// Contains methods for working with an assembly, modules, members, parameters and other objects in managed code.
/// </summary>
internal class Reflection
{
	/// <summary>
	/// Calls a static constructor for all base type descendants within its assembly.    
	/// </summary>
	/// <typeparam name="T">Base type.</typeparam>
	internal static void Activate<T>() 
        => Activate<T>(typeof(T).Assembly);

	/// <summary>
	/// Calls a static constructor for all base type descendants within the specified assembly.
	/// </summary>
	/// <typeparam name="T">Base type.</typeparam>
	/// <param name="assembly">Specified assembly.</param>
	internal static void Activate<T>(Assembly assembly)
    { 
        var types = assembly.GetTypes().Where(t => 
                (t.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(t)) 
                && !t.IsAbstract).Select(t => t).ToArray();
        foreach (var type in types) RuntimeHelpers.RunClassConstructor(type.TypeHandle);
    }
}
