using System.Reflection;

namespace BorgIncursion;

public static class ReflectionHelper
{
    public static BindingFlags CommonBindingFlags { get; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
}