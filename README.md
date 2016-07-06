# Wired

First-person block-based simulator based around circuitry.

COMPLETED:
  - Rudimentary player controller
  - Generate block-based ground on game start
  - Place objects with right-click
  - Create mutable game objects which snap to adjacent objects
  - Maintain game object register based on coordinates of game objects in real time
  - Create power source model (currently non-functional)
  - Create rudimentary backpack for selection of game objects for placement (currently cycled with "[" and "]")

TODO:
  - Write player controller (current is very rudimentary)
  - Write shader to show exaggerated changes in wire temperature so player may visualize moving signal
  - Add ground model and functionality
  - When wires are placed, a path to connected grounds and power sources should be found and stored in a C# object
