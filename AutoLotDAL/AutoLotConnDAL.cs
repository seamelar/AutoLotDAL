using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AutoLotConnectedLayer
{
    public class InventoryDAL
    {
        private SqlConnection connect = null;

        public void OpenConnection(string connectionString)
        {
            connect = new SqlConnection(connectionString);
            connect.Open();
        }

        public void CloseConnection()
        {
            connect.Close();
        }
        public void InsertAuto(int id, string color, string make, string petName)
        {
            // Оператор SQL
            string sql = string.Format("Insert Into Inventory" +
                "(CarID, Make, Color, PetName) Values('{0}','{1}','{2}','{3}')", id, make, color, petName);

            // Параметризованная команда
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@CarID";
                param.Value = id;
                param.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@Make";
                param.Value = make;
                param.SqlDbType = SqlDbType.Char;
                param.Size = 10;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@Color";
                param.Value = color;
                param.SqlDbType = SqlDbType.Char;
                param.Size = 10;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@PetName";
                param.Value = petName;
                param.SqlDbType = SqlDbType.Char;
                param.Size = 10;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteCar(int id)
        {
            string sql = string.Format("Delete from Inventory where CarID = '{0}'", id);
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("К сожалению, эта машина заказана!", ex);
                    throw error;
                }
            }
        }
        public void UpdateCarPetName(int id, string newpetName)
        {
            string sql = string.Format("Update Inventory Set PetName = '{0}' Where CarID = '{1}'",
                   newpetName, id);
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetAllInventoryAsDataTable()
        {
            DataTable inv = new DataTable();
            string sql = "Select * From Inventory";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                inv.Load(dr);
                dr.Close();
            }
            return inv;
        }
    }
}