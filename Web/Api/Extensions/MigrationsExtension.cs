using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Modules.Notifications.Infrastructure.Data;
using Modules.Trips.Infrastructure.Data;
using Modules.Users.Infrastructure.Data;

namespace Api.Extensions;

public static class MigrationsExtension
{
    public static async Task AddMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider
            .GetRequiredService<UsersDbContext>();
        var notificationsDbContext = scope.ServiceProvider
            .GetRequiredService<NotificationDbContext>();
        var tripsDbContext = scope.ServiceProvider
            .GetRequiredService<TripsDbContext>();
        usersDbContext.Database.Migrate();
        notificationsDbContext.Database.Migrate();
        tripsDbContext.Database.Migrate();
        var minioClient = scope.ServiceProvider.GetRequiredService<IMinioClient>();
        await CreateBucketWithPoliciesAsync(minioClient, "uploads");
    }

    public static async Task CreateBucketWithPoliciesAsync(IMinioClient _minio, string bucketName)
    {
        bool exists = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName));

        if (!exists)
        {
            await _minio.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName));

            var policyJson = GeneratePolicy(bucketName);
            await _minio.SetPolicyAsync(
                new SetPolicyArgs()
                    .WithBucket(bucketName)
                    .WithPolicy(policyJson));
        }
    }

    private static string GeneratePolicy(string bucketName)
    {
        var policy = new
        {
            Version = "2012-10-17",
            Statement = new[]
            {
                new
                {
                    Effect = "Allow",
                    Principal = new { AWS = "*" },
                    Action = new[] { "s3:GetObject" },
                    Resource = new[]
                    {
                        $"arn:aws:s3:::{bucketName}/*"
                    }
                }
            }
        };

        return JsonSerializer.Serialize(policy);
    }
}
