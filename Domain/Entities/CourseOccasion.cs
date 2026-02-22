namespace Domain.Entities;

public sealed class CourseOccasion
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int Capacity { get; private set; }

    private CourseOccasion() { }

    public CourseOccasion(Guid courseId, DateOnly startDate, DateOnly endDate, int capacity)
    {
        if (courseId == Guid.Empty)
            throw new ArgumentException("CourseId is required.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        if (endDate < startDate)
            throw new ArgumentException("EndDate must be the same or after StartDate.");

        CourseId = courseId;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
    }
}