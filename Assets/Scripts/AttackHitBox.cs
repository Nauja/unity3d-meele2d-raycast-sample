using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Represent an attack hitbox that can hit other entities</summary>
    /// <remarks>This is done by using Physics2D.OverlapCircleAll in Update</remarks>
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
        /// <remarks>Prevent hitting the same entity multiple times</remarks>
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
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
