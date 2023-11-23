using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private GameManager.GameState newGameState;


    public void ChangeGameState() => GameManager.Instance.SetGameState(newGameState);
}
