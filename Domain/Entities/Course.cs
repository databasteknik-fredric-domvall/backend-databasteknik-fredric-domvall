namespace Domain.Entities;

public sealed class Course
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Course() { }

    private readonly List<CourseOccasion> _occasions = [];
    public IReadOnlyCollection<CourseOccasion> Occasions => _occasions;

    public Course(string title, string description)
    {
        UpdateDetails(title, description);
    }

    public void UpdateDetails(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Course title is required.");

        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
    }
}