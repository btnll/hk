namespace hw_reflect_attribute.Domain;

public class DBFieldAttribute : Attribute
{
    public string DBField { get; set; }
    public DBFieldAttribute(string dbfield)
    {
        DBField = dbfield;
    }
}