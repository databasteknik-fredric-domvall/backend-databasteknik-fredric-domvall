namespace Domain.Entities;

public sealed class Teacher
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private readonly List<CourseOccasion> _courseOccasions = [];
    public IReadOnlyCollection<CourseOccasion> CourseOccasions => _courseOccasions;

    private Teacher() { }

    public Teacher(string firstName, string lastName, string email)
    {
        UpdateName(firstName, lastName);
        SetEmail(email);
    }

    public void UpdateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("LastName is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        Email = email.Trim();
    }
}