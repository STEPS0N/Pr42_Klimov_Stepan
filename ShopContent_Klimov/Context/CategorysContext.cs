using MySql.Data.MySqlClient;
using ShopContent_Klimov.Classes;
using ShopContent_Klimov.Model;
using ShopContent_Klimov.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopContent_Klimov.Context
{
    public class CategorysContext : Categorys
    {
        public static ObservableCollection<CategorysContext> AllCategorys()
        {
            ObservableCollection<CategorysContext> allCategorys = new ObservableCollection<CategorysContext>();
            MySqlConnection connection;
            MySqlDataReader dataCategory = Connection.Query("SELECT * FROM `ShopContent`.`Categorys`", out connection);

            while (dataCategory.Read())
            {
                allCategorys.Add(new CategorysContext()
                {
                    Id = dataCategory.GetInt32(0),
                    Name = dataCategory.GetString(1)
                });
            }
            Connection.CloseConnection(connection);
            return allCategorys;
        }

        public void Save(bool New = false)
        {
            MySqlConnection connection;

            if (New)
            {
                MySqlDataReader dataCategorys = Connection.Query("INSERT INTO " +
                    "`ShopContent`.`Categorys` (" +
                        "Name) " +
                    "VALUES (" +
                        $"'{this.Name}'); " +
                    "SELECT LAST_INSERT_ID();", out connection);

                dataCategorys.Read();
                this.Id = dataCategorys.GetInt32(0);
            }
            else
            {
                Connection.Query("UPDATE `ShopContent`.`Categorys` " +
                    "SET " +
                        $"Name = '{this.Name}' " +
                    "WHERE " +
                        $"Id = {this.Id}", out connection);
                
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection;
            Connection.Query("DELETE FROM `ShopContent`.`Categorys` " +
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
                    MainWindow.init.frame.Navigate(new View.CategoryAdd(this));
                });
            }
        }

        public RelayCommand OnSave
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (this.Id == 0)
                        Save(true);
                    else
                        Save();
                    MainWindow.init.Category.DataContext = new VMCategorys();
                    MainWindow.init.frame.Navigate(MainWindow.init.Category);
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
                    MainWindow.init.Category.DataContext = new VMCategorys();
                    MainWindow.init.frame.Navigate(MainWindow.init.Category);
                });
            }
        }
    }
}
