using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource[] AudioFX;
    public void PlayAudio(int i)
    {
        Level.AddFX(AudioFX[i].gameObject, transform.position, Quaternion.identity);
    }
}
