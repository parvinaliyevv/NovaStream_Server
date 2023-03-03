namespace NovaStream.Admin;

public partial class App : System.Windows.Application
{
    public static IServiceCollection Services { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }
    private static IConfiguration _Configuration { get; set; }


    static App()
    {
        _Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
    }


    private void Register()
    {
        Services = new ServiceCollection();

        Services.AddSingleton<IMessenger, Messenger>();
        Services.AddSingleton<INavigationService, NavigationService>();

        Services.AddSingleton<MainViewModel>();
        Services.AddTransient<LoginViewModel>();

        Services.AddSingleton<MovieViewModel>();
        Services.AddTransient<AddMovieViewModel>();

        Services.AddSingleton<MovieActorViewModel>();
        Services.AddTransient<AddMovieActorViewModel>();

        Services.AddSingleton<MovieGenreViewModel>();
        Services.AddTransient<AddMovieGenreViewModel>();

        Services.AddSingleton<SoonViewModel>();
        Services.AddTransient<AddSoonViewModel>();

        Services.AddSingleton<SoonGenreViewModel>();
        Services.AddTransient<AddSoonGenreViewModel>();

        Services.AddSingleton<SerialViewModel>();
        Services.AddTransient<AddSerialViewModel>();

        Services.AddSingleton<SerialActorViewModel>();
        Services.AddTransient<AddSerialActorViewModel>();

        Services.AddSingleton<SerialGenreViewModel>();
        Services.AddTransient<AddSerialGenreViewModel>();

        Services.AddSingleton<SeasonViewModel>();
        Services.AddTransient<AddSeasonViewModel>();

        Services.AddSingleton<EpisodeViewModel>();
        Services.AddTransient<AddEpisodeViewModel>();
        Services.AddTransient<EditEpisodeViewModel>();

        Services.AddSingleton<ActorViewModel>();
        Services.AddTransient<AddActorViewModel>();

        Services.AddSingleton<DirectorViewModel>();
        Services.AddTransient<AddDirectorViewModel>();

        Services.AddSingleton<GenreViewModel>();
        Services.AddTransient<AddGenreViewModel>();

        Services.PersistenceRegister(_Configuration);
        Services.InfrastructureRegister(_Configuration);

        Mapster();

        ServiceProvider = Services.BuildServiceProvider();
    }

    private void Mapster()
    {
        TypeAdapterConfig<MovieViewModelContent, Movie>.NewConfig()
            .Map(dest => dest.DirectorId, src => src.Director.Id)
            .Ignore(dest => dest.Director);

        TypeAdapterConfig<Movie, MovieViewModelContent>.NewConfig()
            .Ignore(dest => dest.Director);

        TypeAdapterConfig<SerialViewModelContent, Serial>.NewConfig()
            .Map(dest => dest.DirectorId, src => src.Director.Id)
            .Ignore(dest => dest.Director);

        TypeAdapterConfig<Serial, SerialViewModelContent>.NewConfig()
            .Ignore(dest => dest.Director);

        TypeAdapterConfig<SeasonViewModelContent, Season>.NewConfig()
            .Map(dest => dest.SerialName, src => src.Serial.Name)
            .Ignore(dest => dest.Serial);

        TypeAdapterConfig<Season, SeasonViewModelContent>.NewConfig()
            .Ignore(dest => dest.Serial);

        TypeAdapterConfig<EpisodeViewModelContent, Episode>.NewConfig()
            .Map(dest => dest.SeasonId, src => src.Season.Id)
            .Ignore(dest => dest.Season);

        TypeAdapterConfig<Episode, EpisodeViewModelContent>.NewConfig()
            .Ignore(dest => dest.Serial)
            .Ignore(dest => dest.Season);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Register();

        new LoginView().ShowDialog();

        base.OnStartup(e);
    }
}
