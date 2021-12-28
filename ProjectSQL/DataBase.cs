using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectSQL
{
    class DataBase
    {
        private readonly string _connstring;

        public struct Store
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public int Count { get; set; }
        }

        public DataBase(string connstring)
        {
            _connstring = connstring;
        }

        public IEnumerable<Store> GetProdList()
        {
            var connection = new SqlConnection(_connstring);
            try
            {
                connection.Open();
                var prodList = new List<Store>();
                var select = new SqlCommand("select * from dbo.sklad", connection);
                var reader = select.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var newProd = new Store
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Name = reader[1].ToString(),
                            Price = int.Parse(reader[2].ToString()),
                            Count = int.Parse(reader[3].ToString())
                        };
                        prodList.Add(newProd);
                    }
                    reader.Close();
                }
                connection.Close();
                return prodList;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<Store>();
            }
        }

        public bool AddNewProd(string prodName, int Price, int Count)
        {
            var connection = new SqlConnection(_connstring);
            try
            {
                connection.Open();
                string cmd = "insert into dbo.sklad(name, price, count) values('" + prodName + "', " + Price + @", " + Count + @")";
                var insert = new SqlCommand(cmd, connection);
                insert.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                connection.Close();
                return false;
            }
        }

        public bool DeleteProd(int id)
        {
            var connection = new SqlConnection(_connstring);
            try
            {
                connection.Open();
                string cmd = "delete from sklad WHERE id = " + id;
                var insert = new SqlCommand(cmd, connection);
                insert.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                connection.Close();
                return false;
            }
        }
    }
}
