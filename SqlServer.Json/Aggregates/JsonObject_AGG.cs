using System;
using System.IO;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using SimpleJson.Core;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize =8000)]
public struct JsonObject_AGG:IBinarySerialize
{
    private JsonObject _value;
    private JsonObjectMergeConflictOption _mergeOption;
    private bool _mergeOptionPassed;
    public void Init()
    {
        _value = new JsonObject();
        _mergeOption = JsonObjectMergeConflictOption.MergeArray;
        _mergeOptionPassed = false;
    }

    public void Accumulate(SqlString Key, SqlJson Value, SqlByte MergeFlag)
    {
        if (!_mergeOptionPassed)
        {
            _mergeOption = (JsonObjectMergeConflictOption)MergeFlag.Value;
            _mergeOptionPassed = true;
        }
        _value.Add(new JsonString(Key.ToString()), Value._value, _mergeOption);
    }

    public void Merge(JsonObject_AGG Group)
    {
        _value.Merge(Group._value, _mergeOption);
    }

    public SqlJson Terminate()
    {
        return new SqlJson(_value);
    }


    public void Read(BinaryReader r)
    {
        _mergeOption = (JsonObjectMergeConflictOption)r.ReadByte();
        _mergeOptionPassed = r.ReadBoolean();
        _value = (JsonObject)JsonObject.Parse(r.ReadString());
        if (_value == null) _value = new JsonObject();
    }

    public void Write(BinaryWriter w)
    {
        w.Write((byte)_mergeOption);
        w.Write(_mergeOptionPassed);
        w.Write(_value.ToString());
    }
}
