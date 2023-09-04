using System.Data.SqlClient;
using System.Configuration;
using System;
namespace hw_reflect_attribute.Domain;


public class DateReader
{
    private readonly IConfiguration _configuration;

    public DateReader(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public  T read<T>(int id) where T :BaseModel
    {
        Type type = typeof(T);
        string columnString = string.Join(",", type.GetProperties().Select(x => $"[{x.Name}]"));
        
        string sql = $"SELECT {columnString} FROM [{type.Name}] WHERE Id = {id}";
        var str = _configuration["ConnectionStrings:Default"];
        using (SqlConnection conn = new SqlConnection(str))
        {
            SqlCommand command = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            T t = Activator.CreateInstance<T>();
            if (reader.HasRows)
            {
                reader.Read();
                foreach (var property in type.GetProperties())
                {
                    property.SetValue(t,reader[property.Name] is DBNull ? null : reader[property.Name]);
                }

            }
            return t;
        }
    }
}