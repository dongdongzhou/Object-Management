using UnityEngine;

public class Shape : PersistableObject
{
    private int shapeId=int.MinValue;

    public int ShapeId
    {
        get => shapeId;
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
                shapeId = value;
            else
                Debug.LogError("Not allowed to change shapeId.");
        }
    }
    public int MaterialId { get; set; }
    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapeId);
        writer.Write(MaterialId);
        base.Save(writer);
 //       print(shapeId);
      
    }

    public void SetMaterial(Material material, int materialId)
    {
        GetComponent<MeshRenderer>().material = material;
        MaterialId = materialId;
    }
}