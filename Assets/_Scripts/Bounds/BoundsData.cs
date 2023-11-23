using UnityEngine;

[CreateAssetMenu(fileName = "Bounds", menuName = "Bounds")]
public class BoundsData : ScriptableObject
{
    [field: SerializeField] public Bounds bounds { get; private set; }
}
