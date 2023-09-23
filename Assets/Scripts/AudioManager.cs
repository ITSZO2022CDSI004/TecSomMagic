using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource asource;
    [SerializeField]
    bool state = false;
    Image skin;
    public Sprite play, pause;
    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
        skin = GetComponent<Image>();
    }

    public void SetState()
    {
        state = !state;
        if (state) PlayTrack();
        else PauseTrack();
    }

    public void PlayTrack()
    {
        asource.Play();
        skin.sprite = play;
    }

    public void PauseTrack()
    {
        asource.Pause();
        skin.sprite = pause;
    }
}