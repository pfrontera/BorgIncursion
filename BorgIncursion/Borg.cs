﻿using System.Reflection;

namespace BorgIncursion;

public static class Borg
{
    /// <summary>
    /// Resistance is futile! Extracts an internal class and returns the Type. Also provides an instance of the class.
    /// </summary>
    /// <param name="assemblyName">The name of the dll</param>
    /// <param name="className">The fully qualified name of the inner class.
    /// Example of a namespace `CNN.Cancel.Operations.CancelOperation`</param>
    /// <param name="parameters">Parameters to be passed to the constructor of the internal class. The type and order of parameters should match the constructor's signature.</param>
    /// <returns>An instance of the Type object representing the extracted inner class.</returns>
    public static object Assimilate(string assemblyName, string className, params object[] parameters)
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
            return instance;
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
    /// <param name="instance">The instance on which to invoke the method.</param>
    /// <param name="methodName">The name of the method to be invoked on the instance.</param>
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
    /// array of out parameter.
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
    /// <param name="flags">A bitmask comprised of one or more BindingFlags that specify how the search is conducted. -or- Zero, to return null.</param>
    /// <returns>The MethodInfo object representing the method, or null if not found.</returns>
    private static MethodInfo? GetMethod(
        this IReflect type, 
        string methodName, 
        BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) =>
        type!.GetMethod(methodName, flags);
    
    
    private static bool IsNullParameters(object[] parameters)
    {
        return parameters is object[] && parameters.Length == 0;
    }
}