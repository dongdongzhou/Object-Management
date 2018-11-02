using System.IO;
using UnityEngine;

public class GameDataReader
{
    private readonly BinaryReader reader;
    public int Version { get; }

    public GameDataReader(BinaryReader reader, int version)
    {
        this.reader = reader;
        Version = version;
    }

    public float ReadFloat() => reader.ReadSingle();

    public int ReadInt() => reader.ReadInt32();

    public Quaternion ReaderQuaternion()
    {
        Quaternion value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        value.w = reader.ReadSingle();
        return value;
    }

    public Vector3 ReadVector3()
    {
        Vector3 value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        return value;
    }

    public Color ReadColor()
    {
        Color value;
        value.r = reader.ReadSingle();
        value.g = reader.ReadSingle();
        value.b = reader.ReadSingle();
        value.a = reader.ReadSingle();
        return value;
    }

    public Random.State ReadRandomState() =>
        JsonUtility.FromJson<Random.State>(reader.ReadString());
}