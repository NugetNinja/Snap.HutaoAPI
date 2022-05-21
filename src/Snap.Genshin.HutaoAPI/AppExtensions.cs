namespace Snap.HutaoAPI;

public static class AppExtensions
{
    public static WebApplicationBuilder ExitOnWrongEnvironment(this WebApplicationBuilder builder)
    {
#if DEBUG
        if (!builder.Environment.IsDevelopment())
        {
            Console.WriteLine("panic: only in Development envirenment can you run DEBUG verison.");
            Environment.Exit(Environment.ExitCode);
        }
#endif
#if RELEASE
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("panic: only in Production envirenment can you run RELEASE verison.");
            Environment.Exit(Environment.ExitCode);
        }
#endif
        return builder;
    }
}
