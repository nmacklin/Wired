# Wired

First-person block-based simulator based around circuitry.

COMPLETED:
  - Rudimentary player controller
  - Generate block-based ground on game start
  - Place objects with right-click
  - Create mutable objects which snap to adjacent objects
  - Maintain object register based on coordinates of objects in real time
  - Create power source model (currently non-functional)

TODO:
  - Write player controller (current is very rudimentary)
  - Write shader to show exaggerated changes in wire temperature so player may visualize moving signal
  - Add ground model and functionality
  - When wires are placed, a path to connected grounds and power sources should be found and stored in a C# object
