using System.Reflection;

namespace BorgIncursion;

public class BorgDrone
{
    public object Instance { get;}
    public IEnumerable<string> Methods => this.GetMethods();
    public Dictionary<string, object> Fields => this.GetFields();
    public IEnumerable<string> Constructors => this.GetConstructors();
    internal Type Type { get; }
    internal MethodInfo[] _Methods { get; }
    internal FieldInfo[] _Fields { get; }
    internal ConstructorInfo[] _Constructors { get; }
    
    
    public BorgDrone(object instance)
    {
        Instance = instance;
        Type = instance.GetType();
        _Methods = Type.GetMethods(ReflectionHelper.CommonBindingFlags);
        _Fields = Type.GetFields(ReflectionHelper.CommonBindingFlags);
        _Constructors = Type.GetConstructors(ReflectionHelper.CommonBindingFlags);
    }
}