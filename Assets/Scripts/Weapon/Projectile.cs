using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask HittableLayers = -1;

    public float GravityDownAcceleration = 0.1f;
    public float Radius = 0.01f;
    public float ImpactLife = 1f;
    public GameObject ImpactParticlePref;

    private Vector3 velocity;
    private Vector3 lastPosition;
    private float impactImpulse;

    private void Update()
    {
        this.transform.position += velocity * Time.deltaTime;

        if (GravityDownAcceleration > 0)
            velocity += Vector3.down * GravityDownAcceleration * Time.deltaTime;

        Vector3 direction = this.transform.position - lastPosition;
        RaycastHit[] hits = Physics.SphereCastAll(lastPosition, Radius, direction.normalized, direction.magnitude,
            HittableLayers, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
            OnHit(hit.point, hit.normal, hit.collider);

        lastPosition = this.transform.position;
    }

    public void Shoot(float speed, float impulse)
    {
        velocity = this.transform.forward * speed;
        impactImpulse = impulse;
    }

    private void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        var damageable = collider.GetComponentInParent<IDamageable>();

        if (damageable == null) return;

        damageable.OnDamage();

        var rgbodie = collider.GetComponent<Rigidbody>();

        if (rgbodie != null)
            rgbodie.AddForceAtPosition(velocity * impactImpulse, point + normal);

        if (ImpactParticlePref != null)
        {
            GameObject impactParticles = Instantiate(ImpactParticlePref, point + normal, Quaternion.LookRotation(normal));
           
            if (ImpactLife > 0)
                Destroy(impactParticles.gameObject, ImpactLife);
        }

        Destroy(this.gameObject);
    }
}
