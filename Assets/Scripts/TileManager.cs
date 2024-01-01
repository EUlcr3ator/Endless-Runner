using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    private float zSpawn = 0;
    public float tileLength;
    int numberOfTiles = 5;

    public Transform playerTransform;
    private List<GameObject> activeTiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SpawnTile(0);
        for (int i = 0; i < numberOfTiles; i++)
            SpawnTile(Random.Range(0, tilePrefabs.Length));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }
    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        zSpawn += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
