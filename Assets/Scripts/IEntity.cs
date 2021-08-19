using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Interface for player and enemies</summary>
    public interface IEntity
    {
        /// <summary>Called when this entity is hit by an attack</summary>
        void OnHitBy(AttackPhase attackPhase, AttackCollision attackCollision);
    }
}