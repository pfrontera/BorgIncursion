using System.Reflection;

namespace BorgIncursion;

public static class BorgIncursion
{
    //TODO : continuar redocumentant els métodes
    
    //TODO : Preveure excepcions i nulls per a donar informació en cas de malfuncionament.

    /// <summary>
    /// Resistance is futile! Extracts an internal class and returns the Type. Also provides an instance of the class.
    /// </summary>
    /// <param name="assemblyName">The name of the dll</param>
    /// <param name="className">The fully qualified name of the inner class.
    /// Example of a namespace `CNN.Cancel.Operations.CancelOperation`</param>
    /// <param name="parameters">parameters to be passed to the constructor</param>
    /// <returns>An instance of the Type object representing the extracted inner class.</returns>
    public static object Assimilate(string assemblyName, string className, params object[] parameters)
    {
        var outerAssembly = Assembly.LoadFrom($"{assemblyName}.dll");
        var internalType = outerAssembly.GetType(className);

        var constructor = internalType.GetConstructor(Type.EmptyTypes);
        var instance = constructor.Invoke(IsNullParameters(parameters) ? null : parameters);
        return instance;
    }


    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <param name="instance">The instance on which to invoke the method.</param>
    /// <param name="method">The method information to invoke.</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T>(this object instance, string methodName, params object[] parameters)
    {
        var method = GetMethod(instance.GetType(), methodName);
        return (T)method!.Invoke(instance, parameters)!;
    }


    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type with an
    /// out parameter.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <typeparam name="TOut">The type of the out param</typeparam>
    /// <param name="instance">The instance on which to invoke the method.</param>
    /// <param name="methodName">The method information to invoke.</param>
    /// <param name="outParam">The out param that will return the method.</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T, TOut>(
        this object instance, 
        string methodName, 
        out TOut outParam, 
        params object[] parameters)
    {
        var method = instance.GetType().CollectMethod(methodName);
        parameters = parameters.Append(null).ToArray();
        var result = (T)method!.Invoke(instance, parameters);
        outParam = (TOut)parameters.Last();
        return result;
    }

    
    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type with an
    /// out parameter.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <param name="instance">The instance on which to invoke the method.</param>
    /// <param name="methodName">The method information to invoke.</param>
    /// <param name="outParams">The out params that will return the method. You must know the type of the out params in
    /// order to iterate the object array of out params</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T>(this object instance, string methodName, out object[] outParams,
        params object[] parameters)
    {
        var method = instance.GetType().CollectMethod(methodName);
        var methodParameters = method.GetParameters();
        var skipParamsCount = parameters.Length;

        foreach (var parameter in methodParameters)
        {
            if (parameter.IsOut)
            {
                parameters = parameters.Append(null).ToArray();
            }
        }

        var result = (T)method.Invoke(instance, parameters);

        outParams = parameters.Skip(skipParamsCount).ToArray();
        
        return result;
    }

    private static MethodInfo CollectMethod(this Type type, string methodName)
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
    private static MethodInfo? GetMethod(
        this Type type, 
        string methodName, 
        BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) =>
        type!.GetMethod(methodName, flags);
    
    
    private static bool IsNullParameters(object[] parameters)
    {
        return parameters is object[] && parameters.Length == 0;
    }
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
    
    private static int AddPrivateStatic(int a, int b)
    {
        return a + b;
    }
    
    public static int AddStatic(int a, int b)
    {
        return a + b;
    }
    
    private int AddPrivate(int a, int b)
    {
        return a + b;
    }
    
    public int AddWithOut(int a, int b, out string message)
    {
        message = "Resistance is futile!";
        return a + b;
    }
    
    public int AddWithTwoOuts(int a, int b, out string message, out string message2)
    {
        message = "Resistance is futile!";
        message2 = "I sell opel corsa";
        return a + b;
    }
}
