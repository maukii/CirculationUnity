using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private float score = 0;
    private bool isPlaying = false;


    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void Update()
    {
        if (!isPlaying)
            return;

        UpdateScore();
    }

    private void UpdateScore() => score += Time.deltaTime;

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PreparingGame)
            score = 0;

        isPlaying = state == GameManager.GameState.Playing;
    }

    public int GetScore() => Mathf.FloorToInt(score);
}