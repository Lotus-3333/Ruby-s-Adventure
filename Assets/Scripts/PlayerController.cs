using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 5;
    private int _currentHealth;

    public float invicibleTotalFrozenTime = 2;
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

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _currentHealth = 1;
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

        _animator.SetFloat("lookX", _lookDirection.x);
        _animator.SetFloat("lookY", _lookDirection.y);
        _animator.SetFloat("speed", movement.magnitude);

        _currentInput = movement;
        if (Input.GetKeyDown(KeyCode.C))
        {
            LaunchProjectile();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
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
            
            isInvincible = true;
            invincibleTimer = invicibleTotalFrozenTime;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        print("Current Health: " + _currentHealth);

        if (_currentHealth == 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        _rigidbody2D.position = respawnPosition.position;
    }

    private void LaunchProjectile()
    {
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
}
