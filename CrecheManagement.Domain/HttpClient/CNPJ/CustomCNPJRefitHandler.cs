using System.Net;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Messages;

namespace CrecheManagement.Domain.HttpClient.CNPJ;

public class CustomCNPJRefitHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new CrecheManagementException(ReturnMessages.ERROR_CONSULT_CNPJ, HttpStatusCode.BadRequest);

        return response;
    }
}