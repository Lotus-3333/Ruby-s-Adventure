using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemAmmo : MonoBehaviour
{
    public int _ammoAmount = 5;

    public AudioClip collectedClip;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller)
        {
            if (controller.AmmoCount < controller._maxAmmoCount)
            {
                controller.PlaySound(collectedClip);

                controller.ChangeAmmoAcount(_ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
