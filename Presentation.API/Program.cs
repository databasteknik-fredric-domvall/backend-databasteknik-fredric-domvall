using Application;
using Domain.Entities;
using Infrastructure;
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

app.Run();