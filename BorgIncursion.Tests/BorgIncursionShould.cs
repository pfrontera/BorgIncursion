using FluentAssertions;
using FluentAssertions.Execution;

namespace BorgIncursion.Tests;

public class BorgIncursionShould : IDisposable
{
    private readonly object _droneBorg;
    private readonly Type _assimilatedType;

    public BorgIncursionShould()
    {
        _droneBorg = BorgIncursion.Assimilate("BorgIncursion","BorgIncursion.Locutus");
    }
    
    public void Dispose() => GC.SuppressFinalize(this);

    [Fact]
    public void Assimilate_and_return_new_instance_as_borg_drone()
    {
        var instance = _droneBorg;
        
        using (new AssertionScope())
        {
            instance.Should().NotBeNull();
            instance.GetType().Name.Should().Be("Locutus");
        }
    }
    
    [Fact(Skip = "Working on it...")]
    public void Assimilate_and_return_new_instance_as_borg_drone_passing_params_to_the_ctor()
    {
        var parameters = new object[]{new {Message = "Resistance is futile!"}} ;
        var sut = BorgIncursion.Assimilate("BorgIncursion","BorgIncursion.Locutus", parameters);
        
        using (new AssertionScope())
        {
            sut.GetType().Should().NotBeNull();
            sut.GetType().Name.Should().Be("Locutus");
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
        var result = _droneBorg.Execute<int>("AddWithTwoOuts", out  var outParams, 2, 3);

        using (new AssertionScope())
        {
            result.Should().Be(5);
            outParams[0].Should().Be("Resistance is futile!");
            outParams[1].Should().Be("I sell opel corsa");
        }
    }
}


