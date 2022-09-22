using System.Collections.Generic;

namespace LearningStarter.Common;

public class Response
{
    public object Data { get; set; }
    public List<Error> Errors { get; set; } = new List<Error>();
    public bool HasErrors => Errors.Count > 0;

    public void AddError(string property, string message)
    {
        Errors.Add(new Error(property, message));
    }
}
