using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    private Transform target;

    private void Start() {
        target = FindObjectOfType<EnemyMover>().transform;
    }

    private void Update() {
        AimWeapon();
    }

    private void AimWeapon() {
        weapon.LookAt(target);
    }
}
