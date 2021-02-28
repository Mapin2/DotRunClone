using UnityEngine;
using UnityEngine.UI;
using DotRun.Utils;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private AudioClip normalSound = null;
    [SerializeField] private AudioClip changeColorSound = null;
    [SerializeField] private AudioClip wrongColorSound = null;
    [SerializeField] private AudioClip powerUpSound = null;

    [HideInInspector] public bool muted = false;

    [SerializeField] private Image[] soundUIs = null;
    [SerializeField] private Sprite spriteMuted = null;
    [SerializeField] private Sprite spriteUnmuted = null;

    public override void Awake()
    {
        base.Awake();

        int mutedValue = PlayerPrefs.GetInt(Constants.PLAYERPREF_AUDIO_MUTED, 0);
        muted = mutedValue == 0 ? true : false;
        MuteChecks();
    }

    public void ChooseAudio(InteractableType type)
    {
        switch (type)
        {
            case InteractableType.Dot:
                PlaySound(SoundType.Normal);
                break;
            case InteractableType.Triangle:
                PlaySound(SoundType.ChangeColor);
                break;
            case InteractableType.PowerUp:
                PlaySound(SoundType.PowerUp);
                break;
        }
    }

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Normal:
                source.clip = normalSound;
                break;
            case SoundType.ChangeColor:
                source.clip = changeColorSound;
                break;
            case SoundType.Hurt:
                source.clip = wrongColorSound;
                break;
            case SoundType.PowerUp:
                source.clip = powerUpSound;
                break;
        }

        if (!muted)
            source.Play();
    }

    public void MuteAudio()
    {
        muted = !muted;
        MuteChecks();
    }

    private void MuteChecks()
    {
        if (muted)
        {
            foreach (Image soundUI in soundUIs)
                soundUI.sprite = spriteMuted;
        }
        else
        {
            foreach (Image soundUI in soundUIs)
                soundUI.sprite = spriteUnmuted;
        }

        PlayerPrefs.SetInt(Constants.PLAYERPREF_AUDIO_MUTED, muted ? 0 : 1);
    }
}
