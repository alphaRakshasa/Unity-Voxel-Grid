using UnityEngine;
using System.Collections;

[SelectionBase]
public class VoxelGrid : MonoBehaviour
{
    [SerializeField]
    private  int resolution;
    [SerializeField]
    private GameObject voxelPrefab;
    
    private float voxelSize;
    private bool[] voxels;

    private Material[] voxelMaterials;

    public void Initialize (int resolution, float size)
    {
        this.resolution = resolution;
        voxelSize = 1f / (float)resolution;
        voxels = new bool[resolution * resolution];
        voxelMaterials = new Material[voxels.Length];
        
        for (int i = 0, y = 0; y < resolution; y++)
        {
            for (int x =0; x < resolution; x++, i++)
            {
                CreateVoxel(i, x, y);
            }
        }
        SetVoxelColors();
    }

    private void CreateVoxel (int i, int x, int y)
    {
        Vector3 voxelScale = Vector3.one * voxelSize * 0.9f;
        GameObject instance = Instantiate(voxelPrefab) as GameObject;
        instance.transform.parent = transform;
        instance.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize);
        instance.transform.localScale = voxelScale;
        voxelMaterials[i] = instance.GetComponent<MeshRenderer>().material;
    }

    public void SetVoxel (int x, int y, bool state)
    {
        voxels[y * resolution + x] = state;
        SetVoxelColors();
    }

    private void SetVoxelColors ()
    {
        for (int i = 0; i < voxels.Length; i++)
        {
            voxelMaterials[i].color = (voxels[i] ? Color.black : Color.white);
        }
        

    }
}