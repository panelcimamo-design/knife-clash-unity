using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5.75f;
    public float sprintSpeed = 9.0f;
    public float jumpForce = 5f;
    public float groundDrag = 5f;
    public float airDrag = 2f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Weapon")]
    public Transform weaponHolder;
    public GameManager.Weapon currentWeapon;
    private float shootCooldown = 0f;
    private float reloadCooldown = 0f;

    [Header("Abilities")]
    public float ability1Cooldown = 0f;
    public float ability2Cooldown = 0f;
    public float ultimateCooldown = 0f;

    [Header("Combat")]
    public int health = 100;
    public int maxHealth = 100;
    public int armor = 0;
    public int credits = 800;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    private bool isSprinting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.drag = groundDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (GameManager.Instance != null)
        {
            currentWeapon = GameManager.Instance.GetWeapon(0);
        }
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleCombat();
        UpdateCooldowns();
        GroundCheck();
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility1();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility2();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            UseUltimate();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SwitchWeapon(i - 1);
            }
        }
    }

    void HandleMovement()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
            rb.drag = groundDrag;

            if (rb.velocity.magnitude > currentSpeed)
            {
                rb.velocity = rb.velocity.normalized * currentSpeed;
            }
        }
        else
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * 0.4f, ForceMode.Force);
            rb.drag = airDrag;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }

    void HandleCombat()
    {
        if (weaponHolder != null && currentWeapon != null)
        {
            weaponHolder.localPosition = new Vector3(0.3f, -0.2f, 0.5f);
        }
    }

    void Shoot()
    {
        if (shootCooldown <= 0 && currentWeapon != null && currentWeapon.ammo > 0)
        {
            shootCooldown = 1f / currentWeapon.fireRate;
            currentWeapon.ammo--;

            GameManager.Instance.PlayWeaponSound(System.Array.IndexOf(GameManager.Instance.weapons, currentWeapon));

            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.forward * 0.5f, transform.forward, out hit, 100f))
            {
                Debug.Log($"Hit: {hit.collider.name} for {currentWeapon.damage} damage");
            }
        }
    }

    void Reload()
    {
        if (reloadCooldown <= 0 && currentWeapon != null)
        {
            reloadCooldown = currentWeapon.reloadTime;
            int ammoNeeded = currentWeapon.maxAmmo - currentWeapon.ammo;
            currentWeapon.ammo = currentWeapon.maxAmmo;
        }
    }

    void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < GameManager.Instance.weapons.Length)
        {
            currentWeapon = GameManager.Instance.GetWeapon(weaponIndex);
            shootCooldown = 0;
        }
    }

    void UseAbility1()
    {
        if (ability1Cooldown <= 0)
        {
            ability1Cooldown = 6f;
            Debug.Log("Ability 1 used!");
        }
    }

    void UseAbility2()
    {
        if (ability2Cooldown <= 0)
        {
            ability2Cooldown = 8f;
            Debug.Log("Ability 2 used!");
        }
    }

    void UseUltimate()
    {
        if (ultimateCooldown <= 0)
        {
            ultimateCooldown = 30f;
            Debug.Log("Ultimate used!");
        }
    }

    void UpdateCooldowns()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        if (reloadCooldown > 0) reloadCooldown -= Time.deltaTime;
        if (ability1Cooldown > 0) ability1Cooldown -= Time.deltaTime;
        if (ability2Cooldown > 0) ability2Cooldown -= Time.deltaTime;
        if (ultimateCooldown > 0) ultimateCooldown -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = damage;
        if (armor > 0)
        {
            actualDamage = Mathf.RoundToInt(damage * 0.75f);
            armor -= 5;
        }

        health -= actualDamage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}
