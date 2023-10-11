using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    SoundManager soundManager;

    int selectSound;
    string[] sounds;

    private void OnEnable()
    {
        soundManager = target as SoundManager;
        UnityEditorInternal.ComponentUtility.MoveComponentUp(soundManager);

        sounds = new string[soundManager.AudioClips.Count];

        for (int i = 0; i < soundManager.AudioClips.Count; i++)
        {
            sounds[i] = soundManager.AudioClips[i].name;
        }
    }

    public override void OnInspectorGUI()
    {
        if (soundManager == null)
            return;
       
        selectSound = GUILayout.Toolbar(selectSound, sounds);

        base.OnInspectorGUI();
    }
}

#endif

public enum SoundType
{
    Effect, BGM, End
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource[] audios;

    [SerializeField]
    float pitch;
    [SerializeField]
    float volume;

    public List<AudioClip> AudioClips { get { return audioClips; } }

    // Start is called before the first frame update
    void Start()
    {
        audios = new AudioSource[(int)SoundType.End];
    }

    public void PlaySound(int soundID, SoundType soundType)
    {
        soundID--;
        if(soundID < 0)
        {
            throw new System.Exception("Sound ID가 올바르지 않습니다.");
        }

        audios[((int)soundType)].clip = audioClips[soundID];

        AudioSource audioSource = audios[((int)soundType)];

        audioSource.volume = volume;
        audioSource.pitch = pitch;
        
        switch(soundType)
        { 
            case SoundType.Effect:
                audioSource.PlayOneShot(audioClips[soundID]);
                break;
            case SoundType.BGM:
                if(audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = audioClips[soundID];
                break;
        }
    }

    public void StopAllSound()
    {
        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].Stop(); 
        }
    }
}
