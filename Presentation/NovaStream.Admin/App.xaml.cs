﻿namespace NovaStream.Admin;

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
        Services.AddTransient<AddMovieViewModel>();

        Services.AddSingleton<SoonViewModel>();
        Services.AddTransient<AddSoonViewModel>();

        Services.AddSingleton<SerialViewModel>();
        Services.AddTransient<AddSerialViewModel>();

        Services.AddSingleton<SeasonViewModel>();
        Services.AddTransient<AddSeasonViewModel>();

        Services.AddSingleton<EpisodeViewModel>();
        Services.AddTransient<AddEpisodeViewModel>();

        Services.AddSingleton<ActorViewModel>();
        Services.AddTransient<AddActorViewModel>();

        Services.AddSingleton<ProducerViewModel>();
        Services.AddTransient<AddProducerViewModel>();

        Services.AddSingleton<GenreViewModel>();
        Services.AddTransient<AddGenreViewModel>();

        Services.PersistenceRegister(_Configuration);
        Services.InfrastructureRegister(_Configuration);

        Mapster();

        ServiceProvider = Services.BuildServiceProvider();
    }

    private void Mapster()
    {
        TypeAdapterConfig<UploadMovieModel, Movie>.NewConfig()
            .Map(dest => dest.ProducerId, src => src.Producer.Id)
            .Ignore(dest => dest.Producer);

        TypeAdapterConfig<Movie, UploadMovieModel>.NewConfig()
            .Ignore(dest => dest.Producer);

        TypeAdapterConfig<UploadSerialModel, Serial>.NewConfig()
            .Map(dest => dest.ProducerId, src => src.Producer.Id)
            .Ignore(dest => dest.Producer);

        TypeAdapterConfig<Serial, UploadSerialModel>.NewConfig()
            .Ignore(dest => dest.Producer);

        TypeAdapterConfig<UploadSeasonModel, Season>.NewConfig()
            .Map(dest => dest.SerialName, src => src.Serial.Name)
            .Ignore(dest => dest.Serial);

        TypeAdapterConfig<Season, UploadSeasonModel>.NewConfig()
            .Ignore(dest => dest.Serial);

        TypeAdapterConfig<UploadEpisodeModel, Episode>.NewConfig()
            .Map(dest => dest.SeasonId, src => src.Season.Id)
            .Ignore(dest => dest.Season);

        TypeAdapterConfig<Episode, UploadEpisodeModel>.NewConfig()
            .Ignore(dest => dest.Serial)
            .Ignore(dest => dest.Season);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Register();

        new MainView().ShowDialog();

        base.OnStartup(e);
    }
}
