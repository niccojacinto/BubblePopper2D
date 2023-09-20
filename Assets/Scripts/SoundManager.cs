using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] sfxPop;
    [SerializeField]
    AudioClip sfxlevelup;


    AudioSource audioSource;
    static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            // If the instance does not exist, find it in the scene or create a new one
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();

                // If no instance exists in the scene, create a new GameObject with the script attached
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                    _instance = singletonObject.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Ensure there is only one instance of this object
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayPop()
    {
        int popIndex = Random.Range(0, sfxPop.Length);
        audioSource.PlayOneShot(sfxPop[popIndex]);
    }

    public void PlayLevelUp()
    {
        audioSource.PlayOneShot(sfxlevelup);
    }
}
