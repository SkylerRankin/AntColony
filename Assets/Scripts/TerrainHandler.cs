using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandler {

    private int seed = 92847234;
    private float[,] terrainHeight;
    private float scale = 1f;
    private float offset = 1f;

    public void LoadTerrain() {
        Random.InitState(seed);
        terrainHeight = new float[Chunk.chunkX, Chunk.chunkZ];
        for (int x = 0; x < Chunk.chunkX; x++) {
            for (int z = 0; z < Chunk.chunkZ; z++) {
                terrainHeight[x, z] = Mathf.PerlinNoise(
                    (x + 0.1f) / Chunk.chunkX * scale + offset,
                    (z + 0.1f) / Chunk.chunkZ * scale + offset
                );
            }
        }
    }

    public float GetHeight(int x, int z) {
        if (
            x < 0 ||
            x > terrainHeight.GetLength(0) ||
            z < 0 ||
            z > terrainHeight.GetLength(1)) return 0f;
        return terrainHeight[x, z] * Chunk.chunkY;
    }

}
