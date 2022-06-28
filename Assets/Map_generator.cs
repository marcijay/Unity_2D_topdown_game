using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]

public class Map_generator : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject Enemy;
    public GameObject Portal;
    public GameObject HealingItem;

    public GameObject Player;

    public Grid grid;
    public Tilemap tileMap;
    public TilemapRenderer tilemapRenderer;

    public Tile plainsTile;
    public Tile highlandTile;
    public Tile barierTile;

    public List<GameObject> plainsFoliage;
    public List<GameObject> highlandFoliage;

    public List<(int x, int y)> plainsCoordinates;
    public List<(int x, int y)> highlandCoordinates;

    public bool createPlayer;
    private bool portalExists = false;
    private int startingEnemiesCount;
    private UI_scipt uiScript;

    [Range(0, 500)]
    public int mapHeight;
    [Range(0, 500)]
    public int mapWidth;

    [Range(0, 50)]
    public int enemies;

    [Range(0, 50)]
    public int enemiesToKill;

    [Range(0f, 1f)]
    public float foliageProbalility;
    [Range(0f, 1f)]
    public float healingItemProbability;
    [Range(0f, 1f)]
    public float plainsRangeTop;
    [Range(0f, 1f)]
    public float highlandRangeTop;

    public void GenerateMap(int x, int y)
    {
        plainsCoordinates = new List<(int x, int y)>();
        highlandCoordinates = new List<(int x, int y)>();

        int seed1 = Random.Range(0, 100000);
        int seed2 = Random.Range(0, 100000);
        int seed3 = Random.Range(0, 100000);
        Tile selected;

        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                if(i == 0 || i == x - 1 || j == 0 || j == y - 1)
                {
                    selected = barierTile;
                }
                else
                {
                    float val1 = Mathf.PerlinNoise((float)i / x * seed1, (float)j / y * seed1) * 3/12f;
                    float val2 = Mathf.PerlinNoise((float)i / x * seed2, (float)j / y * seed2) * 4/12f;
                    float val3 = Mathf.PerlinNoise((float)i / x * seed3, (float)j / y * seed3) * 3/12f;
                    float val4 = Random.value * 2 / 12f;

                    float totalVal = val1 + val2 + val3 + val4; 

                    if(totalVal >= 0 && totalVal < plainsRangeTop)
                    {
                        selected = plainsTile;
                        plainsCoordinates.Add((i, j));
                    }
                    else if(totalVal >= plainsRangeTop && totalVal < highlandRangeTop)
                    {
                        selected = highlandTile;
                        highlandCoordinates.Add((i, j));
                    }
                    else
                    {
                        selected = barierTile;
                    }
                }
                tileMap.SetTile(new Vector3Int(i, j, 0), selected);
            }
        }

    }

    public void GenerateFoliage(float probalility)
    {
        foreach(var coordinates in plainsCoordinates)
        {
            float val = Random.Range(0f, 1f);
            if(val < probalility)
            {
                GameObject foliageObject = plainsFoliage[Random.Range(0, plainsFoliage.Count)];
                float x = coordinates.x + Random.Range(0f, 0.5f);
                float y = coordinates.y + Random.Range(0f, 0.5f);
                Instantiate(foliageObject, new Vector2(x, y), Quaternion.identity);
            }
        }
        foreach (var coordinates in highlandCoordinates)
        {
            float val = Random.Range(0f, 1f);
            if (val < probalility)
            {
                GameObject foliageObject = highlandFoliage[Random.Range(0, highlandFoliage.Count)];
                float x = coordinates.x + Random.Range(0f, 0.5f);
                float y = coordinates.y + Random.Range(0f, 0.5f);
                Instantiate(foliageObject, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    public void GenerateHealingItems(float probalility)
    {
        foreach (var coordinates in highlandCoordinates)
        {
            float val = Random.Range(0f, 1f);
            if (val < probalility)
            {
                float x = coordinates.x + Random.Range(0f, 0.5f);
                float y = coordinates.y + Random.Range(0f, 0.5f);
                Instantiate(HealingItem, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    public void GenerateEnemies(int numberToCreate)
    {
        for (int i = 0; i < numberToCreate; i++)
        {
            var pos = plainsCoordinates[Random.Range(0, plainsCoordinates.Count)];
            GameObject enemy = Instantiate(Enemy, new Vector3(pos.x + Random.Range(0, 3), pos.y + Random.Range(0, 3), 0), Quaternion.identity);
            enemy.GetComponent<Enemy_script>().OnDestroy += Map_generator_OnDestroy;
        }
    }

    private void Map_generator_OnDestroy(object sender, System.EventArgs e)
    {
        enemies--;

        UpdateEnemiesCount();

        if (startingEnemiesCount - enemies >= enemiesToKill && !portalExists)
        {
            Vector2 direction = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            Vector2 oppositeDirection = -direction;
            Instantiate(Portal, Player.transform.position + (Vector3)direction, Quaternion.identity);
            Instantiate(Portal, Player.transform.position + (Vector3)oppositeDirection, Quaternion.identity);
            portalExists = true;
        }
    }

    private void UpdateEnemiesCount()
    {
        uiScript.SetEnemiesLeftText(enemies);
        uiScript.SetToKillText((enemiesToKill - (startingEnemiesCount - enemies) > 0) ? enemiesToKill - (startingEnemiesCount - enemies) : 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        startingEnemiesCount = enemies;
        grid = GetComponent<Grid>();
        tileMap = GetComponent<Tilemap>();
        GenerateMap(mapWidth, mapHeight);
        GenerateFoliage(foliageProbalility);
        GenerateEnemies(enemies);
        GenerateHealingItems(healingItemProbability);

        var pos = plainsCoordinates[Random.Range(0, plainsCoordinates.Count)];
        uiScript = FindObjectOfType<Canvas>().GetComponent<UI_scipt>();
        UpdateEnemiesCount();

        if (createPlayer)
        {
            Player = Instantiate(PlayerPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

            uiScript.effectsSource = Player.GetComponent<AudioSource>();
            uiScript.SetEffectsSourceVolume();
        }
        else
        {
            Player = GameObject.FindWithTag("Player");
            Player.transform.position = new Vector2(pos.x, pos.y);
            Portal.GetComponent<Portal_script>().levelName = "StartMenu";
        }
        
    }
}
