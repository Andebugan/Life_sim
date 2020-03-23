using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public Tilemap WorldMap;
    public Grid MainGrid;
    public Tile[] Terrain;
    
    double tileChoise;
    int tileChoiseArray;

    public int perlinTextureSizeX;
    public int perlinTextureSizeY;
    public bool randomizeNoiseOffcet;
    public float noiseScale = 1f;
    public int perlinGridStepSizeX;
    public int perlinGridStepSizeY;
    Vector2 perlinOffcet = new Vector2(0, 0);

    private Texture2D perlinTexture;

    public static MapScript instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Generate();
    }

    public void Generate()
    {
        GenerateNoise();
        VisualizeGrid();
    }

    void GenerateNoise()
    {
        if(randomizeNoiseOffcet)
        {
            perlinOffcet = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
        }

        perlinTexture = new Texture2D(perlinTextureSizeX, perlinTextureSizeY);
        
        for (int x = 0; x < perlinTextureSizeX; x++)
        {
            for (int y = 0; y < perlinTextureSizeY; y++)
            {
                perlinTexture.SetPixel(x, y, SampleNoise(x, y));
            }
        }

        perlinTexture.Apply();
    }

    Color SampleNoise(int x, int y)
    {
        float xCoord = (float)x / perlinTextureSizeX * noiseScale + perlinOffcet.x;
        float yCoord = (float)y / perlinTextureSizeY * noiseScale + perlinOffcet.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color perlinColor = new Color(sample, sample, sample);

        return perlinColor;
    }

    public float SampleStepped(int x, int y)
    {
        int gridStepSizeX = perlinTextureSizeX / perlinGridStepSizeX;
        int gridStepSizeY = perlinTextureSizeY / perlinGridStepSizeY;

        float sampledFloat = perlinTexture.GetPixel((Mathf.FloorToInt(x * gridStepSizeX)), (Mathf.FloorToInt(y * gridStepSizeY))).grayscale;

        return sampledFloat;
    }

    void VisualizeGrid()
    {
        for (int x = 0; x < perlinGridStepSizeX; x++)
        {
            for (int y = 0; y < perlinGridStepSizeY; y++)
            {
                tileChoise = SampleStepped(x, y);
                tileChoiseArray = (int)(float)(tileChoise / 0.125);
                WorldMap.SetTile(new Vector3Int(x, y, 0), Terrain[tileChoiseArray]);
            }
        }
    }
}
