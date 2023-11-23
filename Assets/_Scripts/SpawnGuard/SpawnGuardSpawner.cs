using System;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnGuardSpawner : MonoBehaviour
{
    [SerializeField] private SpawnGuard spawnGuardPrefab;


    public async Task GenerateSpawnGuardAsync()
    {
        SpawnGuard spawnGuard = Instantiate(spawnGuardPrefab, Vector3.zero, Quaternion.identity, transform);
        float creationDelay = spawnGuard.GetComponent<ICreationDelay>().GetCreationDelay();
        await Task.Delay(TimeSpan.FromSeconds(creationDelay));
    }
}
