using ShopContent_Klimov.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using ShopContent_Klimov.Classes;
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
            SqlConnection connection;
            SqlDataReader dataCategory = Connection.Query("SELECT * FROM `Categorys`", out connection);

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
    }
}
