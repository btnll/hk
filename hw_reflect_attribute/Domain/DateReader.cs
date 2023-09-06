using System.Data.SqlClient;
using System.Configuration;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace hw_reflect_attribute.Domain;


public class DateReader
{
    private readonly IConfiguration _configuration;

    public DateReader(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public  T Read<T>(int id) where T :BaseModel
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
    
    public  List<T> ReadAll<T>() where T :BaseModel
    {
        Type type = typeof(T);

        Dictionary<PropertyInfo,string> propdic = new Dictionary<PropertyInfo, string>();
        foreach (var property in type.GetProperties())
        {
            if (property.IsDefined(typeof(DBFieldAttribute)))
            {
                DBFieldAttribute DBField = (DBFieldAttribute)property.GetCustomAttribute(typeof(DBFieldAttribute),true);
                propdic.Add(property,DBField.DBField);
            }
            else
            {
                propdic.Add(property,property.Name);
            }
        }
        
        //string columnString = string.Join(",", type.GetProperties().Select(x => $"[{x.Name}]"));
        string columnString = string.Join(",", type.GetProperties().Select(x => $"[{propdic[x]}]"));
        
        
        
        string sql = $"SELECT {columnString} FROM [{type.Name}]";
        var str = _configuration["ConnectionStrings:Default"];
        using (SqlConnection conn = new SqlConnection(str))
        {
            SqlCommand command = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            
            List<T> tlist = new List<T>();
            while(reader.Read())
            {
                T t = Activator.CreateInstance<T>();
                foreach (var property in type.GetProperties())
                {
                    property.SetValue(t,reader[propdic[property]]is DBNull ? null : reader[propdic[property]]);
                }
                tlist.Add(t);

            }
            return tlist;
        }
    }
}