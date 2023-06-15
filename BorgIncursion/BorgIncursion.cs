using System.Reflection;

namespace BorgIncursion;

public static class BorgIncursion
{
    //TODO : Redocumentar els métodes
    
    /// <summary>
    /// Resistance is futile! Extracts an internal class and returns the Type. Also provides an instance of the class.
    /// </summary>
    /// <param name="className">The fully qualified name of the inner class.
    /// Example of a namespace `CNN.Cancel.Operations.CancelOperation`</param>
    /// <param name="instance">An instance of the extracted inner class.</param>
    /// <param name="parameters">parameters to be passed to the constructor</param>
    /// <returns>The Type object representing the extracted inner class.</returns>
    public static object Assimilate(string className, out Type internalType, params object[] parameters)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        var targetAssembly = className.Split('.').FirstOrDefault();
        var projectReference = callingAssembly.GetReferencedAssemblies()
            .FirstOrDefault(a => a.Name == targetAssembly);
        var outerAssembly = Assembly.LoadFrom($"{projectReference.Name}.dll");
        internalType = outerAssembly.GetType(className);

        var constructor = internalType.GetConstructor(Type.EmptyTypes);
        var instance = constructor.Invoke(IsNullParameters(parameters) ? null : parameters);
        return instance;
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
    public static T Execute<T>(this object instance, string methodName, params object[] parameters)
    {
        var method = GetMethod(instance.GetType(), methodName);
        return (T)method!.Invoke(instance, parameters)!;
    }
        
    
    public static T Execute<T, TOut>(this object instance, string methodName, out TOut outParam, params object[] parameters)
    {
        var method = instance.GetType().CollectMethod(methodName);
        parameters = parameters.Append(null).ToArray();
        var result = (T)method!.Invoke(instance, parameters);
        outParam = (TOut)parameters.Last();
        return result;
    }

    public static MethodInfo CollectMethod(this Type type, string methodName)
    {
        var methodWithoutDefinedParams = type.GetMethod(methodName);
        var parameters = methodWithoutDefinedParams.GetParameters();
        var types = new Type[parameters.Length];
        
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].IsOut)
            {
                types[i] = parameters[i].ParameterType.GetElementType().MakeByRefType();
            }
            else
            {
                types[i] = parameters[i].ParameterType;
            }
        }
        var methodWithDefinedParams = type.GetMethod(methodName,types);

        return methodWithDefinedParams;
    }

    
    /// <summary>
    /// Asks to the borg sphere for a method with the specified name on the given type.
    /// </summary>
    /// <param name="type">The type on which to search for the method.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <returns>The MethodInfo object representing the method, or null if not found.</returns>
    private static MethodInfo? GetMethod(this Type type, string methodName) =>
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
    
    public int AddWithOut(int a, int b, out string message)
    {
        message = "Resistance is futile!";
        return a + b;
    }
}
