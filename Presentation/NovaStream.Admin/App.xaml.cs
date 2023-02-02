namespace NovaStream.Admin;

public partial class App : System.Windows.Application
{
    public static IServiceCollection Services { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }
    private static IConfiguration _Configuration { get; set; }


    static App()
    {
        _Configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
    }


    private void Register()
    {
        Services = new ServiceCollection();

        Services.AddSingleton<IMessenger, Messenger>();
        Services.AddSingleton<INavigationService, NavigationService>();

        Services.AddSingleton<MainViewModel>();

        Services.AddSingleton<MovieViewModel>();
        Services.AddSingleton<AddMovieViewModel>();

        Services.AddSingleton<SoonViewModel>();
        Services.AddSingleton<AddSoonViewModel>();

        Services.AddSingleton<SerialViewModel>();
        Services.AddSingleton<AddSerialViewModel>();

        Services.AddSingleton<SeasonViewModel>();
        Services.AddSingleton<AddSeasonViewModel>();

        Services.AddSingleton<EpisodeViewModel>();
        Services.AddSingleton<AddEpisodeViewModel>();

        Services.AddSingleton<ActorViewModel>();
        Services.AddSingleton<AddActorViewModel>();

        Services.AddSingleton<ProducerViewModel>();
        Services.AddSingleton<AddProducerViewModel>();

        Services.AddSingleton<GenreViewModel>();
        Services.AddSingleton<AddGenreViewModel>();

        Services.PersistenceRegister(_Configuration);
        Services.InfrastructureRegister(_Configuration);

        ServiceProvider = Services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Register();

        new MainView().ShowDialog();

        base.OnStartup(e);
    }
}
