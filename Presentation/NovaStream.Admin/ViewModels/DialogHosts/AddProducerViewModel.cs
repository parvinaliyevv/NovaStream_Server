namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddProducerViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public UploadProducerModel Producer { get; set; }

    public bool ProcessStarted { get; set; }
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }


    public AddProducerViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Producer = new();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);

        OpenImageDialogCommand = new RelayCommand(_ => Producer.ImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Producer.Verify();

        if (Producer.HasErrors) return;

        ProcessStarted = true;

        var producer = Producer.Adapt<Producer>();

        var dbProducer = _dbContext.Producers.FirstOrDefault(p => p.Name == Producer.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        Producer.ImageUploadSuccess = false;

        // Actor ImageUrl
        if (dbProducer is null || dbProducer is not null && dbProducer.ImageUrl != Producer.ImageUrl)
        {
            var imageStream = new FileStream(Producer.ImageUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-image{1}", Producer.Name.Replace(' ', '-'), Path.GetExtension(Producer.ImageUrl));
            producer.ImageUrl = string.Format("Images/Producers/{0}", filename);

            Producer.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, producer.ImageUrl, Producer.ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => Producer.ImageUploadSuccess = true);
        }
        else Producer.ImageUploadSuccess = true;

        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbProducer is not null) _dbContext.Producers.Remove(dbProducer);

        _dbContext.Producers.Add(producer);
        await _dbContext.SaveChangesAsync();

        App.ServiceProvider.GetService<ProducerViewModel>().Producers.Add(producer);

        ProcessStarted = false;
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        Producer.ImageProgress.Progress = 0;
        if (Producer.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Producer.ImageUrl);
    }
}
