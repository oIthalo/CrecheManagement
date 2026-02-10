using CrecheManagement.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsAuthenticatedAttribute : TypeFilterAttribute
{
    public IsAuthenticatedAttribute() : base(typeof(IsAuthenticatedFilter))
    {
    }
}