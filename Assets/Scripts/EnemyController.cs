using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float timeToChangeDirection = 2f;
    public bool horizontal;

    private float _remainingTimer;
    private Vector2 _direction = Vector2.left;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private static readonly int lookX = Animator.StringToHash("lookX");
    private static readonly int lookY = Animator.StringToHash("lookY");

    private bool _repaired;
    public GameObject smokeEffect;
    public ParticleSystem fixedEffect;

    private AudioSource audioSource;
    public AudioClip fixedClip;

    // Start is called before the first frame update
    void Start()
    {
         _rigidbody2D = GetComponent<Rigidbody2D>();
         _animator = GetComponent<Animator>();

         _remainingTimer = timeToChangeDirection;
         _direction = horizontal ? Vector2.left : Vector2.down;

         audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_repaired)
            return ;

        _remainingTimer -= Time.deltaTime;

        if (_remainingTimer <= 0)
        {
            _remainingTimer = timeToChangeDirection;
            _direction *= -1;
        }

        _animator.SetFloat(lookX, _direction.x);
        _animator.SetFloat(lookY, _direction.y);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_repaired)
            return ;

        PlayerController controller = other.collider.GetComponent<PlayerController>();

        if (controller)
        {
            controller.ChangeHealth(-1);
        }
    }

    public void Fixed()
    {
        _animator.SetTrigger("fixed");

        PlaySound(fixedClip);

        smokeEffect.SetActive(false);

        Instantiate(fixedEffect, _rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

        _rigidbody2D.simulated = false;

        _repaired = true;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
