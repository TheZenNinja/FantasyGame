using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WorldButton : MonoBehaviour, IInteractable
{
    public UnityEvent onButtonPush;
    public AudioSource source;
    public void Interact()
    {
        onButtonPush.Invoke();
        source?.Play();
    }

}
