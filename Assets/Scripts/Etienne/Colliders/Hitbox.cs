using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public enum EAgentType
    {
        Player,
        Enemy,
        Count
    }

    public class Hitbox : MonoBehaviour
    {
        [field: SerializeField] public bool CanHit { get; set; }
        [field: SerializeField] public bool CanReceiveHit { get; set; }
        [field: SerializeField] public EAgentType AgentType { get; set; } = EAgentType.Count;
        [field: SerializeField] public List<EAgentType> AffectedAgents { get; set; } = new List<EAgentType>();


        private bool OtherHitboxCanReceiveHit(Hitbox otherHitbox)
        {
            return CanHit &&
                otherHitbox.CanReceiveHit &&
                AffectedAgents.Contains(otherHitbox.AgentType);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("Hitbox OnCollisionEnter  " + gameObject.name);

            var otherHb = collision.gameObject.GetComponent<Hitbox>();
            if (otherHb == null)
            {
                //Debug.Log(collision.gameObject.name + " has no Hitbox script");
                return;
            }

            if (OtherHitboxCanReceiveHit(otherHb))
            {
                Vector2 contactPoint = collision.GetContact(0).point;
                //call FXManager

                switch (AgentType)
                {
                    case EAgentType.Player:
                        break;
                    case EAgentType.Enemy:
                        var enemy = GetComponent<EnemySystem.Enemy>();
                        if (enemy == null)
                        {
                            Debug.Log("enemy null");
                            return;
                        }

                        if (enemy.CanAttack())
                        {
                            enemy.ContactAttack(contactPoint);
                        }

                        break;
                    case EAgentType.Count:
                        break;
                    default:
                        break;
                }
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Entered ontrigger");
            var otherHb = other.gameObject.GetComponent<Hitbox>();
            if (otherHb == null)
            {
                //Debug.Log(other.gameObject.name + " has no Hitbox script");
                return;
            }

            //Debug.Log("There is a otherHB " + otherHb.name);

            if (OtherHitboxCanReceiveHit(otherHb))
            {
                switch (AgentType)
                {
                    case EAgentType.Player:
                        //must be a projectile
                        var enemy = other.gameObject.GetComponent<EnemySystem.Enemy>();
                        var projectile = GetComponent<WeaponSystem.Projectile>();
                        if (enemy == null || projectile == null)
                        {
                            //Debug.Log("enemy or projectile null");
                            return;
                        }
                        //if (enemy == null)
                        //{
                        //    Debug.Log("enemy null");
                        //    return;
                        //}
                        //if (projectile == null)
                        //{
                        //    Debug.Log("projectile null");
                        //    return;
                        //}
                        enemy.OnDamageTaken(projectile.OnHit());

                        break;
                    case EAgentType.Enemy:
                        break;
                    case EAgentType.Count:
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
