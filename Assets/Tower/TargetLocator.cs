using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private ParticleSystem projectileParticles;
    [SerializeField] private float range = 45f;

    private Transform target;

    private void Start() {
        target = FindObjectOfType<Enemy>().transform;
    }

    private void Update() {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget() {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies) {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance) {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    private void AimWeapon() {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        Attack(targetDistance <= range);
        weapon.LookAt(target);
    }

    private void Attack(bool isActive) {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
