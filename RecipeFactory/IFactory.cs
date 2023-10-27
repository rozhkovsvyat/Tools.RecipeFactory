namespace Tools;

/// <summary>
/// Represents the methods and properties of a specified type object abstract factory.
/// </summary>
/// <typeparam name="TObj">Type of object produced by the factory.</typeparam>
public interface IFactory<out TObj>
{
	/// <summary>
	/// Returns new object.
	/// </summary>
	/// <param name="title">Recipe name.</param>
	/// <returns>New object.</returns>
	TObj Get(string title);
}

/// <summary>
/// Represents the methods and properties of a specified type object abstract factory.
/// </summary>
/// <typeparam name="TObj">Type of object produced by the factory.</typeparam>
/// <typeparam name="TArg">Type of arguments (ingredients) set for creating an object</typeparam>
public interface IFactory<out TObj, in TArg>
{
	/// <summary>
	/// Returns new object.
	/// </summary>
	/// <param name="title">Recipe name.</param>
	/// <param name="args">Recipe arguments.</param>
	/// <returns>New object.</returns>
	public TObj Get(string title, TArg args);
}
