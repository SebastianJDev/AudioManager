using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManagerEditorWindow : EditorWindow
{
    private string defaultAudioManagerName = "DefaultAudioManager";
    private GameObject defaultAudioManager;
    private List<Sound> sounds = new List<Sound>();
    private Vector2 scrollPosition = Vector2.zero;


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
        if (defaultAudioManager == null)
        {
            defaultAudioManager = GameObject.FindGameObjectWithTag("AudioManager");
        }

        // Si no se encuentra el AudioManager o la lista de sonidos está vacía, inicializa la lista
        if (defaultAudioManager == null || sounds.Count == 0)
        {
            sounds = new List<Sound>();
        }
        // Si se encuentra un AudioManager en la escena
        else if (defaultAudioManager != null)
        {
            // Actualiza los datos de la ventana con la información del AudioManager
            AudioManager audioManager = defaultAudioManager.GetComponent<AudioManager>();

            // Actualiza el nombre del AudioManager en la ventana
            defaultAudioManagerName = audioManager.gameObject.name;

            // Actualiza la lista de sonidos en la ventana con los sonidos del AudioManager
            sounds = audioManager.sounds;
        }
        float windowWidth = EditorGUIUtility.currentViewWidth;

        float imageWidth = windowWidth;
        float imageHeight = 140;
        // Definir el área que contendrá los elementos desplazables
        Rect scrollArea = new Rect(0, 0, position.width - 16, position.height);

        // Iniciar el ScrollView con la posición de desplazamiento actual
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

        Texture2D creatorImage = EditorGUIUtility.Load("Assets/AudioManager/CreatorImage.png") as Texture2D;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(creatorImage, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        GUILayout.Label("CREATE AUDIO MANAGER", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        defaultAudioManagerName = EditorGUILayout.TextField(defaultAudioManagerName, GUILayout.Width(imageWidth/2), GUILayout.Height(40));
        
        if (GUILayout.Button("CREATE",GUILayout.Width(imageWidth/3),GUILayout.Height(40)))
        {
            CreateDefaultAudioManager();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        GUILayout.Label("CREATE NEW SOUND", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        newSoundName = EditorGUILayout.TextField(newSoundName, GUILayout.Width(imageWidth / 3));
        newSoundClip = EditorGUILayout.ObjectField(newSoundClip, typeof(AudioClip), false, GUILayout.Width(imageWidth / 3)) as AudioClip;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        newSoundType = (SoundType)EditorGUILayout.EnumPopup(newSoundType, GUILayout.Width(imageWidth / 3));
        newSoundVolume = EditorGUILayout.Slider(newSoundVolume, 0f, 1f, GUILayout.Width(imageWidth / 3));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        newSoundPlayOnAwake = EditorGUILayout.Toggle("Play On Awake", newSoundPlayOnAwake);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Create New Sound",GUILayout.Width(imageWidth),GUILayout.Height(40)))
        {
            CreateNewSound();
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.FlexibleSpace();
        GUILayout.Label("AUDIOS IN AUDIO MANAGER", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (defaultAudioManager != null)
        {
            AudioManager audioManager = defaultAudioManager.GetComponent<AudioManager>();

            List<Sound> soundsCopy = new List<Sound>(sounds); // Create a copy of the sounds list
            foreach (Sound sound in soundsCopy) // Iterate over the copy
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                GUILayout.FlexibleSpace();
                GUILayout.Label(sound.name);
                string newName = EditorGUILayout.TextField(sound.name, GUILayout.Width(imageWidth/3));
                if (newName != sound.name)
                {
                    sound.name = newName;
                }
                if (GUILayout.Button("DELETE", GUILayout.Width(imageWidth/3)))
                {
                    audioManager.RemoveSound(sound);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void CreateDefaultAudioManager()
    {
        if (defaultAudioManager != null)
        {
            Debug.LogWarning("AudioManager already exists in the scene. Delete it before creating a new one.");
            return;
        }

        defaultAudioManager = new GameObject(defaultAudioManagerName);
        defaultAudioManager.AddComponent<AudioManager>();
        defaultAudioManager.tag = "AudioManager";

        // Limpiar la lista de sonidos para evitar excepciones
        sounds.Clear();
    }

    void CreateNewSound()
    {
        if (defaultAudioManager == null || defaultAudioManager.GetComponent<AudioManager>() == null)
        {
            Debug.LogWarning("Default AudioManager not found. Please create it first.");
            return;
        }

        Sound newSound = new Sound();
        newSound.name = newSoundName;
        newSound.clip = newSoundClip;
        newSound.type = newSoundType;
        newSound.volume = newSoundVolume;

        AudioManager audioManager = defaultAudioManager.GetComponent<AudioManager>();
        if (audioManager != null)
        {
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