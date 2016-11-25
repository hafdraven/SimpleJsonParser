using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;
using SimpleJsonParser;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined,MaxByteSize =8000)]
public struct ScalarArray_AGG:IBinarySerialize
{
    private JsonArray _value;
    public void Init()
    {
        _value = new JsonArray();
    }

    public void Accumulate(SqlString Value)
    {
        if (!Value.IsNull)
        _value.Add(JsonValue.ParseScalar(Value.ToString()));
    }

    public void Merge (ScalarArray_AGG Group)
    {
        _value.Add(Group._value);
    }

    public SqlJson Terminate ()
    {
        // Put your code here
        return new SqlJson(_value);
    }

    public void Read(BinaryReader r)
    {
        _value=(JsonArray)JsonArray.Parse(r.ReadString());
        if (_value == null) _value = new JsonArray();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(_value.ToString());
    }
}
