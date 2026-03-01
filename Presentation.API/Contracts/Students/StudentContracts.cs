namespace Presentation.API.Contracts.Students;

public sealed record StudentResponse(Guid Id, string FirstName, string LastName, string Email);

public sealed record CreateStudentRequest(string FirstName, string LastName, string Email);

public sealed record UpdateStudentRequest(string FirstName, string LastName, string Email);