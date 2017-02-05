using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MH_UWPFundamentals.Data;
using MH_UWPFundamentals.Mvvm;
using MH_UWPFundamentals.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MH_UWPFundamentals.ViewModels
{
    internal class MyHorsesViewModel : ViewModelBase
    {
        //declare commands
        private DelegateCommand newCommand;
        private DelegateCommand saveCommand;
        private DelegateCommand deleteCommand;
        private DelegateCommand cancelCommand;

        private bool isDatabaseCreated = false;
        private bool hasSelection = false;
        private ObservableCollection<HorseViewModel> horses = new ObservableCollection<HorseViewModel>();
        private HorseViewModel selectedHorse = null;

        //CONSTRUCTOR
        public MyHorsesViewModel()
        {
            if (this.IsInDesignMode)
            {
                return;
            }

            //create commands
            this.newCommand = new DelegateCommand(this.New_Executed, this.New_CanExecute);
            this.saveCommand = new DelegateCommand(this.Save_Executed, this.Save_CanExecute);
            this.deleteCommand = new DelegateCommand(this.Delete_Executed, this.Edit_CanExecute);
            this.cancelCommand = new DelegateCommand(this.Cancel_Executed, this.Save_CanExecute);

            //retrieve from database and put into a list
            List<Horse> models = DataAccessLayer.GetAllHorses();

            //populate collection with the data from the list
            this.horses.Clear();
            foreach (var m in models)
            {
                this.horses.Add(new HorseViewModel(m));
            }

            this.isDatabaseCreated = true;
            this.newCommand.RaiseCanExecuteChanged();
        }

        //assign commands
        public ICommand NewCommand
        {
            get { return this.newCommand; }
        }
        public ICommand SaveCommand
        {
            get { return this.saveCommand; }
        }
        public ICommand DeleteCommand
        {
            get { return this.deleteCommand; }
        }
        public ICommand CancelCommand
        {
            get { return this.cancelCommand; }
        }

        //HANDLE SELECTION OF HORSES
        public bool HasSelection
        {
            get { return this.hasSelection; }
            private set { this.SetProperty(ref this.hasSelection, value); }
        }

        public ObservableCollection<HorseViewModel> Horses
        {
            get { return this.horses; }
            set { this.SetProperty(ref this.horses, value); }
        }

        public HorseViewModel SelectedHorse
        {
            get { return this.selectedHorse; }
            set
            {
                //allows changes for the selected horse
                this.SetProperty(ref this.selectedHorse, value);
                this.HasSelection = this.selectedHorse != null;
                this.deleteCommand.RaiseCanExecuteChanged();
                this.editCommand.RaiseCanExecuteChanged();
            }
        }

        //COMMAND EXECUTED
        protected override void Edit_Executed()
        {
            //switches on edit mode for the selected horse
            base.Edit_Executed();
            this.selectedHorse.IsInEditMode = true;

            //enables save and cancel commands
            this.saveCommand.RaiseCanExecuteChanged();
            this.cancelCommand.RaiseCanExecuteChanged();
        }

        private void New_Executed()
        {
            //adds a new horse, brings it focus and allows editing
            this.Horses.Add(new HorseViewModel(new Horse()));
            this.SelectedHorse = this.horses.Last();
            this.editCommand.Execute(null);
        }

        private void Save_Executed()
        {
            // Store new one in db
            DataAccessLayer.SaveHorse(this.selectedHorse.Model);

            // Force a property change notification on the ViewModel:
            this.selectedHorse.Model = this.selectedHorse.Model;

            // Leave edit mode
            this.IsInEditMode = false;
            this.selectedHorse.IsInEditMode = false;
        }

        private void Delete_Executed()
        {
            //remove from db
            DataAccessLayer.DeleteHorse(this.selectedHorse.Model);

            //remove from list
            this.Horses.Remove(this.selectedHorse);
            this.SelectedHorse = null;
        }

        private void Cancel_Executed()
        {
            //if horse doesn't exist
            if (this.selectedHorse.Id == 0)
            {
                //remove from list
                this.horses.Remove(this.selectedHorse);
                this.SelectedHorse = null;

                //select last horse. 
                if (this.horses.Count > 0)
                {
                    this.SelectedHorse = this.Horses.Last();
                }
            }
            //if horse does exist
            else
            {
                //get old one back from db
                this.selectedHorse.Model = DataAccessLayer.GetHorseById(this.selectedHorse.Id);
                this.selectedHorse.IsInEditMode = false;
            }
            
            //disable editing
            this.IsInEditMode = false;
        }

        //CAN THESE COMMANDS EXECUTE?
        private bool New_CanExecute()
        {
            return !this.IsInEditMode && this.isDatabaseCreated;
        }

        protected override bool Edit_CanExecute()
        {
            return this.selectedHorse != null && base.Edit_CanExecute();
        }

        private bool Save_CanExecute()
        {
            return this.IsInEditMode;
        }
    }
}
