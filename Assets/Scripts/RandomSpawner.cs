using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] int spawnAmt;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject spawnObject;

    Bounds bounds;

    void Start()
    {
        bounds = ground.GetComponent<MeshCollider>().bounds;
        
        for (int i = 0; i < spawnAmt; i++)
        {
            GameObject newSpawn = Instantiate(spawnObject);
            Vector3 randomLocation = new Vector3(Random.Range(-bounds.max.x, bounds.max.x), 0f, Random.Range(-bounds.max.z, bounds.max.z));
            float randomScale = Random.Range(0.5f, 10f);
            
            newSpawn.transform.position = randomLocation;
            newSpawn.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
    }

}
