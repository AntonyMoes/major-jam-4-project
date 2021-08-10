using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Weapon))]
public class PlayerController : MonoBehaviour {
    [SerializeField] InputDetector inputDetector;
    [SerializeField] float speed;

    public Respawn respawn;
    
    Vector2 _moveDirection;
    Vector2 _aimDirection;
    bool _shoot;

    Camera _mainCamera;
    Rigidbody2D _rb;
    Weapon _weapon;
    

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        
        var health = GetComponent<Health>();
        health.OnDeath += () => {
            // gameObject.SetActive(false);
            gameObject.transform.position = (Vector2) respawn.transform.position;
            health.Init();
            _weapon.Init();
        };

        _weapon = GetComponent<Weapon>();
    }

    void Update() {
        GetInputs(inputDetector.State);
        
        _weapon.Aim(_aimDirection);
        if (_shoot) {
            _weapon.Shoot();
        }
    }

    void GetInputs(InputDetector.InputState inputState) {
        var prefix = "";
        if (inputState == InputDetector.InputState.Controller) {
            prefix = InputDetector.ControllerPrefix;
        }
        
        var moveHorizontal = Input.GetAxisRaw(prefix + "MoveHorizontal");
        var moveVertical = Input.GetAxisRaw(prefix + "MoveVertical");
        _moveDirection = new Vector2(moveHorizontal, moveVertical);

        if (inputState == InputDetector.InputState.MouseKeyboard) {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _aimDirection = mousePos - transform.position;
        }
        else {
            var lookHorizontal = Input.GetAxisRaw("ControllerLookHorizontal");
            var lookVertical = Input.GetAxisRaw("ControllerLookVertical");
            _aimDirection = new Vector2(lookHorizontal, lookVertical);
        }

        _shoot = Input.GetButton("Fire");
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
}
