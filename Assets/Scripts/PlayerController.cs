using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int _maxHealth = 5;
    private int _currentHealth;

    public float invincibleTotalFrozenTime = 2f;
    private float invincibleTimer;
    private bool isInvincible = false;

    public Transform respawnPosition;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private Vector2 _lookDirection = Vector2.down;
    private Vector2 _currentInput;

    private float _x;
    private float _y;

    public int Health => _currentHealth;

    public GameObject projectilePrefab;

    private GameObject Enemys;

    public int _maxAmmoCount = 30;
    private int _currentAmmoCount;
    
    public int AmmoCount => _currentAmmoCount;

    private AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip launchClip;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _currentHealth = 3;
        _currentAmmoCount = 2;
        
        UIManager.instance.UpdateHealthBar(_currentHealth, _maxHealth);
        UIManager.instance.UpdateAmmoAmountBar(_currentAmmoCount, _maxAmmoCount);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }
        
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(_x, _y);

        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            _lookDirection.Set(movement.x, movement.y);
            _lookDirection.Normalize();
        }

        _animator.SetFloat("Look X", _lookDirection.x);
        _animator.SetFloat("Look Y", _lookDirection.y);
        _animator.SetFloat("Speed", movement.magnitude);

        _currentInput = movement;
        if (Input.GetKeyDown(KeyCode.J) && _currentAmmoCount > 0)
        {
            ChangeAmmoAcount(-1);

            LaunchProjectile();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * 0.2f, _lookDirection, 2f, LayerMask.GetMask("NPC"));

            if (hit)
            {
                NPCController npcController = hit.collider.GetComponent<NPCController>();

                if (npcController)
                {
                    npcController.DisplayDialog();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject enemy = (GameObject)Resources.Load("Prefabs/Enemy" + Random.Range(1, 3));

            Enemys = Instantiate<GameObject>(enemy);

            Enemys.transform.position = new Vector2(Random.Range(-15, 15), Random.Range(-8, 11));
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = _rigidbody2D.position;
        position += _currentInput * speed * Time.deltaTime;
        _rigidbody2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return ;
            
            _animator.SetTrigger("Hit");

            PlaySound(hitClip);

            isInvincible = true;
            invincibleTimer = invincibleTotalFrozenTime;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        // print("Current Health: " + _currentHealth);

        UIManager.instance.UpdateHealthBar(_currentHealth, _maxHealth);

        if (_currentHealth == 0)
        {
            Respawn();
        }
    }

    public void ChangeAmmoAcount(int amount)
    {
        _currentAmmoCount = Mathf.Clamp(_currentAmmoCount + amount, 0, _maxAmmoCount);
        
        UIManager.instance.UpdateAmmoAmountBar(_currentAmmoCount, _maxAmmoCount);
    }

    private void Respawn()
    {
        ChangeHealth(_maxHealth);
        _rigidbody2D.position = respawnPosition.position;
    }

    private void LaunchProjectile()
    {
        _animator.SetTrigger("Launch");

        PlaySound(launchClip);

        GameObject projectileGameObject = null;

        if (_lookDirection == Vector2.down)
        {
            projectileGameObject = Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.down, Quaternion.identity);
        }
        else
        {
            projectileGameObject = Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
