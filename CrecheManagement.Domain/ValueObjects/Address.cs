namespace CrecheManagement.Domain.ValueObjects;

public sealed class Address
{
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
}