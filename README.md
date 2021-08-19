# unity3d-meele2d-raycast-sample

![Unity](https://img.shields.io/badge/Unity-2021.1.16+-blue)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Nauja/unity3d-meele2d-raycast-sample/master/LICENSE)

Sample of a meele combat system done with raycasts.

![Preview](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/preview.gif)

This project is an example of how to write a meele combat system by using raycasts such as `Physics2D.OverlapCircle` to detect attack collisions and even allow a single attack to hit multiple times.

## Table of contents:

- [Controls](#controls)
- [Animation Driven Hitbox and Events](#animation-driven-hitbox-and-events)
- [AttackPhase](#attackphase)

## Controls

Check `Assets/Controls.inputactions` for the full list of controls available:

![Controls](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/controls.png)

  * Left/Right arrow or Left analog stick: Move
  * Space or Bottom button: Punch, a light attack that hits one time
  * E or Left button: Kick, a light attack that hits one time
  * Ctrl or Up button: High punch, an heavy attack that hits three times

## Animation Driven Hitbox and Events

Check [Assets/Textures/Ryu.png](Assets/Textures/Ryu.png) for the spritesheet containing all of Ryu's animations:

![Ryu](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/ryu-texture.png)

In the scene we have two game objects for the player:
  * `Player`: this is our controllable player with a renderer and a `BoxCollider2D` highlighted in green
  * `HitBox`: this is the hitbox used to detect attack collisions and it is rendered as a red sphere

![HitBox](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/hitbox.png)

Both the position and radius of the hitbox are driven in animation to match Ryu's animation:

![Events](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/animation-events.gif)

You can also see that there are multiple events called during the animation. Those are used to notify
the `PlayerController` script when:
  * `OnAttackBegin`: the attack begins and we block the player inputs so he can't move
  * `OnAttackPhaseBegin`: we enter a new phase of the attack
  * `OnAttackPhaseEnd`: the current phase of the attack ends
  * `OnAttackEnd`: the attack ends and we unblock the player inputs so he can move again

This is how all attack animations are configured.

## AttackPhase

All attacks are divided in one or multiple phases:

```csharp
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
```

Phases are driven by animation events handled in the `PlayerController` script:

```csharp
/// <summary>Player controller for handling inputs and physics</summary>
public class PlayerController : MonoBehaviour, IEntity
{
    ...
 
    [SerializeField]
    private AttackHitBox _attackHitBox;
    /// <summary>Current attack phase</summary>
    private AttackPhase _attackPhase;

    ...

    /// <summary>Called by animation when a new phase of attack begins</summary>
    private void OnAttackPhaseBegin()
    {
        // Create a new phase
        _attackPhase = new AttackPhase();
        _attackPhase.source = this;

        // Activate the hitbox
        _attackHitBox.attackPhase = _attackPhase;
        _attackHitBox.gameObject.SetActive(true);
    }

    /// <summary>Called by animation when the current phase of attack ends</summary>
    private void OnAttackPhaseEnd()
    {
        CancelAttackPhase();
    }
    
    ...
}
```

This is the hitbox that checks collisions with entities during attack phases:

```csharp
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

    ...
}
```

Each phase can only hit the same entity once, but a single attack can have multiple phases as demonstrated with the heavy punch.

## Credits

Sprites are coming from [The Spriters Resource](https://www.spriters-resource.com/).

Font from [dafont](https://www.dafont.com/fr/great-fighter.font).

## License

Licensed under the [MIT](LICENSE) License.
