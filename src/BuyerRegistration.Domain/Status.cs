using Newtonsoft.Json.Converters;
namespace BuyerRegistration.Domain{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        None = 0,
        Denied,
        Pending,
        Approved
    }
}