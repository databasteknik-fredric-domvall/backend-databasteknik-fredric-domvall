using Domain.Entities;
using FluentAssertions;

namespace backend.Tests.Domain;

public sealed class CourseOccasionTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateOccasion()
    {
        var courseId = Guid.NewGuid();
        Guid? teacherId = Guid.NewGuid();

        var occasion = new CourseOccasion(
            courseId,
            teacherId,
            new DateOnly(2026, 03, 01),
            new DateOnly(2026, 03, 10),
            20);

        occasion.Id.Should().NotBeEmpty();
        occasion.CourseId.Should().Be(courseId);
        occasion.TeacherId.Should().Be(teacherId);
        occasion.Capacity.Should().Be(20);
    }

    [Fact]
    public void Constructor_WithEndDateBeforeStartDate_ShouldThrow()
    {
        var courseId = Guid.NewGuid();

        var action = () => new CourseOccasion(
            courseId,
            null,
            new DateOnly(2026, 03, 10),
            new DateOnly(2026, 03, 01),
            10);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdate()
    {
        var courseId = Guid.NewGuid();

        var occasion = new CourseOccasion(
            courseId,
            null,
            new DateOnly(2026, 03, 01),
            new DateOnly(2026, 03, 10),
            10);

        var newTeacherId = Guid.NewGuid();

        occasion.Update(
            newTeacherId,
            new DateOnly(2026, 04, 01),
            new DateOnly(2026, 04, 05),
            15);

        occasion.TeacherId.Should().Be(newTeacherId);
        occasion.StartDate.Should().Be(new DateOnly(2026, 04, 01));
        occasion.EndDate.Should().Be(new DateOnly(2026, 04, 05));
        occasion.Capacity.Should().Be(15);
    }
}