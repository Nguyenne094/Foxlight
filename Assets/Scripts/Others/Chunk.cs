using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public event Action<Chunk> OnPlayerEnter;
    public event Action<Chunk> OnPlayerExit;
    
    public void RaiseEventPlayerEnter()
    {
        OnPlayerEnter?.Invoke(this);
    }
    
    public void RaiseEventPlayerExit()
    {
        OnPlayerExit?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RaiseEventPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RaiseEventPlayerExit();
        }
    }
}