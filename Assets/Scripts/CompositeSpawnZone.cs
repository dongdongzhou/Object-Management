using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
    [SerializeField] private bool sequential;
    [SerializeField] private bool overrideConfig;
    [SerializeField] private SpawnZone[] spawnZones;
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

    public override void ConfigureSpawn(Shape shape)
    {
        if (overrideConfig)
            base.ConfigureSpawn(shape);
        else
        {
            int index;
            if (sequential)
            {
                nextSequentialIndex %= spawnZones.Length;
                index = nextSequentialIndex++;
            }
            else
                index = Random.Range(0, spawnZones.Length);

            spawnZones[index].ConfigureSpawn(shape);
        }
    }
}