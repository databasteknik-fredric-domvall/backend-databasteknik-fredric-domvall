namespace Presentation.API.Contracts.Teachers;

public sealed record TeacherResponse(Guid Id, string FirstName, string LastName, string Email);

public sealed record CreateTeacherRequest(string FirstName, string LastName, string Email);

public sealed record UpdateTeacherRequest(string FirstName, string LastName, string Email);