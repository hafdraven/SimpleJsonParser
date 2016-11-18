using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using SimpleJsonParser;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, IsFixedLength=false, IsByteOrdered=false)]
public struct SqlJson: INullable
{
    private JsonValue _value;

    public override string ToString()
    {
        if (_null || _value == null) return String.Empty;
        return _value.ToString();
    }
    
    public bool IsNull
    {
        get
        {
            return _null;
        }
    }
    
    public static SqlJson Null
    {
        get
        {
            SqlJson h = new SqlJson();
            h._null = true;
            return h;
        }
    }
    
    public static SqlJson Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        SqlJson u = new SqlJson() { _value = JsonValue.Parse(s.ToString()), _null=false};
        // Put your code here
        return u;
    }
    
 
    //  Private member
    private bool _null;
}