using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] prefabs;
    [SerializeField] private Material[] materials;
    [SerializeField] private bool recycle;
    private List<Shape>[] pools;
    private Scene poolScene;

    public Shape Get(int shapeId = 0, int materialId = 0)
    {
        Shape instance;
        if (recycle)
        {
            if (pools == null)
                CreatePools();
            List<Shape> pool = pools[shapeId];
            int lastIndex = pool.Count - 1;

            if (0 <= lastIndex)
            {
                instance = pool[lastIndex];
                instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }
            else
            {
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);

            }
        }
        else
        {
            instance = Instantiate(prefabs[shapeId]);
            instance.ShapeId = shapeId;
        }

        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandom() => Get(
                                    Random.Range(0, prefabs.Length),
                                    Random.Range(0, materials.Length)
                                   );

    public void Reclaim(Shape shapeToRecycle)
    {
        if (recycle)
        {
            if (pools == null)
                CreatePools();
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }

    private void CreatePools()
    {
        pools = new List<Shape>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
            pools[i] = new List<Shape>();

//        if (Application.isEditor)
//        {
//            poolScene = SceneManager.GetSceneByName(name);
//            if (poolScene.isLoaded)
//            {
//                GameObject[] rootObjects = poolScene.GetRootGameObjects();
//                foreach (GameObject instance in rootObjects)
//                {
//                    Shape pooledShape = instance.GetComponent<Shape>();
//                    pools[pooledShape.ShapeId].Add(pooledShape);
//                }
//                return;
//            } 
//        }
        poolScene = SceneManager.CreateScene(name);
    }
}