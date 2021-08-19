using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Enum of possible attacks</summary>
    public enum EAttackId
    {
        Punch,
        Kick,
        HighPunch
    }

    /// <summary>Represent collision between an entity and an attack</summary>
    public class AttackCollision
    {
        /// <summary>Hit point</summary>
        public Vector3 position;
    }

    /// <summary>Represent a phase of the current attack</summary>
    /// <remarks>Allow to keep track of entities already hit</remarks>
    public class AttackPhase
    {
        /// <summary>Entity who attacked</summary>
        public IEntity source;
        /// <summary>Id of the attack</summarry>
        public EAttackId attackId;
        /// <summary>List of entities hit by the attack</summary>
        public List<IEntity> targets = new List<IEntity>();
    }
}