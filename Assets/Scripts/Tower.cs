using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour
{
    public float VisibilityDistance = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var enemies = FindObjectsOfType<ZombieAI>();

            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(other.transform.position, enemy.transform.position) < VisibilityDistance)
                {
                    Tips.Instance.ShowTip("There are enemies. Press \"Enter\" button to enter in combat state. To quit combat state press that button again.");
                }
            }
        }
    }
}
