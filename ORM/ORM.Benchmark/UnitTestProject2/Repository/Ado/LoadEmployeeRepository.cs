﻿using System.Data;

namespace UnitTestProject2.Repository.Ado
{
    public class LoadEmployeeRepository : IAdoEmployeeRepository
    {
        public LoadEmployeeRepository(string connectionName)
        {
            this.ConnectionName = connectionName;
        }

        public string ConnectionName { get; set; }

        public DataTable GetAllEmployees(out int count)
        {
            DataTable result = null;

            using (var dbConnection = DbManager.CreateConnection(this.ConnectionName))
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandType = CommandType.Text;

                dbCommand.CommandText = SqlEmployeeText.AllEmployee;
                var reader = dbCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                result = new DataTable();
                result.Load(reader);
                count = result.Rows.Count;
            }

            return result;
        }

        public DataTable GetAllEmployeesDetail(out int count)
        {
            DataTable result = null;

            using (var dbConnection = DbManager.CreateConnection(this.ConnectionName))
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandType = CommandType.Text;

                dbCommand.CommandText = SqlIdentityText.Count;
                count = (int) dbCommand.ExecuteScalar();
                if (count == 0)
                {
                    return result;
                }

                dbCommand.CommandText = SqlIdentityText.InnerJoinEmployee;
                var reader = dbCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                result = new DataTable();
                result.Load(reader);
            }

            return result;
        }
    }
}