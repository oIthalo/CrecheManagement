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
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IImageUploader _imageUploader;

    public RegisterStudentCommandHandler(
        IStudentsRepository studentsRepository,
        ILoggedUser loggedUser,
        ICrechesRepository crechesRepository,
        IImageUploader imageUploader)
    {
        _studentsRepository = studentsRepository;
        _loggedUser = loggedUser;
        _crechesRepository = crechesRepository;
        _imageUploader = imageUploader;
    }

    public async Task<BaseResponse<RegisteredStudentResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

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