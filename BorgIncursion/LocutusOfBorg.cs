namespace BorgIncursion;

internal class LocutusOfBorg
{
    private string _message;
    
    public LocutusOfBorg(){}
    
    public LocutusOfBorg(string message)
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