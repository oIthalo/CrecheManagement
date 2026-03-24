using CrecheManagement.Domain.Responses.Dashboard;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Dashboard;

public class GetDashboardQuery : IRequest<BaseResponse<DashboardResponse>>
{
    public string CrecheIdentifier { get; set; }
}