using ShopContent_Klimov.Classes;
using ShopContent_Klimov.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopContent_Klimov.Context
{
    public class ItemsContext : Items
    {
        public ItemsContext(bool save = false)
        {
            if (save) Save(true);
            Category = new Categorys();
        }

        public static ObservableCollection<ItemsContext> AllItems()
        {
            ObservableCollection<ItemsContext> allItems = new ObservableCollection<ItemsContext>();
            ObservableCollection<CategorysContext> allCategorys = CategorysContext.AllCategorys();
            SqlConnection connection;
            SqlDataReader dataItems = Connection.Query("SELECT * FROM `Items`", out connection);

            while (dataItems.Read())
            {
                allItems.Add(new ItemsContext()
                {
                    Id = dataItems.GetInt32(0),
                    Name = dataItems.GetString(1),
                    Price = dataItems.GetDouble(2),
                    Description = dataItems.GetString(3),
                    Category = dataItems.IsDBNull(4) ?
                        null :
                        allCategorys.Where(x => x.Id == dataItems.GetInt32(4)).First()
                });
            }
            Connection.CloseConnection(connection);
            return allItems;
        } 

        public void Save(bool New = false)
        {
            SqlConnection connection;

            if (New)
            {
                SqlDataReader dataItems = Connection.Query("INSERT INTO " +
                    "`Items`(" +
                        "Name, " +
                        "Price, " +
                        "Description) " +
                    "VALUES (" +
                        $"'{this.Name}', " +
                        $"{this.Price}, " +
                        $"'{this.Description}');", out connection);
                dataItems.Read();

                this.Id = dataItems.GetInt32(0);
            }
            else
            {
                Connection.Query("UPDATE `Items` " +
                    "SET " +
                        $"Name = '{this.Name}', " +
                        $"Price = {this.Name}, " +
                        $"Description = '{this.Description}', " +
                        $"IdCategory = {this.Category.Id} " +
                    "WHERE " +
                        $"Id = {this.Id}", out connection);
            }
            Connection.CloseConnection(connection);
            MainWindow.init.frame.Navigate(MainWindow.init.Main);
        }

        public void Delete()
        {
            SqlConnection connection;
            Connection.Query("DELETE FROM `Items` " +
                "WHERE " +
                $"Id = {this.Id}", out connection);
            Connection.CloseConnection(connection);
        }

        public RelayCommand OnEdit
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    MainWindow.init.frame.Navigate(new View.Add(this));
                });
            }
        }

        public RelayCommand OnSave
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    
                    Save();
                });
            }
        }

        public RelayCommand OnDelete
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Delete();
                    (MainWindow.init.Main.DataContext as ViewModel.VMItems).Items.Remove(this);
                });
            }
        }
    }
}
