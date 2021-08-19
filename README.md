# unity3d-meele2d-raycast-sample

![Unity](https://img.shields.io/badge/Unity-2021.1.16+-blue)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Nauja/unity3d-meele2d-raycast-sample/master/LICENSE)

Sample of a meele combat system done with raycasts.

![Preview](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/preview.gif)

This project is an example of how to write a meele combat system by using raycasts such as `Physics2D.OverlapCircle` to detect attack collisions.

## Table of contents:

- [Controls](#controls)
- [Animation Driven Hitbox and Events](#animation-driven-hitbox-and-events)
- [Attack Phase](#attack-phase)

## Controls

Check `Assets/Controls.inputactions` for the full list of controls available:

![Controls](https://github.com/Nauja/unity3d-meele2d-raycast-sample/raw/media/controls.png)

  * Left/Right arrow or Left analog stick: Move
  * Space or Bottom button: Punch
  * E or Left button: Kick
  * Ctrl or Up button: High punch

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

## Attack Phase



## Credits

Sprites are coming from [The Spriters Resource](https://www.spriters-resource.com/).

Font from [dafont](https://www.dafont.com/fr/great-fighter.font).

## License

Licensed under the [MIT](LICENSE) License.
