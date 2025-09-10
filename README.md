# Cubes
## Features
### Gameplay
- Main Cube: Left Click
  - Flash its own color
- Main Cube: Right Click
  - Random jiggle animation
  - All sub cubes flash Main Cube's color
- Sub Cube: Left Click
  - Main Cube plays sub cube's colors for a bit
### Editor Tool
Found at `Tools/CubeSnapEditor`
- Snap All Cubes
  - Aligns all cubes in Scene to a straight line
  - Changes all display to Main primary color
- Unsnap All Cubes
  - Returns all cubes to previous orientation (position and rotation)

Note: Changes can be applied at Editor time, Playmode, and persist across editor sessions.

## Implementation Details:
- `CubesController` manages the sub cube list and main cube.
  - it performs the Snap and UnSnap operations and listens for mouse clicks over the colliders
- Each cube has a `CubeController` component attached, which manages animations and input
- `CubeSnapEditor` is the window that provides buttons to do the Snap and UnSnap operations. It retrieves the appropriate game object by looking up via tag and then serializes it.

### Some Limitations
- Editor tool depends on Cubes tag being set
- Snap tool doesn't adjust for scale

## Next Steps
### LED Display
Here's the approach I'd take:
- Every cube would probably have a `GridController` attached to it
- Instantiate a 7x7 Texture2D for each cube
- Write the pixels using `Texture2D.SetPixels` and passing in an array of colors
- Assign the texture to the render material of the quad of the SubCube
- For the character display requirement (M, 1,2,etc) I would see about creating some small .img files with the characters to get the byte data from and then use it as a mask so that the flashing color would be applied to the background underneath and not the character (perhaps applying bitwise operations)

In the meantime I attached a Quad game object to be the front 'face' of the cube - the texture would be added to this Quad.

### Other notes
- I chose DoTween for animation package to reflect my would-be real-world decision.
  - Factors into decision:
    - the studio values quick iteration, so better use an established library than try to roll my own
    - Open source, well regarded
- In a production setting I'd be adding docs to the repo with system flow diagrams and adding automated tests
- I'd be structuring my git commits with feature branches and executing proper pull requests with detailed What Why How type-information after each feature is complete.
- I'd also go through my git commit history to ensure that each commit can be checked out without breaking the project

## Demo
https://github.com/user-attachments/assets/f1baa1dc-f0a5-41bc-a8a0-b0e784a89fb4

