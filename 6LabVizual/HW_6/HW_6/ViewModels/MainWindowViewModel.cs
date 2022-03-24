using HW_6.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HW_6.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        bool isEditingExisting = false;
        string title = "";
        string description = "";
        ToDoList current = null;

        public String Title
        {
            get { return title; }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }
        public String Description
        {
            get { return description; }
            set
            {
                this.RaiseAndSetIfChanged(ref description, value);
            }
        }

        DateTimeOffset date = DateTimeOffset.Now.Date;
        public DateTimeOffset Date
        {
            set
            {
                this.RaiseAndSetIfChanged(ref date, value);
                this.ChangeObservableCollection(this.date);
            }
            get
            {
                return this.date;
            }
        }
        public ObservableCollection<ToDoList> Items { get; set; }



        private Dictionary<DateTimeOffset, List<ToDoList>> ListsOnDays;

        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            this.ListsOnDays = new Dictionary<DateTimeOffset, List<ToDoList>>();
            this.Items = new ObservableCollection<ToDoList>();
            this.Content = new FirstViewModel();
        }

        private void InitToDoList()
        {
            var ListsOnDays = new Dictionary<DateTimeOffset, List<ToDoList>>();
            ListsOnDays.Add(this.date, new List<ToDoList>());
            this.ListsOnDays = ListsOnDays;
        }

        public void AppendAction(DateTimeOffset date, ToDoList item)
        {
            if (!this.ListsOnDays.ContainsKey(date))
            {
                this.ListsOnDays.Add(date, new List<ToDoList>());
            }
            this.ListsOnDays[date].Add(item);
            this.ChangeObservableCollection(this.Date);
        }


        public void ChangeView()
        {
            if (this.Content is FirstViewModel)
            {
                this.Content = new SecondViewModel();

            }
            else
            {
                this.Title = "";
                this.Description = "";
                this.current = null;
                this.isEditingExisting = false;
                this.Content = new FirstViewModel();
            }
        }

        public void ChangeObservableCollection(DateTimeOffset date)
        {
            if (!this.ListsOnDays.ContainsKey(date))
            {
                this.Items.Clear();
            }
            else
            {
                this.Items.Clear();
                foreach (var item in this.ListsOnDays[date])
                {
                    this.Items.Add(item);
                }
            }
        }

        public void SaveChanges()
        {
            if (this.Title != "")
            {
                if (this.isEditingExisting)
                {
                    var item = this.ListsOnDays[date].Find(x => x.Equals(this.current));
                    item.Title = this.Title;
                    item.Description = this.Description;
                    this.isEditingExisting = false;
                }
                else
                {
                    this.AppendAction(this.Date, new ToDoList(this.Title, this.Description));
                }
                this.ChangeView();
            }
        }

        public void DeleteItem(ToDoList item)
        {
            this.ListsOnDays[date].Remove(item);
            this.ChangeObservableCollection(date);
        }

        public void ViewExisting(ToDoList item)
        {
            this.isEditingExisting = true;
            this.current = item;
            this.Title = current.Title;
            this.Description = current.Description;
            this.ChangeView();
        }
    }
}
