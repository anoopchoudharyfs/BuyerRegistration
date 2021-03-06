namespace BuyerRegistration.Api.Services.IService;

public interface ICorrelationIdManager
{
    string InitializeCorrelationId();
    void SetCorrelationId(string correlationId);
}


public interface ICorrelationIdProvider
{
    string? GetCorrelationId();
}