using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VoxelPlace : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public MeshFilter MeshFilter;
    public MeshCollider MeshCollider;
    public bool RequiresMeshGeneration = false;
    private int PreviousFrame;

    private void Awake()
    {
        MeshCollider = GetComponent<MeshCollider>();
        MeshFilter = GetComponent<MeshFilter>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        RequiresMeshGeneration = true;
    }

    private void Update()
    {
    }
    
    public void Show(int frame)
    {
        List<Vector3Int> Peaks = new List<Vector3Int>();
        if (frame > 0 && frame <= 1280 && frame != PreviousFrame)
        {
            using (var reader = new StreamReader(@"C:\Users\Dev\PlacePlacer\Assets\Resources\" + frame.ToString() + ".csv"))
            { 
                string tmp = reader.ReadLine();
                for (int i = 0; i < 20000; i+=3)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(',');
                    Peaks.Add(new Vector3Int(-int.Parse(values[0]), int.Parse(values[2]), int.Parse(values[1])));//int.Parse(values[i + 1]), int.Parse(values[i + 2])));
                }
            }
            PreviousFrame = frame;
            GenerateMesh(Peaks);
        }
    }
    
    void GenerateMesh(List<Vector3Int> Peaks)
    {
        Mesh newMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> indices = new List<int>();

        int currentIndex = 0;
        //GeneratePlane(ref currentIndex, new Vector3Int(0, 0, 0), vertices, normals, uvs, indices, new Rect());
        for (int i = 0; i < Peaks.Count; i++)
            GeneratePeak(ref currentIndex, Peaks[i], vertices, normals, uvs, indices);
        
        newMesh.SetVertices(vertices);
        newMesh.SetNormals(normals);
        newMesh.SetUVs(0, uvs);
        newMesh.SetIndices(indices, MeshTopology.Triangles, 0);

        newMesh.RecalculateTangents();
        MeshFilter.mesh = newMesh;
        MeshCollider.sharedMesh = newMesh;
        //Set Texture

        RequiresMeshGeneration = false;
    }

    void GeneratePeak(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices) {
        GenerateBlock_Top(ref currentIndex, offset, vertices, normals, uvs, indices, new Rect());
        GenerateBlock_Left(ref currentIndex, offset, vertices, normals, uvs, indices, new Rect());
        GenerateBlock_Right(ref currentIndex, offset, vertices, normals, uvs, indices, new Rect());
        GenerateBlock_Back(ref currentIndex, offset, vertices, normals, uvs, indices, new Rect());
        GenerateBlock_Forward(ref currentIndex, offset, vertices, normals, uvs, indices, new Rect());
    }
   
    void GenerateBlock_Back(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(1999f, 1f, 0f) + offset);
        vertices.Add(new Vector3(2000f, 1f, 0f) + offset);
        vertices.Add(new Vector3(2000f, -offset.y, 0f) + offset);
        vertices.Add(new Vector3(1999f, -offset.y, 0f) + offset);

        normals.Add(Vector3.back);
        normals.Add(Vector3.back);
        normals.Add(Vector3.back);
        normals.Add(Vector3.back);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }

    void GenerateBlock_Forward(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(2000f, 1f, 1f) + offset);
        vertices.Add(new Vector3(1999f, 1f, 1f) + offset);
        vertices.Add(new Vector3(1999f, -offset.y, 1f) + offset);
        vertices.Add(new Vector3(2000f, -offset.y, 1f) + offset);

        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }

    void GenerateBlock_Left(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(1999f, 1f, 1f) + offset);
        vertices.Add(new Vector3(1999f, 1f, 0f) + offset);
        vertices.Add(new Vector3(1999f, -offset.y, 0f) + offset);
        vertices.Add(new Vector3(1999f, -offset.y, 1f) + offset);

        normals.Add(Vector3.left);
        normals.Add(Vector3.left);
        normals.Add(Vector3.left);
        normals.Add(Vector3.left);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }

    void GenerateBlock_Right(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(2000f, 1f, 0f) + offset);
        vertices.Add(new Vector3(2000f, 1f, 1f) + offset);
        vertices.Add(new Vector3(2000f, -offset.y, 1f) + offset);
        vertices.Add(new Vector3(2000f, -offset.y, 0f) + offset);

        normals.Add(Vector3.right);
        normals.Add(Vector3.right);
        normals.Add(Vector3.right);
        normals.Add(Vector3.right);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }

    void GenerateBlock_Top(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(0f, 1f, 1f) + offset);
        vertices.Add(new Vector3(1f, 1f, 1f) + offset);
        vertices.Add(new Vector3(1f, 1f, 0f) + offset);
        vertices.Add(new Vector3(0f, 1f, 0f) + offset);

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }
    void GeneratePlane(ref int currentIndex, Vector3Int offset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> indices, Rect blockUVs)
    {
        vertices.Add(new Vector3(0f, 0f, 2000f) + offset);
        vertices.Add(new Vector3(2000f, 0f, 2000f) + offset);
        vertices.Add(new Vector3(2000f, 0f, 0f) + offset);
        vertices.Add(new Vector3(0f, 0f, 0f) + offset);

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMax));
        uvs.Add(new Vector2(blockUVs.xMax, blockUVs.yMin));
        uvs.Add(new Vector2(blockUVs.xMin, blockUVs.yMin));

        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 1);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 0);
        indices.Add(currentIndex + 2);
        indices.Add(currentIndex + 3);
        currentIndex += 4;
    }
}
