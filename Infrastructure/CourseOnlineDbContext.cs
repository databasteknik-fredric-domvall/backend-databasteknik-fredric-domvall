using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class CourseOnlineDbContext(DbContextOptions<CourseOnlineDbContext> options)
    : DbContext(options)
{
}