using Domain.Entities;
using FluentAssertions;

namespace backend.Tests.Domain;

public sealed class EnrollmentTests
{
    [Fact]
    public void Constructor_WithValidIds_ShouldCreateEnrollment()
    {
        var studentId = Guid.NewGuid();
        var occasionId = Guid.NewGuid();

        var enrollment = new Enrollment(studentId, occasionId);

        enrollment.StudentId.Should().Be(studentId);
        enrollment.CourseOccasionId.Should().Be(occasionId);
        enrollment.CreatedAtUtc.Should().BeAfter(DateTimeOffset.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public void Constructor_WithEmptyStudentId_ShouldThrow()
    {
        var action = () => new Enrollment(Guid.Empty, Guid.NewGuid());

        action.Should().Throw<ArgumentException>();
    }
}