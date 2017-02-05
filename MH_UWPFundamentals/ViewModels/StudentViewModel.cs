using MH_UWPFundamentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_UWPFundamentals.ViewModels
{
    internal class StudentViewModel : ViewModelBase
    {
        private Student model;

        public StudentViewModel(Student model)
        {
            this.model = model;
        }

        public Student Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.SetProperty(ref this.model, value);
                this.OnPropertyChanged(string.Empty);
            }
        }

        public int Id
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Id;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Id = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Name;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Number
        {
            get
            {
                if (this.model == null)
                {
                    return String.Empty;
                }

                return this.model.Number;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Number = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Notes;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Notes = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
