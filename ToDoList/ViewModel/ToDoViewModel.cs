using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

// Directive for the data model.
using LocalDatabaseSample.Model;


namespace LocalDatabaseSample.ViewModel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private ToDoDataContext toDoDB;

        // Class constructor, create the data context object.
        public ToDoViewModel(string toDoDBConnectionString)
        {
            toDoDB = new ToDoDataContext(toDoDBConnectionString);
        }

        //
        // TODO: Add collections, list, and methods here.
        //

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            toDoDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // All to-do items.
        private ObservableCollection<ToDoItem> _allToDoItems;
        public ObservableCollection<ToDoItem> AllToDoItems
        {
            get { return _allToDoItems; }
            set
            {
                _allToDoItems = value;
                NotifyPropertyChanged("AllToDoItems");
            }
        }

        // To-do items associated with the genk category.
        private ObservableCollection<ToDoItem> _genkToDoItems;
        public ObservableCollection<ToDoItem> GenkToDoItems
        {
            get { return _genkToDoItems; }
            set
            {
                _genkToDoItems = value;
                NotifyPropertyChanged("GenkToDoItems");
            }
        }

        // To-do items associated with the hasselt category.
        private ObservableCollection<ToDoItem> _hasseltToDoItems;
        public ObservableCollection<ToDoItem> HasseltToDoItems
        {
            get { return _hasseltToDoItems; }
            set
            {
                _hasseltToDoItems = value;
                NotifyPropertyChanged("HasseltToDoItems");
            }
        }

        // To-do items associated with the sttruiden category.
        private ObservableCollection<ToDoItem> _stTruidenToDoItems;
        public ObservableCollection<ToDoItem> StTruidenToDoItems
        {
            get { return _stTruidenToDoItems; }
            set
            {
                _stTruidenToDoItems = value;
                NotifyPropertyChanged("StTruidenToDoItems");
            }
        }

	// To-do items associated with the overpelt category.
        private ObservableCollection<ToDoItem> _overpeltToDoItems;
        public ObservableCollection<ToDoItem> OverpeltToDoItems
        {
            get { return _overpeltToDoItems; }
            set
            {
                _overpeltToDoItems = value;
                NotifyPropertyChanged("OverpeltToDoItems");
            }
        }

        // To-do items associated with the hasselt category.
        private ObservableCollection<ToDoItem> _tongerenToDoItems;
        public ObservableCollection<ToDoItem> TongerenToDoItems
        {
            get { return _tongerenToDoItems; }
            set
            {
                _tongerenToDoItems = value;
                NotifyPropertyChanged("TongerenToDoItems");
            }
        }

        // A list of all categories, used by the add task page.
        private List<ToDoCategory> _categoriesList;
        public List<ToDoCategory> CategoriesList
        {
            get { return _categoriesList; }
            set
            {
                _categoriesList = value;
                NotifyPropertyChanged("CategoriesList");
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var toDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                select todo;

            // Query the database and load all to-do items.
            AllToDoItems = new ObservableCollection<ToDoItem>(toDoItemsInDB);

            // Specify the query for all categories in the database.
            var toDoCategoriesInDB = from ToDoCategory category in toDoDB.Categories
                                     select category;


            // Query the database and load all associated items to their respective collections.
            foreach (ToDoCategory category in toDoCategoriesInDB)
            {
                switch (category.Name)
                {
                    case "Genk":
                        GenkToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Hasselt":
                        HasseltToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "StTruiden":
                        StTruidenToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Overpelt":
                        OverpeltToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Tongeren":
                        TongerenToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    default:
                        break;
                }
            }

            // Load a list of all categories.
            CategoriesList = toDoDB.Categories.ToList();

        }

        // Add a to-do item to the database and collections.
        public void AddToDoItem(ToDoItem newToDoItem)
        {
            // Add a to-do item to the data context.
            toDoDB.Items.InsertOnSubmit(newToDoItem);

            // Save changes to the database.
            toDoDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllToDoItems.Add(newToDoItem);

            // Add a to-do item to the appropriate filtered collection.
            switch (newToDoItem.Category.Name)
            {
                case "Genk":
                    GenkToDoItems.Add(newToDoItem);
                    break;
                case "Hasselt":
                    HasseltToDoItems.Add(newToDoItem);
                    break;
                case "StTruiden":
                    StTruidenToDoItems.Add(newToDoItem);
                    break;
                case "Overpelt":
                    OverpeltToDoItems.Add(newToDoItem);
                    break;
                case "Tongeren":
                    TongerenToDoItems.Add(newToDoItem);
                    break;
                default:
                    break;
            }
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(ToDoItem toDoForDelete)
        {

            // Remove the to-do item from the "all" observable collection.
            AllToDoItems.Remove(toDoForDelete);

            // Remove the to-do item from the data context.
            toDoDB.Items.DeleteOnSubmit(toDoForDelete);

            // Remove the to-do item from the appropriate category.   
            switch (toDoForDelete.Category.Name)
            {
                case "Genk":
                    GenkToDoItems.Remove(toDoForDelete);
                    break;
                case "Hasselt":
                    HasseltToDoItems.Remove(toDoForDelete);
                    break;
                case "StTruiden":
                    StTruidenToDoItems.Remove(toDoForDelete);
                    break;
                case "Overpelt":
                    OverpeltToDoItems.Remove(toDoForDelete);
                    break;
                case "Tongeren":
                    TongerenToDoItems.Remove(toDoForDelete);
                    break;
                default:
                    break;
            }

            // Save changes to the database.
            toDoDB.SubmitChanges();
        }
        #endregion
    }
}
