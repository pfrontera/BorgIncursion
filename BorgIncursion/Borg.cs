using System.Reflection;

namespace BorgIncursion;

public static class Borg
{
    /// <summary>
    /// Resistance is futile! Extracts an internal class and returns the Type. Also provides an instance of the class.
    /// </summary>
    /// <param name="assemblyName">The name of the dll</param>
    /// <param name="className">The fully qualified name of the inner class.
    /// Example of a namespace `Provider.Cancel.Operations.CancelOperation`</param>
    /// <param name="parameters">Parameters to be passed to the constructor of the internal class. The type and order of parameters should match the constructor's signature.</param>
    /// <returns>An instance of the Type object representing the extracted inner class.</returns>
    public static BorgDrone Assimilate(string assemblyName, string className, params object[] parameters)
    {
        if (string.IsNullOrEmpty(assemblyName))
        {
            throw new ArgumentException("Assembly name cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(className))
        {
            throw new ArgumentException("Class name cannot be null or empty.");
        }

        try
        {
            var outerAssembly = Assembly.LoadFrom($"{assemblyName}.dll");
            var internalType = outerAssembly.GetTypes().FirstOrDefault(t => t.Name == className && t.IsNotPublic);
            if (internalType == null)
            {
                throw new Exception($"The class {className} was not found in the assembly {assemblyName}.dll");
            }
            var parameterTypes = parameters.Select(p => (Type)p.GetType()).ToArray();
            var constructor = internalType.GetConstructor(parameterTypes);
            if (constructor == null)
            {
                throw new InvalidOperationException($"The class '{className}' does not have a constructor that matches the given parameters.");

            }
            var instance = constructor.Invoke(IsNullParameters(parameters) ? null : parameters);
            
            return new BorgDrone(instance);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while trying to assimilate the class.", ex);
        }  
    }
    
    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <param name="drone">Our drone which has the instance of the class</param>
    /// <param name="methodName">The name of the method to be invoked on the instance.</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T>(this BorgDrone drone, string methodName, params object[] parameters)
    {
        var method = drone._Methods.FirstOrDefault(m => m.Name == methodName);
        if (method is null)
        {
            throw new InvalidOperationException($"Method '{methodName}' not found.");
        }
        return (T)method!.Invoke(drone.Instance, parameters)!;
    }
    
    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type with an
    /// out parameter.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <typeparam name="TOut">The type of the out param</typeparam>
    /// <param name="drone">Our drone which has the instance of the class</param>
    /// <param name="methodName">The method information to invoke.</param>
    /// <param name="outParam">The out param that will return the method.</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T, TOut>(
        this BorgDrone drone, 
        string methodName, 
        out TOut outParam, 
        params object[] parameters)
    {
        var method = drone._Methods.FirstOrDefault(m => m.Name == methodName);
        if (method is null)
        {
            throw new InvalidOperationException($"Method '{methodName}' not found.");
        }
        parameters = parameters.Append(null).ToArray();
        var result = (T)method!.Invoke(drone.Instance, parameters);
        outParam = (TOut)parameters.Last();
        return result;
    }
    
    /// <summary>
    /// Extension method that invokes a method using reflection and returns the result as the specified type with an
    /// array of out parameter.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result will be converted.</typeparam>
    /// <param name="drone">Our drone which has the instance of the class</param>
    /// <param name="methodName">The method information to invoke.</param>
    /// <param name="outParams">The out params that will return the method. You must know the type of the out params in
    /// order to iterate the object array of out params</param>
    /// <param name="parameters">The parameters for the method invocation.</param>
    /// <returns>The result of the Borgs execution converted to the specified type.</returns>
    public static T Execute<T>(this BorgDrone drone, string methodName, out object[] outParams,
        params object[] parameters)
    {
        var method = drone._Methods.FirstOrDefault(m => m.Name == methodName);
        if (method is null)
        {
            throw new InvalidOperationException($"Method '{methodName}' not found.");
        }
        var methodParameters = method.GetParameters();
        var skipParamsCount = parameters.Length;

        foreach (var parameter in methodParameters)
        {
            if (parameter.IsOut)
            {
                parameters = parameters.Append(null).ToArray();
            }
        }

        var result = (T)method.Invoke(drone.Instance, parameters);

        outParams = parameters.Skip(skipParamsCount).ToArray();
        
        return result;
    }
    
    private static bool IsNullParameters(object[] parameters)
    {
        return parameters is object[] && parameters.Length == 0;
    }
}