using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RuneSpawner : NetworkBehaviour
{
    [Header("Rune Prefabs (Must have NetworkObject)")]
    public GameObject waterRunePrefab;
    public GameObject lightningRunePrefab;

    [Header("Spawn Locations")]
    [Tooltip("Drag your empty GameObjects here to act as spawn spots.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Timing Settings")]
    public float initialSpawnDelay = 2f;
    public float spawnInterval = 10f;
    public int maxRunesInStage = 5;

    // Track active runes and their associated spawn point transform
    private Dictionary<Transform, GameObject> activeRuneMap = new Dictionary<Transform, GameObject>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            StartCoroutine(RuneSpawnRoutine());
        }
    }

    private IEnumerator RuneSpawnRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            // Clean up missing/despawned runes from our tracking map
            CleanupDespawnedRunes();

            // Only spawn if below capacity and we have available spawn points
            if (activeRuneMap.Count < maxRunesInStage)
            {
                SpawnRandomRuneAtFreeLocation();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void CleanupDespawnedRunes()
    {
        List<Transform> keysToRemove = new List<Transform>();

        foreach (var kvp in activeRuneMap)
        {
            if (kvp.Value == null)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (Transform key in keysToRemove)
        {
            activeRuneMap.Remove(key);
        }
    }

    private void SpawnRandomRuneAtFreeLocation()
    {
        // 1. Gather all spawn points that DO NOT currently have an active rune
        List<Transform> freeSpawnPoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (point != null && !activeRuneMap.ContainsKey(point))
            {
                freeSpawnPoints.Add(point);
            }
        }

        // If no empty points are left, skip spawning
        if (freeSpawnPoints.Count == 0) return;

        // 2. Pick a random unoccupied spawn point
        Transform chosenSpawnPoint = freeSpawnPoints[Random.Range(0, freeSpawnPoints.Count)];

        // 3. Select prefab (50/50 Water or Lightning)
        GameObject selectedPrefab = (Random.value > 0.5f) ? waterRunePrefab : lightningRunePrefab;

        if (selectedPrefab == null)
        {
            Debug.LogWarning("[RuneSpawner] Prefab is missing in Inspector!");
            return;
        }

        // 4. Instantiate and Spawn across Network
        GameObject spawnedRune = Instantiate(selectedPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);

        // Mark this spawn point as occupied
        activeRuneMap[chosenSpawnPoint] = spawnedRune;

        NetworkObject netObj = spawnedRune.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
            Debug.Log($"[RuneSpawner] Spawned {selectedPrefab.name} at free spot {chosenSpawnPoint.name}");
        }
        else
        {
            Debug.LogError($"[RuneSpawner] {selectedPrefab.name} is missing NetworkObject!");
        }
    }
}