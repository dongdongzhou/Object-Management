﻿using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
    [SerializeField] private SpawnZone[] spawnZones;
    [SerializeField] private bool sequential;
    private int nextSequentialIndex;

    public override Vector3 SpawnPoint
    {
        get
        {
            int index;
            if (sequential)
            {
                nextSequentialIndex %= spawnZones.Length;
                index = nextSequentialIndex++;
            }
            else
                index = Random.Range(0, spawnZones.Length);

            return spawnZones[index].SpawnPoint;
        }
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(nextSequentialIndex);
    }

    public override void Load(GameDataReader reader)
    {
        nextSequentialIndex = reader.ReadInt();
    }
}