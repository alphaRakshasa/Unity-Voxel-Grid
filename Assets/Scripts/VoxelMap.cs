using UnityEngine;
using System.Collections;

public class VoxelMap : MonoBehaviour
{
    [SerializeField]
    private float size = 2f;
    [SerializeField]
    private int voxelResolution = 8;
    [SerializeField]
    private int chunkResolution = 2;
    [SerializeField]
    private VoxelGrid VoxelGridPrefab;

    private VoxelGrid[] chunks;
    private float chunkSize, voxelSize, halfSize;
    private float voxelSizeInverse, voxelResolutionInverse, chunkResolutionInverse;

    private void Awake ()
    {
        InitializeCoefficients();

        chunks = new VoxelGrid[chunkResolution * chunkResolution];
        for (int i = 0, y = 0; y < chunkResolution; y++)
        {
            for (int x = 0; x < chunkResolution; x++, i++)
            {
                CreateChunk(i, x, y);
            }
        }

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(size, size);
    }

    private void InitializeCoefficients ()
    {
        voxelResolutionInverse = 1f / voxelResolution;
        chunkResolutionInverse = 1f / chunkResolution;

        halfSize = size * 0.5f;
        chunkSize = size / chunkResolution;
        voxelSize = chunkSize / voxelResolution;

        voxelSizeInverse = 1f / voxelSize;
    }

    private void CreateChunk (int i, int x, int y)
    {
        VoxelGrid chunk = Instantiate(VoxelGridPrefab) as VoxelGrid;
        chunk.Initialize(voxelResolution, chunkSize);
        chunk.transform.parent = transform;
        chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
        chunks[i] = chunk;
    }

    private void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    EditVoxels(transform.InverseTransformPoint(hit.point));
                }
            }
        }
    }

    private void EditVoxels (Vector3 point)
    {
        // TODO test float to int conversion vs int division

        int voxelX = (int)((point.x + halfSize) * voxelSizeInverse);
        int voxelY = (int)((point.y + halfSize) * voxelSizeInverse);
        int chunkX = (int)(voxelX * voxelResolutionInverse);
        int chunkY = (int)(voxelY * voxelResolutionInverse);
        //Debug.Log(voxelX + ", " + voxelY + " in chunk " + chunkX + ", " + chunkY);

        voxelX -= chunkX * voxelResolution;
        voxelY -= chunkY * voxelResolution;
        chunks[chunkY * chunkResolution + chunkX].SetVoxel(voxelX, voxelY, true);
    }
}
