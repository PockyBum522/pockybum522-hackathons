using ApparitionsClientRefactor.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ApparitionsClientRefactor.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IAudioPlayerService _audioPlayerService;
    
    public MainViewModel(
        IAudioPlayerService audioPlayerService
        )
    {
        _audioPlayerService = audioPlayerService;
    }
    
    
}
