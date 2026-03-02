namespace Domain.Entities;

public sealed class CourseOccasion
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CourseId { get; private set; }
    public Course Course { get; private set; } = null!;
    public Guid? TeacherId { get; private set; }
    public Teacher? Teacher { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int Capacity { get; private set; }

    private readonly List<Enrollment> _enrollments = [];
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

    private CourseOccasion() { }

    public CourseOccasion(Guid courseId, Guid? teacherId, DateOnly startDate, DateOnly endDate, int capacity)
    {
        if (courseId == Guid.Empty)
            throw new ArgumentException("CourseId is required.");

        if (teacherId == Guid.Empty)
            throw new ArgumentException("TeacherId is invalid.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        if (endDate < startDate)
            throw new ArgumentException("EndDate must be the same or after StartDate.");

        CourseId = courseId;
        TeacherId = teacherId;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
    }

    public void Update(Guid? teacherId, DateOnly startDate, DateOnly endDate, int capacity)
    {
        if (teacherId == Guid.Empty)
            throw new ArgumentException("TeacherId is invalid.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        if (endDate < startDate)
            throw new ArgumentException("EndDate must be the same or after StartDate.");

        TeacherId = teacherId;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
    }
}