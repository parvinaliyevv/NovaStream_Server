namespace NovaStream.Admin.ViewModels;

public class ProducerViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    private int _producerCount;
    public int ProducerCount
    {
        get => _producerCount;
        set { _producerCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Producer> _producers;
    public ObservableCollection<Producer> Producers
    {
        get => _producers;
        set { _producers = value; RaisePropertyChanged(); }
    }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenEditDialogHostCommand { get; set; }
    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public ProducerViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Producers = new ObservableCollection<Producer>(_dbContext.Producers);
        ProducerCount = Producers.Count;

        Producers.CollectionChanged += ProducerCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());


        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var producers = string.IsNullOrWhiteSpace(pattern) ?
        _dbContext.Producers.ToList() :
        _dbContext.Producers.Where(p => (p.Name + " " + p.Surname).Contains(pattern)).ToList();

        if (Producers.Count == producers.Count) return;

        Producers.Clear();

        producers.ForEach(p => Producers.Add(p));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var producer = button?.DataContext as Producer;

        ArgumentNullException.ThrowIfNull(producer);

        _ = MessageBoxService.Show($"Delete <{producer.Name} {producer.Surname}>...", MessageBoxType.Progress);

        await _storageManager.DeleteFileAsync(producer.ImageUrl);

        _dbContext.Producers.Remove(producer);
        await _dbContext.SaveChangesAsync();

        Producers.Remove(producer);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddProducerViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var producer = button?.DataContext as Producer;

        ArgumentNullException.ThrowIfNull(producer);

        var model = App.ServiceProvider.GetService<AddProducerViewModel>();

        model.Producer = producer.Adapt<UploadProducerModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void ProducerCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => ProducerCount = Producers.Count;
}
