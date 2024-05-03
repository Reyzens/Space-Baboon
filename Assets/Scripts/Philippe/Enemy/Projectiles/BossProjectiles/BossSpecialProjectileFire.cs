using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class BossSpecialProjectileFire : BossSpecialProjectile
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            { 
                            
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

            }
        }

    }
}
