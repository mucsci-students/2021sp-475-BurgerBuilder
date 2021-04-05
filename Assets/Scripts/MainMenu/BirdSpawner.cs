using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject bird;
    public float spawnInterval;

    public int spawnFlockMin;
    public int spawnFlockMax;

    public AudioSource birdsChirping;
    public AudioSource birdsFlapping;

    private MainMenuCamera menuCamera;

    private float lastSpawned;

    void Start()
    {
        menuCamera = GameObject.Find("MainMenuCamera").GetComponent<MainMenuCamera>();
        SpawnBird(Random.Range(spawnFlockMin, spawnFlockMax));
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastSpawned + spawnInterval)
        {
            SpawnBird(Random.Range(spawnFlockMin, spawnFlockMax));
            lastSpawned = Time.time;
        }
    }

    void SpawnBird(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            Bird b = Instantiate(bird, transform.position, Quaternion.identity).GetComponent<Bird>();
            b.transform.position = new Vector3(
                transform.position.x + Random.Range(-20f, 20f),
                transform.position.y + Random.Range(-20f, 20f),
                transform.position.z
            );

            b.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Random.Range(-10f, 10f));
        }
        if(menuCamera.isLookingAtBirds())
        {
            birdsChirping.pitch = Random.Range(0.80f, 1.2f);
            birdsFlapping.pitch = Random.Range(0.80f, 1.20f);
            birdsChirping.Play();
            birdsFlapping.Play();
        }
    }
}
