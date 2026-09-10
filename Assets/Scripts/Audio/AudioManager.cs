using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    private Dictionary<AudioIDs, AudioClip> _musicClips;
    private Dictionary<AudioIDs, AudioClip> _sfxClips;

    private void Awake() {
        Instance = this;
    }

    public void PlayMusic(AudioIDs audioId) {
        throw new NotImplementedException("TODO");
    }

    public void PlaySfx(AudioIDs audioId) {
        throw new NotImplementedException("TODO");
    }

    public void PlaySfx(AudioIDs audioId, Transform transform, float volume) {
        throw new NotImplementedException("TODO");
    }
    
}
