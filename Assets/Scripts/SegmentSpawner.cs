using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    public GameObject segmentPrefab;
    public float spawnInterval;
    public Transform spawnOrigin;

    private float timer;

    [SerializeField] private ObjectPoolingSystem poolSystem;

    // Start is called before the first frame update
    void Start()
    {
        poolSystem.InitPool(10, segmentPrefab, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnOnInterval();
    }

    private void SpawnOnInterval()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;

            //
            SpawnSegment();
        }
    }

    public void SpawnSegment()
    {
        //Instantiate(segmentPrefab, spawnOrigin.position, segmentPrefab.transform.rotation, spawnOrigin);
        GameObject newSegment = poolSystem.GetObject();

        if (newSegment == null )
        {
            return;
        }

        newSegment.transform.position = spawnOrigin.position;
        newSegment.transform.rotation = spawnOrigin.rotation;
        newSegment.transform.parent = spawnOrigin;
    }
}
