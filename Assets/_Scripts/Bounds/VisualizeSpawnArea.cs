using UnityEngine;

public class VisualizeSpawnArea : MonoBehaviour
{
    [SerializeField] BoundsData spawnBounds;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnBounds.bounds.center, spawnBounds.bounds.size);
    }
}
