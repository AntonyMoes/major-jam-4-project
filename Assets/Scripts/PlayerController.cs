using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class PlayerController : MonoBehaviour {
    [SerializeField] Transform crosshair;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] InputDetector inputDetector;
    [SerializeField] float speed;

    public Respawn respawn;
    
    Vector2 _moveDirection;
    Vector2 _aimDirection;
    Vector2 _lastAimDirection;
    bool _shoot;

    Camera _mainCamera;
    Rigidbody2D _rb;
    

    void Start() {
        _lastAimDirection = transform.forward;
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        var health = GetComponent<Health>();
        health.OnDeath += () => {
            // gameObject.SetActive(false);
            gameObject.transform.position = (Vector2) respawn.transform.position;
            health.AddHealth(health.MaxHealth);
        };
    }

    float _cooldown;
    void Update() {
        GetInputs(inputDetector.State);
        
        Aim(_aimDirection);
        if (_shoot && _cooldown == 0) {
            Shoot(_aimDirection);
            _cooldown = 0.3f;
        }

        _cooldown = Mathf.Max(0, _cooldown - Time.deltaTime);
    }

    void GetInputs(InputDetector.EInputState inputState) {
        var prefix = "";
        if (inputState == InputDetector.EInputState.Controller) {
            prefix = InputDetector.ControllerPrefix;
        }
        
        var moveHorizontal = Input.GetAxisRaw(prefix + "MoveHorizontal");
        var moveVertical = Input.GetAxisRaw(prefix + "MoveVertical");
        _moveDirection = new Vector2(moveHorizontal, moveVertical);

        if (inputState == InputDetector.EInputState.MouseKeyboard) {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _aimDirection = mousePos - transform.position;
        }
        else {
            var lookHorizontal = Input.GetAxisRaw("ControllerLookHorizontal");
            var lookVertical = Input.GetAxisRaw("ControllerLookVertical");
            _aimDirection = new Vector2(lookHorizontal, lookVertical);
        }

        _shoot = Input.GetButton("Fire1");
    }

    void FixedUpdate() {
        Move(_moveDirection);
    }

    void Move(Vector2 direction) {
        if (direction == Vector2.zero) {
            _rb.velocity = Vector2.zero;
            return;
        }
        
        _rb.velocity = direction.normalized * speed;
    }

    void Aim(Vector2 direction) {
        if (inputDetector.State == InputDetector.EInputState.Controller && direction == Vector2.zero) {
            var velocity = _rb.velocity;
            direction = velocity == Vector2.zero ? _lastAimDirection : velocity.normalized;
        }
        _lastAimDirection = direction;

        crosshair.position = transform.position + (Vector3) direction;
    }
    
    void Shoot(Vector2 direction) {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileController>().Init(tag, direction.normalized);
    }
}
