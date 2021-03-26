using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum BulletImpulse
{
    Weak,
    Medium,
    Strong
}

public class Weapon : MonoBehaviour
{
    public Transform Muzzle;
    public Projectile ProjectilePrefab;
    public float SpreadAngle = 0f;
    public BulletImpulse Impulse;
    
    private float impulse;
    private float fireRate;
    private float bulletSpeed;

    private float currentInterval;

    private void Start()
    {
        LoadWeaponSettings();
    }

    public void HandleShoot()
    {
        if (currentInterval >= fireRate)
        {
            float angle = SpreadAngle / 180f;
            Vector3 spreadDirection = Vector3.Slerp(Muzzle.forward, Random.insideUnitSphere, angle);
            Projectile projectile = Instantiate(ProjectilePrefab, Muzzle.position, Quaternion.LookRotation(spreadDirection));
            projectile.Shoot(bulletSpeed, impulse);

            currentInterval = 0;
        }

        currentInterval += Time.deltaTime;
    }

    private void LoadWeaponSettings()
    {
        var settings = Resources.Load<WeaponSettings>("WeaponSettings");

        switch (Impulse)
        {
            case BulletImpulse.Weak:
                impulse = settings.ImpactImpulse.Weak;
                break;
            case BulletImpulse.Medium:
                impulse = settings.ImpactImpulse.Medium;
                break;
            case BulletImpulse.Strong:
                impulse = settings.ImpactImpulse.Strong;
                break;
        }

        bulletSpeed = settings.BulletSpeed;
        fireRate = settings.FireRate;
    }
}
