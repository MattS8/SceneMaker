using Microsoft.Win32;
using Scene_Maker.Core;

namespace Scene_Maker.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand SceneSettingsCommand { get; set; }
        public RelayCommand SceneCreationCommand { get; set; }

        public EmptyNavOptionsViewModel EmptyNavOptionsVM { get; set; }
        public SimpleNavOptionsViewModel BackToRoutinesVM { get; set; }

        #region All Routines
        public RelayCommand AllRoutinesCommand { get; set; }
        public RelayCommand BackToRoutinesCommand { get; set; }
        public RelayCommand NewRoutineCommand { get; set; }
        public RelayCommand NewMarkerCommand { get; set; }
        public RelayCommand CreateRoutineCommand { get; set; }

        public AllRoutinesViewModel AllRoutinesVM { get; set; }
        public AllRoutinesOptionsViewModel AllRoutinesOptionsVM { get; set; }
        public RoutineCreatorViewModel RoutineCreatorVM { get; set; }
        public RoutineCreatorOptionsViewModel RoutineCreatorOptionsVM { get; set; }
        #endregion

        public SceneSettingsViewModel SceneSettingsVM { get; set;}
        public SceneSettingsOptionsViewModel SceneSettingsOptionsVM { get; set; }
        public SceneCreationViewModel CreateSceneVM { get; set;}
        public SceneCreationOptionsViewModel CreateSceneOptionsVM { get; set; }

        private object _currentView;
        private object _currentViewOptions;
        private object _innerNavView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public object CurrentViewOptions
        {
            get { return _currentViewOptions; }
            set
            {
                _currentViewOptions = value;
                OnPropertyChanged();
            }
        }

        public object InnerNavView
        {
            get { return _innerNavView; }
            set
            {
                _innerNavView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            AllRoutinesVM = new AllRoutinesViewModel();
            AllRoutinesOptionsVM = new AllRoutinesOptionsViewModel(this);

            SceneSettingsVM = new SceneSettingsViewModel();
            SceneSettingsOptionsVM = new SceneSettingsOptionsViewModel();

            CreateSceneVM = new SceneCreationViewModel();
            CreateSceneOptionsVM = new SceneCreationOptionsViewModel();

            CurrentView = AllRoutinesVM;
            CurrentViewOptions = AllRoutinesOptionsVM;

            #region Top View Commands
            AllRoutinesCommand = new RelayCommand(o => 
            { 
                CurrentView = AllRoutinesVM;
                CurrentViewOptions = AllRoutinesOptionsVM;
                InnerNavView = EmptyNavOptionsVM;
            });

            SceneSettingsCommand = new RelayCommand(o =>
            { 
                CurrentView = SceneSettingsVM;
                CurrentViewOptions = SceneSettingsOptionsVM;
                InnerNavView = EmptyNavOptionsVM;
            });

            SceneCreationCommand = new RelayCommand(o =>
            {
                CurrentView = CreateSceneVM;
                CurrentViewOptions = CreateSceneOptionsVM;
                InnerNavView = EmptyNavOptionsVM;
            });
            #endregion

            #region Sub View Commands
            NewRoutineCommand = new RelayCommand(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Supported Files (*.wav;*.mp3)|*.wav;*.mp3|All Files (*.*)|*.*";
                bool? result = openFileDialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    RoutineCreatorVM = new RoutineCreatorViewModel();
                    CurrentView = RoutineCreatorVM;
                    CurrentViewOptions = new RoutineCreatorOptionsViewModel();
                    InnerNavView = BackToRoutinesVM;
                    RoutineCreatorVM.Load(openFileDialog.FileName);
                }
            });
            BackToRoutinesCommand = new RelayCommand(o =>
            {
                CurrentView = AllRoutinesVM;
                CurrentViewOptions = AllRoutinesOptionsVM;
                InnerNavView = EmptyNavOptionsVM;
            });
            BackToRoutinesVM = new SimpleNavOptionsViewModel(BackToRoutinesCommand);
            #endregion  
        }
    }
}
