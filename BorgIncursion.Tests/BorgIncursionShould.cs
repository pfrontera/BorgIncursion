using BorgIncursion;
using FluentAssertions;
using FluentAssertions.Execution;

public class BorgIncursionShould : IDisposable
{
    private readonly object _droneBorg;
    private readonly Type _assimilatedType;

    public BorgIncursionShould()
    {
        _droneBorg = Borg.Assimilate("Borg","LocutusOfBorg");
    }
    
    public void Dispose() => GC.SuppressFinalize(this);

    [Fact]
    public void Assimilate_and_return_new_instance_as_borg_drone()
    {
        var instance = _droneBorg;
        
        using (new AssertionScope())
        {
            instance.Should().NotBeNull();
            instance.GetType().Name.Should().Be("LocutusOfBorg");
        }
    }
    
    [Fact]
    public void Assimilate_and_return_new_instance_as_borg_drone_passing_params_to_the_ctor()
    {
        var parameters = new object[]{"Resistance is futile!"} ;
        var sut = BorgIncursion.Borg.Assimilate("Borg","LocutusOfBorg", parameters);
        
        using (new AssertionScope())
        {
            sut.GetType().Should().NotBeNull();
            sut.GetType().Name.Should().Be("LocutusOfBorg");
        }
    }

    [Fact]
    public void Invoke_method_from_assimilated_borg_drone_and_returns_result()
    {
        var result = _droneBorg.Execute<int>("Add", 2, 3);
    
        result.Should().Be(5);
    }
    
    [Fact]
    public void Invoke_private_method_from_assimilated_borg_drone_and_returns_result()
    {
        var result = _droneBorg.Execute<int>("AddPrivate", 2, 3);
    
        result.Should().Be(5);
    }
    
    [Fact]
    public void Invoke_static_method_from_assimilated_borg_drone_and_returns_result()
    {
        var result = _droneBorg.Execute<int>("AddStatic", 2, 3);
    
        result.Should().Be(5);
    }
    
    [Fact]
    public void Invoke_private_static_method_from_assimilated_borg_drone_and_returns_result()
    {
        var result = _droneBorg.Execute<int>("AddPrivateStatic", 2, 3);
    
        result.Should().Be(5);
    }

    [Fact]
    public void Invoke_method_from_assimilated_borg_drone_and_returns_result_with_out_parameter()
    {
        var result = _droneBorg.Execute<int, string>("AddWithOut", out string outParameter, 2, 3);

        using (new AssertionScope())
        {
            result.Should().Be(5);
            outParameter.Should().Be("Resistance is futile!");
        }
    }

    [Fact]
    public void Invoke_method_from_assimilated_borg_drone_and_returns_result_with_two_out_parameter()
    {
        var result = _droneBorg.Execute<int>("AddWithTwoOuts", out var outParams, 2, 3);

        using (new AssertionScope())
        {
            result.Should().Be(5);
            outParams[0].Should().Be("Resistance is futile!");
            outParams[1].Should().Be("I sell opel corsa");
        }
    }
    
    [Fact]
    public void Throw_ArgumentException_When_AssemblyName_Is_Null_Or_Empty()
    {
        Action act = () => BorgIncursion.Borg.Assimilate(null, "LocutusOfBorg");
        act.Should().Throw<ArgumentException>().WithMessage("Assembly name cannot be null or empty.");

        act = () => BorgIncursion.Borg.Assimilate(string.Empty, "LocutusOfBorg");
        act.Should().Throw<ArgumentException>().WithMessage("Assembly name cannot be null or empty.");
    }

    [Fact]
    public void Throw_ArgumentException_When_ClassName_Is_Null_Or_Empty()
    {
        Action act = () => BorgIncursion.Borg.Assimilate("Borg", null);
        act.Should().Throw<ArgumentException>().WithMessage("Class name cannot be null or empty.");

        act = () => BorgIncursion.Borg.Assimilate("Borg", string.Empty);
        act.Should().Throw<ArgumentException>().WithMessage("Class name cannot be null or empty.");
    }

    [Fact]
    public void Throw_InvalidOperationException_When_Class_Does_Not_Exist_In_Assembly()
    {
        Assert.Throws<InvalidOperationException>(() => BorgIncursion.Borg.Assimilate("Borg", "NonExistentClass"));
    }

    [Fact]
    public void Throw_InvalidOperationException_When_Class_Does_Not_Have_Matching_Constructor()
    {
        Assert.Throws<InvalidOperationException>(() => BorgIncursion.Borg.Assimilate("Borg", "LocutusOfBorg", new object[] { 123 }));
    }
    
    [Fact]
    public void Throw_InvalidOperationException_When_Assembly_Does_Not_Exist()
    {
        Action act = () => BorgIncursion.Borg.Assimilate("NonExistentAssembly", "LocutusOfBorg");
        act.Should().Throw<InvalidOperationException>().WithMessage("An error occurred while trying to assimilate the class.");
    }
}


