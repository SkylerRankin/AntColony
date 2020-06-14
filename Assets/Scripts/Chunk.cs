using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public static readonly int chunkX = 200;
    public static readonly int chunkY = 30;
    public static readonly int chunkZ = 200;

	public MeshRenderer meshRenderer;
	public MeshFilter meshFilter;

    private int vertexIndex;
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;

    private byte[,,] map;
    private Block[] blocks;
    private TerrainHandler terrainHandler;

	public void Start () {
		vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        map = new byte[chunkX, chunkY, chunkZ];
        blocks = Block.GetBlocks();
        terrainHandler = new TerrainHandler();

        LoadMap();
        SetMeshData();
		MakeMesh();

        Debug.Log("Vertices: " + vertices.Count);
        Debug.Log("Triangles: " + triangles.Count / 3);
	}

    private void SetMeshData() {
        for (int x = 0; x < chunkX; x++) {
            for (int y = 0; y < chunkY; y++) {
                for (int z = 0; z < chunkZ; z++) {
                    if (blocks[map[x, y, z]].isSolid) {
                        AddVoxel(new Vector3(x, y, z));
                    }
                }
            }
        }
    }

    private void LoadMap() {
        terrainHandler.LoadTerrain();
        for (int x = 0; x < chunkX; x++) {
            for (int y = 0; y < chunkY; y++) {
                for (int z = 0; z < chunkZ; z++) {
                    float height = terrainHandler.GetHeight(x, z);
                    if (y > height) map[x, y, z] = 0;
                    else if (height - y < 3) map[x, y, z] = 1;
                    else map[x, y, z] = 2;
                }
            }
        }
    }

    private void AddVoxel(Vector3 position) {
        for (int face = 0; face < 6; face++) {
            bool blocked = HasVoxel(position + Voxel.adjacentFaceOffset[face]);
            if (!blocked) {

                // Add each vertex of the face
                vertices.Add(Voxel.vertices[Voxel.triangles[face, 0]] + position);
                vertices.Add(Voxel.vertices[Voxel.triangles[face, 1]] + position);
                vertices.Add(Voxel.vertices[Voxel.triangles[face, 2]] + position);
                vertices.Add(Voxel.vertices[Voxel.triangles[face, 3]] + position);

                // Add each triangle, reusing two of the vertices
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);

                // Used 4 vertices per face
                vertexIndex += 4;

                byte blockType = map[(int) position.x, (int) position.y, (int) position.z];
                AddTexture(blocks[blockType].textures[face]);

            }
		}
    }

    private void AddTexture(int textureIndex) {
        float x = (textureIndex % Block.textureSheetSize) * Block.cellSize;
        float y = (textureIndex / Block.textureSheetSize) * Block.cellSize;
        y = 1f - y - Block.cellSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + Block.cellSize));
        uvs.Add(new Vector2(x + Block.cellSize, y));
        uvs.Add(new Vector2(x + Block.cellSize, y + Block.cellSize));
    
    }

    private void MakeMesh() {
        Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		meshFilter.mesh = mesh;
    }

    private bool PositionInChunk(Vector3 position) {
        return
            position.x >= 0 && position.x < chunkX &&
            position.y >= 0 && position.y < chunkY &&
            position.z >= 0 && position.z < chunkZ;
    }

    private bool HasVoxel(Vector3 position) {
        if (!PositionInChunk(position)) return false;
        return map[
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y),
                    Mathf.FloorToInt(position.z)
                ] > 0;
    }

}