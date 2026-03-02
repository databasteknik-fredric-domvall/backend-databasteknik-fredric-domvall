using Domain.Entities;
using FluentAssertions;

namespace backend.Tests.Domain;

public sealed class CourseTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCourse()
    {
        var course = new Course("C# Grund", "Intro");

        course.Id.Should().NotBeEmpty();
        course.Title.Should().Be("C# Grund");
        course.Description.Should().Be("Intro");
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ShouldThrow()
    {
        var action = () => new Course("", "Intro");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdate()
    {
        var course = new Course("Old", "Old desc");

        course.UpdateDetails("New", "New desc");

        course.Title.Should().Be("New");
        course.Description.Should().Be("New desc");
    }
}