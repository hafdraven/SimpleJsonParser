using System;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;
using SimpleJson.Core;

[Serializable]
[SqlUserDefinedType(Format.UserDefined, IsFixedLength=false, IsByteOrdered=false,MaxByteSize = 4000)]
public struct SqlJson: INullable, IBinarySerialize
{
    
    internal JsonValue _value;

    public override string ToString()
    {
        if (_null || _value == null) return String.Empty;
        return _value.ToString();
    }
    
    public bool IsNull
    {
        get
        {
            return _null || (_value ==null);
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

    public SqlJson Query(SqlString query)
    {
        return new SqlJson(_value.Query(query.ToString()));
    }

    public void Read(BinaryReader r)
    {
        _null = r.ReadBoolean();
        string str = r.ReadString();
        _value = JsonValue.Parse(str);
    }

    public void Write(BinaryWriter w)
    {
        w.Write(_null);
        w.Write(_value==null?"":_value.ToString());
    }

    internal SqlJson(JsonValue v)
    {
        _value = v;
        _null = false;
    }

    //  Private member
    private bool _null;
}