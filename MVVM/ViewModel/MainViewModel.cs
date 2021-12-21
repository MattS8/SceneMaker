using Scene_Maker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RoutineCreatoreViewModel RoutineCreatoreVM { get; set; }
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
            RoutineCreatoreVM = new RoutineCreatoreViewModel();
            AllRoutinesOptionsVM = new AllRoutinesOptionsViewModel(this);
            RoutineCreatorOptionsVM = new RoutineCreatorOptionsViewModel();

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
                CurrentView = RoutineCreatoreVM;
                CurrentViewOptions = RoutineCreatorOptionsVM;
                InnerNavView = BackToRoutinesVM;
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
