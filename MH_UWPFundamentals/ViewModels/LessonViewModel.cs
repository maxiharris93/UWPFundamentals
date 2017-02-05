using MH_UWPFundamentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_UWPFundamentals.ViewModels
{
    internal class LessonViewModel : ViewModelBase
    {
        private Lesson model;

        public LessonViewModel(Lesson model)
        {
            this.model = model;
        }

        public Lesson Model
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

        public DateTime LessonDate
        {
            get
            {
                if (this.model == null)
                {
                    return new DateTime();
                }

                return this.model.LessonDate;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.LessonDate = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Cost
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Cost;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Cost = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Focus
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Focus;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Focus = value;
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
