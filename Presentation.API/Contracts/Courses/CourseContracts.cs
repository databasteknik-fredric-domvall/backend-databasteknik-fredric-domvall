namespace Presentation.API.Contracts.Courses;

public sealed record CourseResponse(Guid Id, string Title, string Description);

public sealed record CreateCourseRequest(string Title, string Description);

public sealed record UpdateCourseRequest(string Title, string Description);