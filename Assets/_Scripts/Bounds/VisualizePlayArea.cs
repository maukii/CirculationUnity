using UnityEngine;

public class VisualizePlayArea : MonoBehaviour
{
    [SerializeField] BoundsData playAreaBounds;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playAreaBounds.bounds.center, playAreaBounds.bounds.size);
    }
}
