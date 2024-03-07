# BorgIncursion
![BorgIncursion Logo](https://github.com/pfrontera/BorgIncursion/blob/main/assets/logo.png?raw=true)

## [English](#English) | [Català](#català) 

 

### English
Resistance is futile! Extract internal classes and assimilate a new instance for your own use.

BorgIncursion is a library in C# that provides a way to interact with internal classes of an assembly. It allows you to extract an internal class and create a new instance of it for your own use. This can be particularly useful when you need to test or interact with internal classes that are not normally accessible.

The library provides the `BorgDrone` class that encapsulates an instance obtained through reflection. The `BorgDrone` class has the following properties:

- `Methods`: A list of strings representing the methods of the instance. Each string includes the method name, its parameters (including whether they are `out` parameters), and the return type.
- `Fields`: A dictionary mapping field names to their current values. This allows you to inspect the current state of the instance.
- `Constructors`: A list of strings representing the constructors of the instance. Each string includes the constructor name and its parameters.

However, it's important to note that BorgIncursion makes use of reflection, which is a powerful but potentially dangerous feature. Reflection allows you to inspect and interact with code dynamically, but it can also lead to code that is hard to understand, difficult to debug, and prone to runtime errors. Therefore, it's recommended to use reflection sparingly and only when absolutely necessary.

BorgIncursion is designed to handle a wide range of use cases. It can reflect on both static and non-static properties, as well as public and non-public properties. This makes it a versatile tool for interacting with internal classes in a variety of scenarios.


## Usage

### Assimilate

Like the Borg assimilated Captain Picard, you can extract an internal class and assimilate it by creating a new instance of it, using the Assimilate method. Here is an example:

```csharp
var drone = Borg.Assimilate("MyAssembly", "MyNamespace.MyClass");
```
In this example, `MyAssembly` is the name of the assembly that contains the internal class, and `MyNamespace.MyClass` is the fully qualified name of the internal class. The `Assimilate` method returns a `BorgDrone` instance that encapsulates the created instance of the internal class.  

You can also pass parameters to the constructor when calling the `Assimilate` method. The parameters are passed after the class name and are used to invoke the constructor of the class that matches the signature of the provided parameters. Here is an example:  
```csharp
var drone = Borg.Assimilate("MyAssembly", "MyNamespace.MyClass", arg1, arg2);
```
In this example, `arg1` and `arg2` are the arguments you want to pass to the constructor of the class. `Borg.Assimilate` will use these arguments to invoke the constructor that matches the signature of the provided arguments.

### Execute
To invoke a method on an object using reflection, you can use the Execute method. There are three overloads of this method to handle different scenarios.

Without out parameters :
```csharp
var drone = instance.Execute<int>("MyMethod", arg1, arg2);
```
In this example, `MyMethod` is the name of the method you want to invoke, and `arg1` and `arg2` are the arguments you want to pass to the method. The result of the method invocation is converted to the specified type (`int` in this case) and returned. 
 
With a single out parameter :
```csharp
var result = drone.Execute<int, string>("MyMethod", out string outParam, arg1, arg2);
```
In this example, `MyMethod` is the name of the method you want to invoke, `arg1` and `arg2` are the arguments you want to pass to the method, and `outParam` is a variable where the value of the `out` parameter will be stored. The result of the method invocation is converted to the specified type (`int` in this case) and returned. 
 
With multiple out parameters :
```csharp
var result = drone.Execute<int>("MyMethod", out var outParams, arg1, arg2);
```
In this example, `MyMethod` is the name of the method you want to invoke, `arg1` and `arg2` are the arguments you want to pass to the method, and `outParams` is a variable where the values of the out parameters will be stored in an array. The result of the method invocation is converted to the specified type (`int` in this case) and returned.
You must know the type of the out params in order to iterate the object array of out params.  

##### _This is open-source software, and you can verify its functionality through its unit tests. The best way to understand what a piece of code does is by taking a look at its tests._


----------

### Català
# BorgIncursion
La resistència és fútil! Extreu classes internes i assimila una nova instància per al teu ús.

BorgIncursion és una llibreria en C# que proporciona una manera d'interactuar amb classes internes d'una assemblatge. Et permet extreure una classe interna i crear una nova instància d'aquesta per al teu ús. Això pot ser particularment útil quan necessites provar o interactuar amb classes internes que normalment no són accessibles.
La biblioteca proporciona la classe `BorgDrone` que encapsula una instància obtinguda a través de la reflexió. La classe `BorgDrone` té les següents propietats:

- `Methods`: Un llistat de strings que representen els mètodes de la instància. Cada string inclou el nom del mètode, els seus paràmetres (incloent si són paràmetres `out`), i el tipus de retorn.
- `Fields`: Un diccionari que mapeja els noms dels camps als seus valors actuals. Això et permet inspeccionar l'estat actual de la instància.
- `Constructors`: Un llistat de string que representen els constructors de la instància. Cada string inclou el nom del constructor i els seus paràmetres.

No obstant això, és important destacar que BorgIncursion fa ús de la reflexió, que és una característica potent però potencialment perillosa. La reflexió et permet inspeccionar i interactuar amb el codi de manera dinàmica, però també pot portar a codi que és difícil d'entendre, difícil de depurar, i propens a errors en temps d'execució. Per tant, es recomana utilitzar la reflexió amb moderació i només quan sigui absolutament necessari.

BorgIncursion està dissenyat per a manejar una àmplia gamma de casos d'ús. Pot reflexionar tant sobre propietats estàtiques com no estàtiques, així com sobre propietats públiques i no públiques. Això el fa una eina versàtil per a interactuar amb classes internes en una varietat d'escenaris.

## Ús

### Assimilate

Com els Borg van assimilar al Capità Picard, pots extreure una classe interna i assimilar-la creant una nova instància d'aquesta, utilitzant el mètode Assimilate. Aquí tens un exemple:

```csharp
var drone = Borg.Assimilate("MyAssembly", "MyNamespace.MyClass");
```  
En aquest exemple, `MyAssembly` és el nom de l'assemblatge que conté la classe interna, i `MyNamespace.MyClass` és el nom completament qualificat de la classe interna. El mètode `Assimilate` retorna una instància de `BorgDrone` que encapsula la instància creada de la classe interna.  

També pots passar paràmetres al constructor quan crides al mètode `Assimilate`. Els paràmetres es passen després del nom de la classe i s'utilitzen per invocar el constructor de la classe que coincideix amb la signatura dels paràmetres proporcionats. Aquí tens un exemple:
```csharp
var drone = Borg.Assimilate("MyAssembly", "MyNamespace.MyClass", arg1, arg2);
```
En aquest exemple, `arg1` i `arg2` són els arguments que vols passar al constructor de la classe. `Borg.Assimilate` utilitzarà aquests arguments per invocar el constructor que coincideix amb la signatura dels arguments proporcionats.

### Execute
Per a invocar un mètode en un objecte utilitzant reflexió, pots utilitzar el mètode `Execute`. Hi ha tres sobrecàrregues d'aquest mètode per a manejar diferents escenaris. 

Sense paràmetres de sortida :
```csharp
var result = drone.Execute<int>("MyMethod", arg1, arg2);
```  

En aquest exemple, `MyMethod` és el nom del mètode que vols invocar, i `arg1` i `arg2` són els arguments que vols passar al mètode. El resultat de la invocació del mètode es converteix al tipus especificat (`int` en aquest cas) i es retorna.

Amb un únic paràmetre de sortida :
```csharp
var result = drone.Execute<int, string>("MyMethod", out string outParam, arg1, arg2);
``` 
En aquest exemple, `MyMethod` és el nom del mètode que vols invocar, `arg1` i `arg2` són els arguments que vols passar al mètode, i `outParam` és una variable on es guardarà el valor del paràmetre de sortida. El resultat de la invocació del mètode es converteix al tipus especificat (`int` en aquest cas) i es retorna.

Amb múltiples paràmetres de sortida :
```csharp
var result = drone.Execute<int>("MyMethod", out var outParams, arg1, arg2);
```
En aquest exemple, `MyMethod` és el nom del mètode que vols invocar, `arg1` i `arg2` són els arguments que vols passar al mètode, i `outParams` és una variable on es guardarà el valor dels paràmetres de sortida en un array. El resultat de la invocació del mètode es converteix al tipus especificat (`int` en aquest cas) i es retorna.
Has de saber el tipus dels paràmetres de sortida per poder iterar l'array d'objectes de paràmetres de sortida.  
  
  
##### _Aquest és un programari de codi obert, i pots verificar la seva funcionalitat a través dels seus tests unitaris. La millor manera d'entendre què fa un tros de codi és donant una ullada als seus tests.**_

------
 
----- 

![BorgIncursion Readme](https://raw.githubusercontent.com/pfrontera/BorgIncursion/main/assets/readme.jpg)  

_Locutus of borg will keep updating...Testing is not futile. It ensures the perfection of assimilated code._

