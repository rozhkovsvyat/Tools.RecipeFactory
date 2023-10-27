namespace Tools;

#region Delegates

/// <summary>
/// Object recipe.
/// </summary>
/// <typeparam name="TObject">Object type.</typeparam>
/// <typeparam name="TArg">Object creation argument type.</typeparam>
/// <param name="args">Object creation arguments.</param>
/// <returns>New object.</returns>
public delegate TObject Recipe<out TObject, in TArg>(TArg args);

/// <summary>
/// Object recipe.
/// </summary>
/// <typeparam name="TObject">Object type.</typeparam>
/// <returns>New object.</returns>
public delegate TObject Recipe<out TObject>();

#endregion

/// <summary>
/// Object abstract factory. Produces specified type objects using child types recipes.
/// A recipe is a constructor delegate of a child type that takes a set of arguments.
/// </summary>
/// <typeparam name="TObj">Type of object produced by the factory.</typeparam>
/// <typeparam name="TArg">Type of arguments (ingredients) set for creating an object.</typeparam>
public class RecipeFactory<TObj, TArg> : 
    RecipeBook<TObj, Recipe<TObj, TArg>, Recipe<TObj>>, 
    IFactory<TObj, TArg> where TObj : class
{
    #region IFactory<TObj, TArg>

    /// <inheritdoc/>
    public TObj Get(string title, TArg args) 
        => Recipes.TryGetValue(title.ToLower(), out var recipe) ?
            recipe.Invoke(args) : DefaultRecipe.Invoke();

	#endregion

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="defaultRecipe">Object default recipe.</param>
	public RecipeFactory(Recipe<TObj> defaultRecipe) 
        : base(defaultRecipe) { }
}

/// <summary>
/// Object abstract factory. Produces specified type objects using child types recipes.
/// A recipe is a constructor delegate of a child type.
/// </summary>
/// <typeparam name="TObj">Type of object produced by the factory.</typeparam>
public class RecipeFactory<TObj> : 
    RecipeBook<TObj, Recipe<TObj>, Recipe<TObj>>, 
    IFactory<TObj> where TObj : class
{
	#region IFactory<TObj>

	/// <inheritdoc/>
	public TObj Get(string title) 
        => Recipes.TryGetValue(title.ToLower(), out var recipe) ?
            recipe.Invoke() : DefaultRecipe.Invoke();

	#endregion

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="defaultRecipe">Object default recipe.</param>
	public RecipeFactory(Recipe<TObj> defaultRecipe) 
        : base(defaultRecipe) { }
}
