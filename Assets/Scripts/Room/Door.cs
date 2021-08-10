using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour {
    Collider2D _collider;

    void Start() {
        _collider = GetComponent<Collider2D>();
    }

    public void Set(bool state) {
        _collider.enabled = state;
    }
}
