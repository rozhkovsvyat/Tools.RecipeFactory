using System.Reflection;

namespace Tools;

/// <summary>
/// Abstract book of object recipes. Stores recipes for creating an object by child types.
/// </summary>
/// <typeparam name="TObj">Object type.</typeparam>
/// <typeparam name="TRecipe">Type of object recipe.</typeparam>
/// <typeparam name="TDefRecipe">Type of default object recipe.</typeparam>
public abstract class RecipeBook<TObj, TRecipe, TDefRecipe> where TObj : class
{
	/// <summary>
	/// Stores object recipes.
	/// </summary>
	protected Dictionary<string, TRecipe> Recipes { get; }

	/// <summary>
	/// List of object recipe names.
	/// </summary>
	public List<string> RecipeNames
        => Recipes.Select(r => 
            r.Key.ToFirstUpper()).ToList();

	/// <summary>
	/// Stores the object's default recipe.
	/// </summary>
	protected TDefRecipe DefaultRecipe { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="defaultRecipe">Default object recipe.</param>
	protected RecipeBook(TDefRecipe defaultRecipe)
    { 
        Recipes = new Dictionary<string, TRecipe>();
        DefaultRecipe = defaultRecipe;
    }

	/// <summary>
	/// Requests an object's recipes by calling static constructors on child types within its assembly.
	/// To add a recipe, use the AddRecipe method in the child type's static constructor.
	/// </summary>
	public void RequestBaseRecipes() => Reflection.Activate<TObj>();

	/// <summary>
	/// Requests an object's recipes by retrieving a static property with the specified name from child types in its assembly.
	/// To add a recipe, create static property in the child type's class.
	/// </summary>
	/// <typeparam name="T">Base type of descendants.</typeparam>
	/// <param name="propertyName">Property name.</param>
	public void RequestBasePropRecipes<T>(string propertyName)
	{
		foreach (var type in Reflection.GetTypeDescendants<T>())
			if (type.GetStaticPropertyValue<TRecipe>(propertyName) 
			    is { } recipe) AddRecipe(recipe, type.Name);
	}

	/// <summary>
	/// Requests an object's recipes by retrieving a static property with the specified name from child types within the call assembly.
	/// To add a recipe, create static property in the child type's class.
	/// </summary>
	/// <typeparam name="T">Base type of descendants.</typeparam>
	/// <param name="propertyName">Property name.</param>
	public void RequestLocalPropRecipes<T>(string propertyName)
	{
		foreach (var type in Reflection.GetTypeDescendants<T>(Assembly.GetCallingAssembly()))
			if (type.GetStaticPropertyValue<TRecipe>(propertyName)
			    is { } recipe) AddRecipe(recipe, type.Name);
	}

	/// <summary>
	/// Requests an object's recipes by calling static constructors on child types within the call assembly.
	/// To add a recipe, use the AddRecipe method in the child type's static constructor.
	/// </summary>
	public void RequestLocalRecipes()
        => Reflection.Activate<TObj>(Assembly.GetCallingAssembly());

	/// <summary>
	/// Adds a recipe of a child object type.
	/// </summary>
	/// <typeparam name="TInh">Child object type.</typeparam>
	/// <param name="recipe">Object recipe.</param>
	public void AddRecipe<TInh>(TRecipe recipe) where TInh : TObj
        => AddRecipe(recipe, typeof(TInh).Name);

	/// <summary>
	/// Adds a recipe of a child object type.
	/// </summary>
	/// <param name="recipe">Object recipe.</param>
	/// <param name="recipeName">Recipe name.</param>
	public void AddRecipe(TRecipe recipe, string recipeName)
        => Recipes.TryAdd(recipeName.ToLower(), recipe);
}
