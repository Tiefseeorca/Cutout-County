using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {
    [Serializable]
    private struct AudioItem {
        public AudioIDs Id;
        public AudioClip Clip;

        public static Dictionary<AudioIDs, AudioClip> TurnItemsToDict(AudioItem[] items) {
            Dictionary<AudioIDs, AudioClip> dict = new();
            for (int i = 0; i < items.Length; i++) {
                dict.TryAdd(items[i].Id, items[i].Clip);
            }
            return dict;
        }
    }
    
    public static AudioManager Instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    private Dictionary<AudioIDs, AudioClip> _musicClips;
    private Dictionary<AudioIDs, AudioClip> _sfxClips;
    [SerializeField] private AudioItem[] _musicItems;
    [SerializeField] private AudioItem[] _sfxItems;
    private AudioSource _currentMusic;

    private void Awake() {
        Instance = this;
        _musicClips = AudioItem.TurnItemsToDict(_musicItems);
        _sfxClips = AudioItem.TurnItemsToDict(_sfxItems);
    }

    public void PlayMusic(AudioIDs audioId) {
        AudioSource source = Instantiate(_musicSource);
        AudioClip clip;
        if (_musicClips.TryGetValue(audioId, out clip)) {
            source.clip = clip;
            source.Play();
            _currentMusic = source;
        }
    }
    
    public void PauseCurrentMusic() { _currentMusic.Pause(); }
    public void ResumeCurrentMusic() { _currentMusic.Play(); }

    public void PlaySfx(AudioIDs audioId, float pitch = 1f, float pitchVary = 0.2f) {
        AudioSource source = Instantiate(_sfxSource);
        AudioClip clip;
        if (_sfxClips.TryGetValue(audioId, out clip)) {
            source.clip = clip;
            source.pitch = Random.Range(pitch - pitchVary, pitchVary + pitchVary);
            source.Play();
            float clipLength = clip.length;
            Destroy(source.gameObject, clipLength);
        }
    }

    public void PlaySfx(AudioIDs audioId, Transform transform, float volume = 1, float pitch = 1, float pitchVary = 0.2f) {
        AudioSource source = Instantiate(_sfxSource, transform.position, Quaternion.identity);
        AudioClip clip;
        if (_sfxClips.TryGetValue(audioId, out clip)) {
            source.clip = clip;
            source.volume = volume;
            source.pitch = Random.Range(pitch - pitchVary, pitchVary + pitchVary);
            source.Play();
            float clipLength = clip.length;
            Destroy(source.gameObject, clipLength);
        }
    }
    
}
