using UnityEngine;

public class Shape : PersistableObject
{
    private static readonly int colorPropertyId = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock sharedPropertyBlock;
    private int shapeId = int.MinValue;
    private Color color;
    private MeshRenderer meshRenderer;


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
        writer.Write(color);
        base.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        base.Load(reader);
    }

    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        if (sharedPropertyBlock == null)
            sharedPropertyBlock = new MaterialPropertyBlock();
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
}