using System.Reflection;

namespace BorgIncursion;

public static class BorgIncursion
{
    /// <summary>
    /// Resistance is futile! Extracts an internal class and returns the Type. Also provides an instance of the class.
    /// </summary>
    /// <param name="className">The fully qualified name of the inner class.
    /// Example of a namespace `CNN.Cancel.Operations.CancelOperation`</param>
    /// <param name="instance">An instance of the extracted inner class.</param>
    /// <param name="parameters">parameters to be passed to the constructor</param>
    /// <returns>The Type object representing the extracted inner class.</returns>
    public static Type Assimilate(string className, out object instance, params object[] parameters)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        var targetAssembly = className.Split('.').FirstOrDefault();
        var projectReference = callingAssembly.GetReferencedAssemblies()
            .FirstOrDefault(a => a.Name == targetAssembly);
        var outerAssembly = Assembly.LoadFrom($"{projectReference.Name}.dll");
        var internalType = outerAssembly.GetType(className);

        var constructor = internalType.GetConstructor(Type.EmptyTypes);
        instance = constructor.Invoke(IsNullParameters(parameters) ? null : parameters);
        return internalType;
    }

    private static bool IsNullParameters(object[] parameters)
    {
        return parameters is object[] && parameters.Length == 0;
    }

    /// <summary>
    /// Invokes a method using reflection and returns the result as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <param name="instance">The instance on which to invoke the method.</param>
    /// <param name="method">The method information to invoke.</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Cthulhu's invocation converted to the specified type.</returns>
    public static T NeuralInvokeAs<T>(this object instance, MethodInfo method, params object[] parameters) =>
        (T)method!.Invoke(instance, parameters)!;
    
    /// <summary>
    /// Asks to the borg sphere for a method with the specified name on the given type.
    /// </summary>
    /// <param name="type">The type on which to search for the method.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <returns>The MethodInfo object representing the method, or null if not found.</returns>
    public static MethodInfo? CollectMethod(this Type type, string methodName) =>
        type!.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
}

internal class Locutus
{
    private string _message;
    
    public Locutus(){}
    
    public Locutus(string message)
    {
        _message = message;
    }
    public int Add(int a, int b)
    {
        return a + b;
    }
}
