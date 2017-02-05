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

namespace MH_UWPFundamentals.ViewModels
{
    internal class MyLessonsViewModel : ViewModelBase
    {
        //declare commands
        private DelegateCommand newCommand;
        private DelegateCommand saveCommand;
        private DelegateCommand deleteCommand;
        private DelegateCommand cancelCommand;

        private bool isDatabaseCreated = false;
        private bool hasSelection = false;
        private ObservableCollection<LessonViewModel> lessons = new ObservableCollection<LessonViewModel>();
        private LessonViewModel selectedLesson = null;

        //CONSTRUCTOR
        public MyLessonsViewModel()
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
            List<Lesson> models = DataAccessLayer.GetAllLessons();

            //populate collection with the data from the list
            this.lessons.Clear();
            foreach (var m in models)
            {
                this.lessons.Add(new LessonViewModel(m));
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

        //HANDLE SELECTION OF LESSONS
        public bool HasSelection
        {
            get { return this.hasSelection; }
            private set { this.SetProperty(ref this.hasSelection, value); }
        }

        public ObservableCollection<LessonViewModel> Lessons
        {
            get { return this.lessons; }
            set { this.SetProperty(ref this.lessons, value); }
        }

        public LessonViewModel SelectedLesson
        {
            get { return this.selectedLesson; }
            set
            {
                //allows changes for the selected lesson
                this.SetProperty(ref this.selectedLesson, value);
                this.HasSelection = this.selectedLesson != null;
                this.deleteCommand.RaiseCanExecuteChanged();
                this.editCommand.RaiseCanExecuteChanged();
            }
        }

        //COMMAND EXECUTED
        protected override void Edit_Executed()
        {
            //switches on edit mode for the selected lesson
            base.Edit_Executed();
            this.selectedLesson.IsInEditMode = true;

            //enables save and cancel commands
            this.saveCommand.RaiseCanExecuteChanged();
            this.cancelCommand.RaiseCanExecuteChanged();
        }

        private void New_Executed()
        {
            //adds a new lesson, brings it focus and allows editing
            this.Lessons.Add(new LessonViewModel(new Lesson()));
            this.SelectedLesson = this.lessons.Last();
            this.editCommand.Execute(null);
        }

        private void Save_Executed()
        {
            // Store new one in db
            DataAccessLayer.SaveLesson(this.selectedLesson.Model);

            // Force a property change notification on the ViewModel:
            this.selectedLesson.Model = this.selectedLesson.Model;

            // Leave edit mode
            this.IsInEditMode = false;
            this.selectedLesson.IsInEditMode = false;
        }

        private void Delete_Executed()
        {
            //remove from db
            DataAccessLayer.DeleteLesson(this.selectedLesson.Model);

            //remove from list
            this.Lessons.Remove(this.selectedLesson);
            this.SelectedLesson = null;
        }

        private void Cancel_Executed()
        {
            //if lesson doesn't exist
            if (this.selectedLesson.Id == 0)
            {
                //remove from list
                this.lessons.Remove(this.selectedLesson);
                this.SelectedLesson = null;

                //select last lesson. 
                if (this.lessons.Count > 0)
                {
                    this.SelectedLesson = this.Lessons.Last();
                }
            }
            //if lesson does exist
            else
            {
                //get old one back from db
                this.selectedLesson.Model = DataAccessLayer.GetLessonById(this.selectedLesson.Id);
                this.selectedLesson.IsInEditMode = false;
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
            return this.selectedLesson != null && base.Edit_CanExecute();
        }

        private bool Save_CanExecute()
        {
            return this.IsInEditMode;
        }
    }
}
