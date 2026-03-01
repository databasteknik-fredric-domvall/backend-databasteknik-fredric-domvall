namespace Domain.Entities;

public sealed class Enrollment
{
    public Guid StudentId { get; private set; }
    public Student Student { get; private set; } = null!;

    public Guid CourseOccasionId { get; private set; }
    public CourseOccasion CourseOccasion { get; private set; } = null!;

    public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    private Enrollment() { }

    public Enrollment(Guid studentId, Guid courseOccasionId)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("StudentId is required.");

        if (courseOccasionId == Guid.Empty)
            throw new ArgumentException("CourseOccasionId is required.");

        StudentId = studentId;
        CourseOccasionId = courseOccasionId;
    }
}