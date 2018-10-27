﻿using System.IO;
using UnityEngine;

public class GameDataReader
{
    private readonly BinaryReader reader;

    public GameDataReader(BinaryReader reader) => this.reader = reader;

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
}