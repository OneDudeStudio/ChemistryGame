using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    [SerializeField] private AudioSource _tick;

    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        _tick.volume = collision.impulse.magnitude;
        _tick.Play();
    }

}
