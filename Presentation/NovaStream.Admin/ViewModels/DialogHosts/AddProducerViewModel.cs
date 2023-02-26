namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddProducerViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public UploadProducerModel Producer { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddProducerViewModel));
    
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

        OpenImageDialogCommand = new RelayCommand(_ => Producer.ImageUrl = FileDialogService.OpenImageFile(Producer.ImageUrl), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            Producer.Verify();

            if (Producer.HasErrors) return;

            ProcessStarted = true;

            var producer = Producer.Adapt<Producer>();

            var dbProducer = _dbContext.Producers.FirstOrDefault(p => p.Id == Producer.Id);

            UploadTasks.Clear();
            UploadTaskTokens.Clear();

            Producer.ImageUploadSuccess = false;

            // Producer ImageUrl
            if (dbProducer is null || dbProducer is not null && dbProducer.ImageUrl != Producer.ImageUrl)
            {
                var imageStream = new FileStream(Producer.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-image-{1}{2}", $"{Producer.Name} {Producer.Surname}".Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Producer.ImageUrl));
                producer.ImageUrl = string.Format("Images/Producers/{0}", filename);

                Producer.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                if (dbProducer is not null) _ = _storageManager.DeleteFileAsync(dbProducer.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, producer.ImageUrl, Producer.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Producer.ImageUploadSuccess = true);
            }
            else Producer.ImageUploadSuccess = true;

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var producerViewModel = App.ServiceProvider.GetService<ProducerViewModel>();

            if (dbProducer is not null)
            {
                var entity = producerViewModel.Producers.FirstOrDefault(p => p.Id == dbProducer.Id);
                _dbContext.Entry(entity).State = EntityState.Detached;

                var index = producerViewModel.Producers.IndexOf(entity);
                producerViewModel.Producers.RemoveAt(index);

                producer.Id = dbProducer.Id;
                _dbContext.Producers.Update(producer);

                producerViewModel.Producers.Insert(index, producer);
            }
            else
            {
                _dbContext.Producers.Add(producer);
                producerViewModel.Producers.Add(producer);
            }

            await _dbContext.SaveChangesAsync();

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Producer saved succesfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            await Cancel();
        }
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        Producer.ImageProgress.Progress = 0;
        if (Producer.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Producer.ImageUrl);
    }
}
