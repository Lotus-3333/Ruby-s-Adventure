﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public AudioClip collectedClip;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller)
        {
            if (controller.Health < controller._maxHealth)
            {
                controller.PlaySound(collectedClip);
                
                controller.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
