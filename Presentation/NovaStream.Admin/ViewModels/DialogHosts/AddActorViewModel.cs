﻿namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddActorViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    
    public UploadActorModel Actor { get; set; }

    public bool ProcessStarted { get; set; }
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }


    public AddActorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Actor = new();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);

        OpenImageDialogCommand = new RelayCommand(_ => Actor.ImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Actor.Verify();

        if (Actor.HasErrors) return;

        ProcessStarted = true;

        var actor = Actor.Adapt<Actor>();

        var dbActor = _dbContext.Actors.FirstOrDefault(a => a.Name == Actor.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        Actor.ImageUploadSuccess = false; 

        // Actor ImageUrl
        if (dbActor is null || dbActor is not null && dbActor.ImageUrl != Actor.ImageUrl)
        {
            var imageStream = new FileStream(Actor.ImageUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-image{1}", Actor.Name.Replace(' ', '-'), Path.GetExtension(Actor.ImageUrl));
            actor.ImageUrl = string.Format("Images/Actors/{0}", filename);

            Actor.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, actor.ImageUrl, Actor.ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => Actor.ImageUploadSuccess = true);
        }
        else Actor.ImageUploadSuccess = true;

        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbActor is not null) _dbContext.Actors.Remove(dbActor);

        _dbContext.Actors.Add(actor);
        await _dbContext.SaveChangesAsync();

        App.ServiceProvider.GetService<ActorViewModel>().Actors.Add(actor);

        ProcessStarted = false;
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        Actor.ImageProgress.Progress = 0;
        if (Actor.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Actor.ImageUrl);
    }
}
