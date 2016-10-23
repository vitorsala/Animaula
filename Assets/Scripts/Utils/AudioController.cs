using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    public static AudioController _instance;
    public static AudioController SharedInstance {
        get {
            return _instance;
        }
    }

    private AudioSource bgmSource;
    private AudioSource uiSoundEffects;
    private AudioSource[] soundEffects;

    [Range(1,8)] public int numberOfChannels = 1;

    private bool _muted = false;
    public bool muted {
        get {
            return _muted;
        }
    }

    private const string MUTED_PREF_KEY = "mutedBGM";

    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
        if(PlayerPrefs.HasKey(MUTED_PREF_KEY)) {
            _muted = (PlayerPrefs.GetInt(MUTED_PREF_KEY) == 0 ? false : true);
        }
        else {
            PlayerPrefs.SetInt(MUTED_PREF_KEY, (_muted ? 1 : 0));
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        uiSoundEffects = gameObject.AddComponent<AudioSource>();

        soundEffects = new AudioSource[numberOfChannels];
        for(int i = 0; i < numberOfChannels; i++) {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].loop = false;
            soundEffects[i].playOnAwake = false;
        }
    }

    public void Mute() {
        _muted = true;
        bgmSource.mute = true;
    }

    public void Unmute() {
        _muted = false;
        bgmSource.mute = false;
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
    }

	public void ChangeBGMVolume(float volume){
		bgmSource.volume = volume;
	}

    public void ChangeMusic(AudioClip clip, bool reset = true) {
        if(bgmSource.clip == clip) return;
        float time = (reset ? 0 : bgmSource.time);
        bgmSource.clip = clip;
        bgmSource.time = time;
        bgmSource.Play();
    }
    
    public void PlaySoundEffect(AudioClip sound, int channel, float volume = 1f) {
        if(channel < 0 || channel >= soundEffects.Length) {
            Debug.LogError("Invalid channel number.");
            return;
        }
        if(_muted) return;

        AudioSource selected = soundEffects[channel];
        selected.clip = sound;
        selected.time = 0;
        selected.volume = volume;
        selected.Play();
    }

    public void PlayeUISoundEffect(AudioClip sound) {
        if(_muted) return;
        uiSoundEffects.clip = sound;
        uiSoundEffects.time = 0;
        uiSoundEffects.Play();
    }
}
