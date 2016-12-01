using System;
using System.IO;
using Microsoft.SqlServer.Server;
using SimpleJson.Core;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize =8000)]
public struct JsonArray_AGG:IBinarySerialize
{
    private JsonArray _value;
    public void Init()
    {
        _value = new JsonArray();
    }

    public void Accumulate(SqlJson Value)
    {
        _value.Add(Value._value);
    }

    public void Merge (JsonArray_AGG Group)
    {
        _value.Add(Group._value);
    }

    public SqlJson Terminate ()
    {
       
        return new SqlJson (_value);
    }

    public void Read(BinaryReader r)
    {
        _value = (JsonArray)JsonArray.Parse(r.ReadString());
        if (_value == null) _value = new JsonArray();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(_value.ToString());
    }

}
