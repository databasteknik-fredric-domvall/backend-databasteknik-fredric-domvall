using Application;
using Domain.Entities;
using Infrastructure;
using System.Globalization;
using Presentation.API.Contracts.CourseOccasions;
using Presentation.API.Contracts.Enrollments;
using Presentation.API.Contracts.Teachers;
using Microsoft.EntityFrameworkCore;
using Presentation.API.Contracts.Students;
using Presentation.API.Contracts.Courses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

#region Courses CRUD Endpoints
app.MapGet("/api/courses", async (CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var courses = await dbContext.Courses
        .AsNoTracking()
        .OrderBy(x => x.Title)
        .Select(x => new CourseResponse(x.Id, x.Title, x.Description))
        .ToListAsync(cancellationToken);

    return Results.Ok(courses);
});

app.MapGet("/api/courses/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var course = await dbContext.Courses
        .AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new CourseResponse(x.Id, x.Title, x.Description))
        .FirstOrDefaultAsync(cancellationToken);

    return course is null ? Results.NotFound() : Results.Ok(course);
});

app.MapPost("/api/courses", async (CreateCourseRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var course = new Course(request.Title, request.Description);

    dbContext.Courses.Add(course);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new CourseResponse(course.Id, course.Title, course.Description);
    return Results.Created($"/api/courses/{course.Id}", response);
});

app.MapPut("/api/courses/{id:guid}", async (Guid id, UpdateCourseRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var course = await dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (course is null)
        return Results.NotFound();

    course.UpdateDetails(request.Title, request.Description);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new CourseResponse(course.Id, course.Title, course.Description);
    return Results.Ok(response);
});

app.MapDelete("/api/courses/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var course = await dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (course is null)
        return Results.NotFound();

    dbContext.Courses.Remove(course);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});
#endregion

#region Students CRUD Endpoints
app.MapGet("/api/students", async (CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var students = await dbContext.Students
        .AsNoTracking()
        .OrderBy(x => x.LastName)
        .ThenBy(x => x.FirstName)
        .Select(x => new StudentResponse(x.Id, x.FirstName, x.LastName, x.Email))
        .ToListAsync(cancellationToken);

    return Results.Ok(students);
});

app.MapGet("/api/students/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var student = await dbContext.Students
        .AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new StudentResponse(x.Id, x.FirstName, x.LastName, x.Email))
        .FirstOrDefaultAsync(cancellationToken);

    return student is null ? Results.NotFound() : Results.Ok(student);
});

app.MapPost("/api/students", async (CreateStudentRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName))
        return Results.BadRequest("FirstName is required.");

    if (string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("LastName is required.");

    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Email is required.");

    var student = new Student(request.FirstName, request.LastName, request.Email);

    dbContext.Students.Add(student);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new StudentResponse(student.Id, student.FirstName, student.LastName, student.Email);
    return Results.Created($"/api/students/{student.Id}", response);
});

app.MapPut("/api/students/{id:guid}", async (Guid id, UpdateStudentRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName))
        return Results.BadRequest("FirstName is required.");

    if (string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("LastName is required.");

    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Email is required.");

    var student = await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (student is null)
        return Results.NotFound();

    student.UpdateName(request.FirstName, request.LastName);
    student.SetEmail(request.Email);

    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new StudentResponse(student.Id, student.FirstName, student.LastName, student.Email);
    return Results.Ok(response);
});

app.MapDelete("/api/students/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var student = await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (student is null)
        return Results.NotFound();

    dbContext.Students.Remove(student);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});
#endregion

#region Teachers CRUD Endpoints

app.MapGet("/api/teachers", async (CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var teachers = await dbContext.Teachers
        .AsNoTracking()
        .OrderBy(x => x.LastName)
        .ThenBy(x => x.FirstName)
        .Select(x => new TeacherResponse(x.Id, x.FirstName, x.LastName, x.Email))
        .ToListAsync(cancellationToken);

    return Results.Ok(teachers);
});

app.MapGet("/api/teachers/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var teacher = await dbContext.Teachers
        .AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new TeacherResponse(x.Id, x.FirstName, x.LastName, x.Email))
        .FirstOrDefaultAsync(cancellationToken);

    return teacher is null ? Results.NotFound() : Results.Ok(teacher);
});

app.MapPost("/api/teachers", async (CreateTeacherRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName))
        return Results.BadRequest("FirstName is required.");

    if (string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("LastName is required.");

    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Email is required.");

    var teacher = new Teacher(request.FirstName, request.LastName, request.Email);

    dbContext.Teachers.Add(teacher);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new TeacherResponse(teacher.Id, teacher.FirstName, teacher.LastName, teacher.Email);
    return Results.Created($"/api/teachers/{teacher.Id}", response);
});

app.MapPut("/api/teachers/{id:guid}", async (Guid id, UpdateTeacherRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName))
        return Results.BadRequest("FirstName is required.");

    if (string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("LastName is required.");

    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Email is required.");

    var teacher = await dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (teacher is null)
        return Results.NotFound();

    teacher.UpdateName(request.FirstName, request.LastName);
    teacher.SetEmail(request.Email);

    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new TeacherResponse(teacher.Id, teacher.FirstName, teacher.LastName, teacher.Email);
    return Results.Ok(response);
});

app.MapDelete("/api/teachers/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var teacher = await dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (teacher is null)
        return Results.NotFound();

    dbContext.Teachers.Remove(teacher);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

#endregion

#region CourseOccasions CRUD Endpoints

static DateOnly ParseDate(string value)
{
    return DateOnly.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
}

app.MapGet("/api/courseoccasions", async (CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var occasions = await dbContext.CourseOccasions
        .AsNoTracking()
        .OrderBy(x => x.StartDate)
        .Select(x => new CourseOccasionResponse(
            x.Id,
            x.CourseId,
            x.StartDate.ToString("yyyy-MM-dd"),
            x.EndDate.ToString("yyyy-MM-dd"),
            x.Capacity))
        .ToListAsync(cancellationToken);

    return Results.Ok(occasions);
});

app.MapGet("/api/courseoccasions/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var occasion = await dbContext.CourseOccasions
        .AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new CourseOccasionResponse(
            x.Id,
            x.CourseId,
            x.StartDate.ToString("yyyy-MM-dd"),
            x.EndDate.ToString("yyyy-MM-dd"),
            x.Capacity))
        .FirstOrDefaultAsync(cancellationToken);

    return occasion is null ? Results.NotFound() : Results.Ok(occasion);
});

app.MapPost("/api/courseoccasions", async (CreateCourseOccasionRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (request.CourseId == Guid.Empty)
        return Results.BadRequest("CourseId is required.");

    if (request.Capacity <= 0)
        return Results.BadRequest("Capacity must be greater than zero.");

    var courseExists = await dbContext.Courses.AnyAsync(x => x.Id == request.CourseId, cancellationToken);
    if (!courseExists)
        return Results.BadRequest("Course does not exist.");

    DateOnly startDate;
    DateOnly endDate;

    try
    {
        startDate = ParseDate(request.StartDate);
        endDate = ParseDate(request.EndDate);
    }
    catch
    {
        return Results.BadRequest("Dates must be formatted as yyyy-MM-dd.");
    }

    var occasion = new CourseOccasion(request.CourseId, startDate, endDate, request.Capacity);

    dbContext.CourseOccasions.Add(occasion);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new CourseOccasionResponse(
        occasion.Id,
        occasion.CourseId,
        occasion.StartDate.ToString("yyyy-MM-dd"),
        occasion.EndDate.ToString("yyyy-MM-dd"),
        occasion.Capacity);

    return Results.Created($"/api/courseoccasions/{occasion.Id}", response);
});

app.MapPut("/api/courseoccasions/{id:guid}", async (Guid id, UpdateCourseOccasionRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (request.Capacity <= 0)
        return Results.BadRequest("Capacity must be greater than zero.");

    DateOnly startDate;
    DateOnly endDate;

    try
    {
        startDate = ParseDate(request.StartDate);
        endDate = ParseDate(request.EndDate);
    }
    catch
    {
        return Results.BadRequest("Dates must be formatted as yyyy-MM-dd.");
    }

    var occasion = await dbContext.CourseOccasions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (occasion is null)
        return Results.NotFound();

    occasion.Update(startDate, endDate, request.Capacity);

    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new CourseOccasionResponse(
        occasion.Id,
        occasion.CourseId,
        occasion.StartDate.ToString("yyyy-MM-dd"),
        occasion.EndDate.ToString("yyyy-MM-dd"),
        occasion.Capacity);

    return Results.Ok(response);
});

app.MapDelete("/api/courseoccasions/{id:guid}", async (Guid id, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var occasion = await dbContext.CourseOccasions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (occasion is null)
        return Results.NotFound();

    dbContext.CourseOccasions.Remove(occasion);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

#endregion

#region Enrollments CRUD Endpoints

app.MapGet("/api/enrollments", async (CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var enrollments = await dbContext.Enrollments
        .AsNoTracking()
        .OrderByDescending(x => x.CreatedAtUtc)
        .Select(x => new EnrollmentResponse(x.StudentId, x.CourseOccasionId, x.CreatedAtUtc))
        .ToListAsync(cancellationToken);

    return Results.Ok(enrollments);
});

app.MapPost("/api/enrollments", async (CreateEnrollmentRequest request, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    if (request.StudentId == Guid.Empty)
        return Results.BadRequest("StudentId is required.");

    if (request.CourseOccasionId == Guid.Empty)
        return Results.BadRequest("CourseOccasionId is required.");

    var studentExists = await dbContext.Students.AnyAsync(x => x.Id == request.StudentId, cancellationToken);
    if (!studentExists)
        return Results.BadRequest("Student does not exist.");

    var occasionExists = await dbContext.CourseOccasions.AnyAsync(x => x.Id == request.CourseOccasionId, cancellationToken);
    if (!occasionExists)
        return Results.BadRequest("CourseOccasion does not exist.");

    var alreadyEnrolled = await dbContext.Enrollments.AnyAsync(
        x => x.StudentId == request.StudentId && x.CourseOccasionId == request.CourseOccasionId,
        cancellationToken);

    if (alreadyEnrolled)
        return Results.BadRequest("Student is already enrolled for this course occasion.");

    var enrollment = new Enrollment(request.StudentId, request.CourseOccasionId);

    dbContext.Enrollments.Add(enrollment);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new EnrollmentResponse(enrollment.StudentId, enrollment.CourseOccasionId, enrollment.CreatedAtUtc);
    return Results.Created("/api/enrollments", response);
});

app.MapDelete("/api/enrollments", async (Guid studentId, Guid courseOccasionId, CourseOnlineDbContext dbContext, CancellationToken cancellationToken) =>
{
    var enrollment = await dbContext.Enrollments.FirstOrDefaultAsync(
        x => x.StudentId == studentId && x.CourseOccasionId == courseOccasionId,
        cancellationToken);

    if (enrollment is null)
        return Results.NotFound();

    dbContext.Enrollments.Remove(enrollment);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

#endregion

app.Run();