using MySql.Data.MySqlClient;
using ShopContent_Klimov.Classes;
using ShopContent_Klimov.Model;
using ShopContent_Klimov.ViewModel;
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
            Category = new Categorys();
        }

        public static ObservableCollection<ItemsContext> AllItems()
        {
            ObservableCollection<ItemsContext> allItems = new ObservableCollection<ItemsContext>();
            ObservableCollection<CategorysContext> allCategorys = CategorysContext.AllCategorys();
            MySqlConnection connection;
            MySqlDataReader dataItems = Connection.Query("SELECT * FROM `ShopContent`.`Items`", out connection);

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
            MySqlConnection connection;

            if (New)
            {
                MySqlDataReader dataItems = Connection.Query("INSERT INTO " +
                    "`ShopContent`.`Items` (" +
                        "Name, " +
                        "Price, " +
                        "Description, " +
                        "IdCategory) " +
                    "VALUES (" +
                        $"'{this.Name}', " +
                        $"{this.Price}, " +
                        $"'{this.Description}', " +
                        $"{this.Category.Id}); " +
                    "SELECT LAST_INSERT_ID();", out connection);
                
                dataItems.Read();
                this.Id = dataItems.GetInt32(0);
            }
            else
            {
                Connection.Query("UPDATE `ShopContent`.`Items` " +
                    "SET " +
                        $"Name = '{this.Name}', " +
                        $"Price = {this.Price}, " +
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
            MySqlConnection connection;
            Connection.Query("DELETE FROM `ShopContent`.`Items` " +
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
                    MainWindow.init.Main.DataContext = new VMItems();
                });
            }
        }

        public RelayCommand OnSave
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Category = CategorysContext.AllCategorys().Where(x => x.Id == this.Category.Id).First();
                    Save();
                    MainWindow.init.Main.DataContext = new VMItems();
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
