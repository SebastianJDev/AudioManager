using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManagerEditorWindow : EditorWindow
{
    private string defaultAudioManagerName = "DefaultAudioManager";
    private GameObject defaultAudioManager;

    private List<Sound> sounds = new List<Sound>();

    private string newSoundName = "NewSound";
    private AudioClip newSoundClip;
    private SoundType newSoundType = SoundType.SoundEffect;
    private float newSoundVolume = 1f;
    private bool newSoundPlayOnAwake = false;

    [MenuItem("Window/Audio Manager Editor")]
    public static void ShowWindow()
    {
        GetWindow<AudioManagerEditorWindow>("Audio Manager");
    }
   
    void OnGUI()
    {
        float windowWidth = EditorGUIUtility.currentViewWidth;

        float imageWidth = windowWidth - 20; // Restar el espacio adicional
        float imageHeight = 130;

        Texture2D creatorImage = EditorGUIUtility.Load("Assets/AudioManager/CreatorImage.png") as Texture2D;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(creatorImage, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        GUILayout.Label("Create Default AudioManager", EditorStyles.boldLabel);
        defaultAudioManagerName = EditorGUILayout.TextField("Name", defaultAudioManagerName);
        if (GUILayout.Button("Create Default AudioManager"))
        {
            CreateDefaultAudioManager();
        }

        GUILayout.Space(20);

        GUILayout.Label("Create New Sound", EditorStyles.boldLabel);
        newSoundName = EditorGUILayout.TextField("Name", newSoundName);
        newSoundClip = EditorGUILayout.ObjectField("Clip", newSoundClip, typeof(AudioClip), false) as AudioClip;
        newSoundType = (SoundType)EditorGUILayout.EnumPopup("Type", newSoundType);
        newSoundVolume = EditorGUILayout.Slider("Volume", newSoundVolume, 0f, 1f);
        newSoundPlayOnAwake = EditorGUILayout.Toggle("Play On Awake", newSoundPlayOnAwake);

        if (GUILayout.Button("Create New Sound"))
        {
            CreateNewSound();
        }


        GUILayout.Space(20);

        GUILayout.Label("Sounds in AudioManager", EditorStyles.boldLabel);

        if (defaultAudioManager != null)
        {
            AudioManager audioManager = defaultAudioManager.GetComponent<AudioManager>();
            foreach (Sound sound in sounds)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(sound.name);
                string newName = EditorGUILayout.TextField("NewName", sound.name);
                if (newName != sound.name)
                {
                    sound.name = newName;
                }
                if (GUILayout.Button("Delete"))
                {
                    audioManager.RemoveSound(sound);
                }

                GUILayout.EndHorizontal();
            }
        }
    }


    void CreateDefaultAudioManager()
    {
        defaultAudioManager = new GameObject(defaultAudioManagerName);
        defaultAudioManager.AddComponent<AudioManager>();
    }

    void CreateNewSound()
    {
        if (defaultAudioManager == null || defaultAudioManager.GetComponent<AudioManager>() == null)
        {
            Debug.LogWarning("Default AudioManager not found. Please create it first.");
            return;
        }

        // Crear el nuevo sonido
        Sound newSound = new Sound();
        newSound.name = newSoundName;
        newSound.clip = newSoundClip;
        newSound.type = newSoundType;
        newSound.volume = newSoundVolume;

        // Obtener el componente AudioManager
        AudioManager audioManager = defaultAudioManager.GetComponent<AudioManager>();
        if (audioManager != null)
        {
            // Agregar el nuevo sonido a la lista de sonidos del AudioManager
            audioManager.AddSound(newSound);
            Debug.Log("New sound added to AudioManager: " + newSound.name);
            sounds = audioManager.sounds;
        }
        else
        {
            Debug.LogWarning("AudioManager component not found on the default AudioManager.");
        }
    }
   
}
