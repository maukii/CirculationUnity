using UnityEngine;

// Serves as a base class that can be overriden
// if any additional logic is required
public class GameStateUI : MonoBehaviour
{
    [SerializeField] protected GameManager.GameState associatedState;


    protected virtual void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    protected virtual void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    protected virtual void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == associatedState)
            EnableUI();
        else
            DisableUI();
    }

    protected virtual void EnableUI() => transform.GetChild(0).gameObject.SetActive(true);

    protected virtual void DisableUI() => transform.GetChild(0).gameObject.SetActive(false);
}