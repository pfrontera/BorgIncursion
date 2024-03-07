internal class LocutusOfBorg
{
    private string _privateMessage;
    public string Message;
    
    public LocutusOfBorg(){}
    
    public LocutusOfBorg(string message)
    {
        Message = message;
        _privateMessage = "We are the Borg. Resistance is futile. Prepare your technology to be assimilated. The " +
                          "singularity is near. Resistance is futile. Adaptation is inevitable.";
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