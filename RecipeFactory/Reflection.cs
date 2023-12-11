using System.Runtime.CompilerServices;
using System.Reflection;

namespace Tools;

/// <summary>
/// Contains methods for working with an assembly, modules, members, parameters and other objects in managed code.
/// </summary>
internal static class Reflection
{
	/// <summary>
	/// Returns all base type descendants within the specified assembly.
	/// </summary>
	/// <typeparam name="T">Base type.</typeparam>
	/// <param name="assembly">Specified assembly.</param>
	/// <returns>Array of type descendants.</returns>
	internal static Type[] GetTypeDescendants<T>(Assembly assembly)
		=> assembly.GetTypes().Where(t => 
			(t.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(t)) 
			&& !t.IsAbstract).Select(t => t).ToArray();

	/// <summary>
	/// Calls a static constructor for all base type descendants within its assembly.    
	/// </summary>
	/// <typeparam name="T">Base type.</typeparam>
	internal static void Activate<T>() => Activate<T>(typeof(T).Assembly);

	/// <summary>
	/// Calls a static constructor for all base type descendants within the specified assembly.
	/// </summary>
	/// <typeparam name="T">Base type.</typeparam>
	/// <param name="assembly">Specified assembly.</param>
	internal static void Activate<T>(Assembly assembly)
    {
	    foreach (var type in GetTypeDescendants<T>(assembly)) 
		    RuntimeHelpers.RunClassConstructor(type.TypeHandle);
    }

	/// <summary>
	/// Gets the value of a specific type static property.
	/// </summary>
	/// <typeparam name="T">Return value type.</typeparam>
	/// <param name="t">Current type.</param>
	/// <param name="propertyName">Property name.</param>
	/// <returns>Property value.</returns>
	public static T? GetStaticPropertyValue<T>(this Type t, string propertyName)
	{
		const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

		var info = t.GetProperty(propertyName, flags);

		if (info is not null) return info.GetValue(null, null) 
				is { } infoValue ? (T)infoValue : default;

		var fieldInfo = t.GetField(propertyName, flags);

		return fieldInfo is null ? default : fieldInfo.GetValue(null) 
			is { } fieldValue ? (T)fieldValue : default;

	}
}
