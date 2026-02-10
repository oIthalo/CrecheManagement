using Refit;

namespace CrecheManagement.Domain.HttpClient.CNPJ;

public interface ICNPJRefitClient
{
    [Get("/office/{cnpj}")]
    public Task<ApiResponse<object>> GetCompany(string cnpj);
}