using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioItem
{
    public string audioName;
    public AudioClip audioClip;
    [HideInInspector] public AudioSource audioSource;
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Logger logger;
    [SerializeField] private List<AudioItem> audioItems = new List<AudioItem>();

    private Dictionary<string, AudioItem> audioDictionary = new Dictionary<string, AudioItem>();


    private void Start() => InitializeAudioDictionary();

    private void InitializeAudioDictionary()
    {
        foreach (AudioItem audioItem in audioItems)
        {
            if (!audioDictionary.ContainsKey(audioItem.audioName))
            {
                audioDictionary.Add(audioItem.audioName, audioItem);

                GameObject instance = new GameObject($"{audioItem.audioName}_Source");
                audioItem.audioSource = instance.AddComponent<AudioSource>();
                audioItem.audioSource.playOnAwake = false;
                audioItem.audioSource.clip = audioItem.audioClip;
                audioItem.audioSource.volume = 0.1f;
                instance.transform.SetParent(transform);
            }
            else
                logger.LogWarning("Duplicate audio name found: " + audioItem.audioName, this);
        }
    }

    public void PlayAudio(string audioName, bool randomPitch = false)
    {
        if (audioDictionary.TryGetValue(audioName, out AudioItem audioItem))
        {
            if (audioItem == null)
            {
                logger.LogWarning("Audio item missing!", this);
                return;
            }

            if (randomPitch)
                audioItem.audioSource.pitch = Random.Range(0.95f, 1.05f);
            else
                audioItem.audioSource.pitch = 1.0f;

            audioItem.audioSource.Play();
        }
        else
        {
            logger.LogWarning($"Audio clip with name {audioName} not found", this);
        }
    }
}
