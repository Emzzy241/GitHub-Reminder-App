using DotNetEnv;    
using GitHubReminderApp.Services;
using Hangfire;
using Hangfire.SQLite;
using SQLitePCL;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Initialize SQLite provider
Batteries.Init(); // Add this line

// Configure Hangfire to use SQLite as the storage provider
builder.Services.AddHangfire(config =>
    config.UseSQLiteStorage("Data Source=hangfire.db;")
);
builder.Services.AddHangfireServer();

// Add email service
builder.Services.AddSingleton<EmailService>();

// Add RecurringJobManager (dependency-injected API for scheduling jobs)
builder.Services.AddSingleton<IRecurringJobManager, RecurringJobManager>();

var app = builder.Build();

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

var emailService = new EmailService();

// Defining common parameters for emails
string toEmail = "emzzyoluwole@gmail.com";
string subject = "GitHub Reminder";
string body = "Don't forget to commit today!, This is your scheduled reminder to update your GitHub streak. To keep up your streak on GitHub, start coding at https://www.github.com/Emzzy241";

// Add endpoints
app.MapGet("/", () => "GitHub Reminder App is running!");

// Nigerian Time schedule (UTC+1):
RecurringJob.AddOrUpdate(
    "Reminder_3am",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 2 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_3:30am",
    () => emailService.SendEmail(toEmail, subject, body),
    "30 2 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_3:40am",
    () => emailService.SendEmail(toEmail, subject, body),
    "40 2 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_4:20am",
    () => emailService.SendEmail(toEmail, subject, body),
    "20 4 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_5am",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 5 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_6am",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 6 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_7am",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 7 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_12pm",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 12 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_3pm",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 15 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_3:30pm",
    () => emailService.SendEmail(toEmail, subject, body),
    "30 15 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_5pm",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 17 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);

RecurringJob.AddOrUpdate(
    "Reminder_7pm",
    () => emailService.SendEmail(toEmail, subject, body),
    "0 19 * * *",
    TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
);




app.MapPost("/schedule", (EmailService emailService, IRecurringJobManager jobManager) =>
{
    jobManager.AddOrUpdate(
        "daily-github-reminder",
        () => emailService.SendEmail("emzzyoluwole@gmail.com", "GitHub Reminder", "Don't forget to commit today!. To keep up your streak on GitHub, start coding at https://www.github.com/Emzzy241"),
        Cron.Daily(7, 0) // Adjust this to your preferred time
    );
    return Results.Ok("Scheduled daily GitHub reminder.");
});

app.MapPost("/test-email", (EmailService emailService) =>
{
    BackgroundJob.Enqueue(() =>
        emailService.SendEmail(
            "dynasty241.d@gmail.com", // Replace with your email
            "GitHub Reminder Test",
            "This is a test email from your GitHub Reminder App!. Bro, ensure you code today, you have a GitHub coding streak to prevent. To keep up your streak on GitHub, start coding at https://www.github.com/Emzzy241")
    );

    return Results.Ok("Test email job created. Check your inbox.");
});

app.Run();
