using AutoMapper;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Responses.Creche;

namespace CrecheManagement.Infrastructure.Mappings;

public class Mappings : Profile
{
    public Mappings()
    {
        ModelToResponse();
    }

    private void ModelToResponse()
    {
        CreateMap<Creche, CrecheResponse>();
    }
}