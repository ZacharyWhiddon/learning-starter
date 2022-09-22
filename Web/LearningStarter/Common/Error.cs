namespace LearningStarter.Common;

public class Error
{
    public string Property { get; set; }
    public string Message { get; set; }

    public Error(string property, string message)
    {
        Property = property;
        Message = message;
    }
}
