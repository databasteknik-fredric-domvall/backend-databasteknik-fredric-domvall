using Domain.Entities;
using FluentAssertions;

namespace backend.Tests.Domain;

public sealed class StudentTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateStudent()
    {
        var student = new Student("Anna", "Svensson", "anna@example.com");

        student.Id.Should().NotBeEmpty();
        student.FirstName.Should().Be("Anna");
        student.LastName.Should().Be("Svensson");
        student.Email.Should().Be("anna@example.com");
    }

    [Fact]
    public void SetEmail_WithEmptyEmail_ShouldThrow()
    {
        var student = new Student("Anna", "Svensson", "anna@example.com");

        var action = () => student.SetEmail("");

        action.Should().Throw<ArgumentException>();
    }
}