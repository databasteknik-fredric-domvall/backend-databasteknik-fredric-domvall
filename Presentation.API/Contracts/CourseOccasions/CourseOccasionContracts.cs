namespace Presentation.API.Contracts.CourseOccasions;

public sealed record CourseOccasionResponse(
    Guid Id,
    Guid CourseId,
    string StartDate,
    string EndDate,
    int Capacity);

public sealed record CreateCourseOccasionRequest(
    Guid CourseId,
    string StartDate,
    string EndDate,
    int Capacity);

public sealed record UpdateCourseOccasionRequest(
    string StartDate,
    string EndDate,
    int Capacity);