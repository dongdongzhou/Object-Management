﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : PersistableObject
{
    private const int saveVersion = 2;
    public KeyCode createKey = KeyCode.C;
    public KeyCode destroyKey = KeyCode.X;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    private List<Shape> shapes;
    public PersistentStorage storage;
    public ShapeFactory shapeFactory;
    private float creationProgress;
    private float destructionProgress;
    public int levelCount;
    private int loadedLevelBuildIndex;
    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapes.Count);
        writer.Write(loadedLevelBuildIndex);
        foreach (Shape instance in shapes)
            //          writer.Write(instance.ShapeId);
            instance.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > saveVersion)
            Debug.LogError("Unsupported future save version " + version);
        int count = version <= 0 ? -version : reader.ReadInt();
        StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));

        for (int i = 0; i < count; i++)
        {
            int shapeId = version > 0 ? reader.ReadInt() : 0;
            int materialId = version > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeId, materialId);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }


    private void Start()
    {
        shapes = new List<Shape>();
        if (Application.isEditor)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name.Contains("Level "))
                {
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
                    return;
                }
            }
        }

        StartCoroutine(LoadLevel(1));
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
            CreateShape();
        else if (Input.GetKeyDown(destroyKey))
            DestroyShape();
        else if (Input.GetKeyDown(newGameKey))
            BeginNewGame();
        else if (Input.GetKeyDown(saveKey))
            storage.Save(this, saveVersion);
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
        else
        {
            for (int i = 1; i <= levelCount; i++)
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
        }

        creationProgress += Time.deltaTime * CreationSpeed;
        while (1f <= creationProgress)
        {
            creationProgress -= 1f;
            CreateShape();
        }

        destructionProgress += Time.deltaTime * DestructionSpeed;
        while (1f <= destructionProgress)
        {
            destructionProgress -= 1f;
            DestroyShape();
        }
    }

    private void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotationUniform;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV(
                                          hueMin: 0f, hueMax: 1f,
                                          saturationMin: 0.5f, saturationMax: 1f,
                                          valueMin: 0.25f, valueMax: 1f,
                                          alphaMin: 1f, alphaMax: 1f
                                         ));
        shapes.Add(instance);
    }

    private void BeginNewGame()
    {
        foreach (Shape instance in shapes)
            shapeFactory.Reclaim(instance);
        shapes.Clear();
    }

    private void DestroyShape()
    {
        if (shapes.Count > 0)
        {
            int index = Random.Range(0, shapes.Count);
            shapeFactory.Reclaim(shapes[index]);
            int lastIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
            shapes.RemoveAt(lastIndex);
        }
    }

    private IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (loadedLevelBuildIndex > 0)
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }
}