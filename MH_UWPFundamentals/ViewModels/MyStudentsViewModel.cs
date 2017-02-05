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
    internal class MyStudentsViewModel : ViewModelBase
    {
        //declare commands
        private DelegateCommand newCommand;
        private DelegateCommand saveCommand;
        private DelegateCommand deleteCommand;
        private DelegateCommand cancelCommand;

        private bool isDatabaseCreated = false;
        private bool hasSelection = false;
        private ObservableCollection<StudentViewModel> students = new ObservableCollection<StudentViewModel>();
        private StudentViewModel selectedStudent = null;

        //CONSTRUCTOR
        public MyStudentsViewModel()
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
            List<Student> models = DataAccessLayer.GetAllStudents();

            //populate collection with the data from the list
            this.students.Clear();
            foreach (var m in models)
            {
                this.students.Add(new StudentViewModel(m));
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

        //HANDLE SELECTION OF STUDENTS
        public bool HasSelection
        {
            get { return this.hasSelection; }
            private set { this.SetProperty(ref this.hasSelection, value); }
        }

        public ObservableCollection<StudentViewModel> Students
        {
            get { return this.students; }
            set { this.SetProperty(ref this.students, value); }
        }

        public StudentViewModel SelectedStudent
        {
            get { return this.selectedStudent; }
            set
            {
                //allows changes for the selected student
                this.SetProperty(ref this.selectedStudent, value);
                this.HasSelection = this.selectedStudent != null;
                this.deleteCommand.RaiseCanExecuteChanged();
                this.editCommand.RaiseCanExecuteChanged();
            }
        }

        //COMMAND EXECUTED
        protected override void Edit_Executed()
        {
            //switches on edit mode for the selected student
            base.Edit_Executed();
            this.selectedStudent.IsInEditMode = true;

            //enables save and cancel commands
            this.saveCommand.RaiseCanExecuteChanged();
            this.cancelCommand.RaiseCanExecuteChanged();
        }

        private void New_Executed()
        {
            //adds a new student, brings it focus and allows editing
            this.Students.Add(new StudentViewModel(new Student()));
            this.SelectedStudent = this.students.Last();
            this.editCommand.Execute(null);
        }

        private void Save_Executed()
        {
            // Store new one in db
            DataAccessLayer.SaveStudent(this.selectedStudent.Model);

            // Force a property change notification on the ViewModel:
            this.selectedStudent.Model = this.selectedStudent.Model;

            // Leave edit mode
            this.IsInEditMode = false;
            this.selectedStudent.IsInEditMode = false;
        }

        private void Delete_Executed()
        {
            //remove from db
            DataAccessLayer.DeleteStudent(this.selectedStudent.Model);

            //remove from list
            this.Students.Remove(this.selectedStudent);
            this.SelectedStudent = null;
        }

        private void Cancel_Executed()
        {
            //if student doesn't exist
            if (this.selectedStudent.Id == 0)
            {
                //remove from list
                this.students.Remove(this.selectedStudent);
                this.SelectedStudent = null;

                //select last student. 
                if (this.students.Count > 0)
                {
                    this.SelectedStudent = this.Students.Last();
                }
            }
            //if student does exist
            else
            {
                //get old one back from db
                this.selectedStudent.Model = DataAccessLayer.GetStudentById(this.selectedStudent.Id);
                this.selectedStudent.IsInEditMode = false;
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
            return this.selectedStudent != null && base.Edit_CanExecute();
        }

        private bool Save_CanExecute()
        {
            return this.IsInEditMode;
        }
    }
}
