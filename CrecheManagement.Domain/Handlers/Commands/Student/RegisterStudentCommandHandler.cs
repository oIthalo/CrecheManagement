using System.Net;
using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Responses.Student;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Student;

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, BaseResponse<RegisteredStudentResponse>>
{
    private readonly IStudentsRepository _studentsRepository;
    private readonly IImageUploader _imageUploader;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;

    public RegisterStudentCommandHandler(
        IStudentsRepository studentsRepository,
        IImageUploader imageUploader,
        ICrecheAuthorizationService crecheAuthorizationService)
    {
        _studentsRepository = studentsRepository;
        _imageUploader = imageUploader;
        _crecheAuthorizationService = crecheAuthorizationService;
    }

    public async Task<BaseResponse<RegisteredStudentResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

        if (await _studentsRepository.ExistStudentWithCPFAsync(request.CrecheIdentifier!, request.CPF))
            throw new CrecheManagementException(ReturnMessages.STUDENT_EXIST_WITH_CPF, HttpStatusCode.Conflict);

        var student = new Models.Student()
        {
            Name = request.Name,
            ContactNumber = request.ContactNumber,
            BirthDate = request.BirthDate.Date,
            CrecheIdentifier = request.CrecheIdentifier!,
            Gender = request.Gender,
            CPF = Util.KeepLettersAndNumbers(request.CPF),
        };

        try
        {
            foreach (var image in request.Documents)
            {
                if (!Util.IsImage(image))
                    throw new CrecheManagementException(ReturnMessages.DOCUMENTS_INVALID, HttpStatusCode.BadRequest);

                var urlImage = await _imageUploader.UploadImageAsync(new ImageUploadDto
                {
                    File = image,
                    Folder = "students",
                    PublicId = student.Identifier
                });

                student.Documents.Add(urlImage);
            }
        }
        catch { }

        await _studentsRepository.UpsertAsync(student);

        return new BaseResponse<RegisteredStudentResponse>
        {
            StatusCode = HttpStatusCode.Created,
            Message = ReturnMessages.STUDENT_REGISTERED_SUCCESSFULLY,
            Data = new RegisteredStudentResponse
            {
                Identifier = student.Identifier,
                RegistrationId = student.RegistrationId,
                Name = student.Name
            }
        };
    }
}