namespace PowerShellUI1
{
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");
        }
    }
}
