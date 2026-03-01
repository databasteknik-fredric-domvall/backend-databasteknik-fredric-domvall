namespace Presentation.API.Contracts.Enrollments;

public sealed record EnrollmentResponse(
    Guid StudentId,
    Guid CourseOccasionId,
    DateTimeOffset CreatedAtUtc);

public sealed record CreateEnrollmentRequest(
    Guid StudentId,
    Guid CourseOccasionId);