using CORE.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Infra_Structure_Layer.Repositories
{
    public class GenericRepository<Tentity> : IRepository<Tentity>
    {
        public async Task Add(Tentity entity)
        {
            var tableName = typeof(Tentity).Name;
            var properties = typeof(Tentity).GetProperties().Where(p => p.Name != "Id" && p.Name != "Image");
            var columnNames = string.Join(",", properties.Select(p => p.Name));
            var parametersName = string.Join(",", properties.Select(p => "@" + p.Name));
            var query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parametersName})";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, entity);
            }
        }
        public async Task Update(Tentity entity)
        {
            var tableName = typeof(Tentity).Name;
            var primaryKey = "Id";
            var properties = typeof(Tentity).GetProperties().Where(p => p.Name != primaryKey);
            var parametersName = string.Join(",", properties.Select(p => $"{p.Name}=@{p.Name}"));
            var query = $"UPDATE {tableName} SET {parametersName} WHERE {primaryKey}=@{primaryKey}";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, entity);
            }
        }
        public async Task Delete(int id)
        {
            var tableName = typeof(Tentity).Name;
            var primaryKey = "Id";
            var query = $"DELETE FROM {tableName} WHERE {primaryKey}=@id";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { id = id });
            }
        }
        public async Task<List<Tentity>> viewAll()
        {
            var tableName = typeof(Tentity).Name;
            var query = $"SELECT * FROM {tableName}";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<Tentity>(query);
                return result.ToList();
            }
        }
        public async Task<Tentity> findById(int id)
        {
            var tableName = typeof(Tentity).Name;
            var primaryKey = "Id";
            var query = $"SELECT * FROM {tableName} WHERE {primaryKey}=@id";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Tentity>(query, new { id = id });
            }
        }
        private async Task<Tentity> MapReaderToObject(SqlDataReader reader)
        {
            var entity = Activator.CreateInstance<Tentity>();
            foreach (var property in typeof(Tentity).GetProperties())
            {
                if (property.Name != "Id")
                {
                    try
                    {
                        int ordinal = reader.GetOrdinal(property.Name);
                        if (!reader.IsDBNull(ordinal))
                        {
                            var value = reader.GetValue(ordinal);
                            if (value != DBNull.Value)
                            {
                                property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return await Task.FromResult(entity);
        }
    }
}










//using CORE.Interface;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dapper;

//namespace Infra_Structure_Layer.Repositories
//{
//    public class GenericRepository<Tentity> : IRepository<Tentity>
//    {
//        //private readonly string connectionString;
//        public void Add(Tentity entity)
//        {
//            var tableName = typeof(Tentity).Name;
//            var properties = typeof(Tentity).GetProperties().Where(p => p.Name != "Id" && p.Name !="Image");
//            var coloumNames = string.Join(",", properties.Select(p => p.Name));
//            var parametersName = string.Join(",", properties.Select(p => "@" + p.Name));
//            var query = $"insert into {tableName} ({coloumNames}) values ({parametersName})";
//            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
//            {
//                connection.Open();
//                connection.Execute(query, entity);
//            }
//        }
//        public void Update(Tentity entity)
//        {
//            var tableName = typeof(Tentity).Name;
//            var primaryKey = "Id";
//            var properties = typeof(Tentity).GetProperties().Where(p => p.Name != primaryKey);
//            var parametersName = string.Join(",", properties.Select(p => $"{p.Name}=@{p.Name}"));
//            var query = $"update {tableName} set {parametersName} where {primaryKey}=@{primaryKey}";
//            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
//            {
//                connection.Open();
//                connection.Execute(query, entity);
//            }
//        }
//        public void Delete(int id)
//        {
//            var tableName = typeof(Tentity).Name;
//            var primaryKey = "Id";
//            var query = $"delete from {tableName} where {primaryKey}=@id";
//            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
//            {
//                connection.Open();
//                connection.Execute(query, new { id = id });
//            }

//        }
//        public List<Tentity> viewAll()
//        {
//            var tableName = typeof(Tentity).Name;
//            var query = $"select * from {tableName}";
//            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
//            {
//                connection.Open();
//                return connection.Query<Tentity>(query).ToList();
//            }
//        }
//        public Tentity findById(int id)
//        {
//            var tableName = typeof(Tentity).Name;
//            var primaryKey = "Id";
//            var query = $"select * from {tableName} where {primaryKey}=@id";
//            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True"))
//            {
//                connection.Open();
//                return connection.Query<Tentity>(query, new { id = id }).FirstOrDefault();
//            }
//        }

//        private Tentity MapReaderToObject(SqlDataReader reader)
//        {
//            var entity = Activator.CreateInstance<Tentity>();
//            foreach (var property in typeof(Tentity).GetProperties())
//            {
//                if (property.Name != "Id")
//                {
//                    try
//                    {
//                        int ordinal = reader.GetOrdinal(property.Name);
//                        if (!reader.IsDBNull(ordinal))
//                        {
//                            var value = reader.GetValue(ordinal);
//                            if (value != DBNull.Value)
//                            {
//                                property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine(ex.Message); 
//                    }
//                }
//            }
//            return entity;
//        }
//    }
//}
