using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Respawn : MonoBehaviour {
    [SerializeField] Door[] doors;
    public bool _activated;
    public bool _completed;
    Health _playerHealth;

    void Start() {
        for (var i = 0; i < transform.childCount; i++) {
            var childObject = transform.GetChild(i).gameObject;

            if (childObject.TryGetComponent(out Health health)) {
                var startPosition = childObject.transform.position;
                health.OnDeath += () => {
                    childObject.transform.position = startPosition;
                };
            } else {
                throw new ApplicationException("Only killable objects should be children of a respawn object");
            }
            
            childObject.SetActive(false);
        }
    }

    void Update() {
        CheckCompletion();
    }

    void CheckCompletion() {
        if (!_activated) {
            return;
        }
        
        for (var i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.activeSelf) {
                return;
            }
        }

        _completed = true;
        
        foreach (var door in doors) {
            door.Set(false);
        }
    }

    public void Restart() {
        Debug.Log("Restart");
        for (var i = 0; i < transform.childCount; i++) {
            var childObject = transform.GetChild(i).gameObject;
            childObject.GetComponent<Health>().OnDeath?.Invoke();
            
            if(!_completed) {
                childObject.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player") || _activated) {
            return;
        }
        
        _activated = true;
        Bind(other.gameObject);
    }

    void Bind(GameObject player) {
        var playerController = player.GetComponent<PlayerController>();
        if (playerController.respawn) {
            playerController.respawn.Unbind();
        }
        playerController.respawn = this;

        _playerHealth = player.GetComponent<Health>();
        _playerHealth.OnDeath += Restart;

        if (_completed) {
            return;
        }
        
        foreach (var door in doors) {
            door.Set(true);
        }
        
        Restart();
    }

    void Unbind() {
        _playerHealth.OnDeath -= Restart;
    }
} 
