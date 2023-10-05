namespace TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;

public class ServiceResponse<T>: IServiceResponse
{
    public T? Data { get; set; }
    public bool HasExceptionError { get; set; } = false;
    public string? ExceptionMessage { get; set; }
    public bool IsSuccessful { get; set; }=true;
}

public interface IServiceResponse
{
    bool HasExceptionError { get; set; }
    string ExceptionMessage { get; set; }
    bool IsSuccessful { get; set; }
}