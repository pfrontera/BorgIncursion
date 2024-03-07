namespace BorgIncursion;

public static class BorgDroneExtensions
{
    public static IEnumerable<string> GetMethods(this BorgDrone drone)
    {
        return drone._Methods.Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(p => (p.IsOut ? "out " : "") + GetTypeName(p.ParameterType) + " " + p.Name))}) -> {GetTypeName(m.ReturnType)}").ToList();
    }

    public static Dictionary<string, object> GetFields(this BorgDrone drone)
    {
        var fieldValues = new Dictionary<string, object>();
        foreach (var field in drone._Fields)
        {
            var value = field.GetValue(drone.Instance);
            fieldValues.Add(field.Name, value);
        }
        return fieldValues;
    }

    public static IEnumerable<string> GetConstructors(this BorgDrone drone)
    {
        return drone._Constructors.Select(c => $"{c.Name}({string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))})").ToList();
    }
    
    private static readonly Dictionary<string, string> TypeMappings = new Dictionary<string, string>
    {
        { "Int32", "int" },
        { "String&", "string" },
        { "Boolean", "bool"} ,
        { "String", "string"}
    };
    
    private static string GetTypeName(Type type)
    {
        var typeName = type.Name;
        return TypeMappings.ContainsKey(typeName) ? TypeMappings[typeName] : typeName;
    }
}