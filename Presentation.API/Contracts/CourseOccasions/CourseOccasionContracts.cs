namespace Presentation.API.Contracts.CourseOccasions;

public sealed record CourseOccasionResponse(
    Guid Id,
    Guid CourseId,
    Guid? TeacherId,
    string StartDate,
    string EndDate,
    int Capacity);

public sealed record CreateCourseOccasionRequest(
    Guid CourseId,
    Guid? TeacherId,
    string StartDate,
    string EndDate,
    int Capacity);

public sealed record UpdateCourseOccasionRequest(
    Guid? TeacherId,
    string StartDate,
    string EndDate,
    int Capacity);