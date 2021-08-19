using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Handle collisions between the attack hitbox and entities</summary>
    /// <remarks>Must be added on the collider</remarks>
    public class AttackHitBox : MonoBehaviour
    {
        /// <summmary>Hitbox radius</summary>
        /// <remarks>Driven by animation</remarks>
        [SerializeField]
        private float _radius;
        /// <summmary>Layer to hit</summary>
        [SerializeField]
        private LayerMask _layerMask;
        /// <summary>Current attack phase</summary>
        [HideInInspector]
        public AttackPhase attackPhase;

        private void Update()
        {
            // Only hit during attack phases
            if (attackPhase == null)
            {
                return;
            }

            // Search for colliding entities
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _layerMask);
            foreach (var hitCollider in hitColliders)
            {
                // IEntity allows to get any entity
                var entity = hitCollider.gameObject.GetComponent<IEntity>();
                if (entity == null || entity == attackPhase.source)
                {
                    continue;
                }

                // Entity already hit by this attack
                if (attackPhase.targets.Contains(entity))
                {
                    continue;
                }

                attackPhase.targets.Add(entity);

                // Notify entity
                var collision = new AttackCollision();
                collision.position = transform.position;
                entity.OnHitBy(attackPhase, collision);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}
