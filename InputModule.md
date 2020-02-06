Lodis Perkins

s188043

Complex Game Systems

Modular Complex System Brief

# I. Requirements

## Description of Problem

### Name: Button Input Script

### Purpose:

The purpose of this script is to create an easier way to bind user defined functions to unity their corresponding input axis.

### Format:

The exstension consists of two c# scripts for unity.

# II. Getting Started

First, start by adding the InputVariable script and the InputButtonBehaviour script in the assets folder of your unity project.

The UI displays the frame rate,playtime, and the current demonstration selected
A* Demo
| |:----------- | User Interface gif

Pursue, Evade, Arrive Demo
| |:----------- | User Interface gif |

Seek, Wander, Flee Demo
| |:----------- | User Interface gif |

II. Design
System Architecture

All objects inherit from the gameobject class. Each gameobject is responsible for drawing, updating and making decisions. Each gameobject has a behaviour associated with it from the objectbehaviour class. Based on the objects current behaviour, its velocity is changed accordingly. Nodes, have no velocity and therfore no behaviour. Instead, nodes positions are set based on their position in the graph. Whether the gameobjects are created as a graph or as agents depends on the user input. If A* is initialized, gameobjects become nodes and a graph is drawn to the screen showing the steps of A*. If seek behaviour is choosen, two agents are created that play an endless game of tag. If pursue behaviour is chosen, two agents are created. One is place at the bottom center of the screen and attempts to move to the top. The other is placed on the far left of the screen and attempts to catch the other by seeking where it will be instead of where it is. After it is caught the first agent then begins to evade; the two then rotate in a circle indefinitely as one evades while the other pursues.

Object Information

File Name: gameobjects.py

Name: Position(Vector2)
Description: The curremt position of the game object
Visibility: public
Name: velocity(Vector2)
Description: Current speed and direction of the gameobject
Visibility: public
Name: is_chaser(bool)
Description: Used in the seek and pursue demos to label the game object as the agent who should be pursuing or seeking
Visibility: public
Name: state(ObjectBehaviour)
Description: the current state the gameobject is in(seek,wander,flee,etc)
Visibility: public
Name: behaviour(Vector2)
Description: Stores the vector2 returned from the object behaviour functions(seek,flee,wander,etc)
Visibility: public
Name: maximum_speed(int)
Description: The maximum scale for the game objects velocity vector
Visibility: public
Name: draw()
Description: Draws the gameobject to the screen with a line representing its current velocity
Visibility: public
Arguments: screen- the surface to blit to
Name: update()
Description: Updates the gameobjects state,velocity,and position
Visibility: public
Arguments: gameobject(a gameobject used for the makedecision function),deltatime
Name: update()
Description: Adds the current behaviour to the velocity used to update the objects position
Visibility: public
Arguments: gameobject(a gameobject used for the makedecision function),deltatime
Name: check_position()
Description: Checks the current position of the object and places it within the bounds specified
Visibility: public
Arguments: bounds(the points the gameobject is not allowed to pass)
Name: make_decision()
Description: Changes behaviour, movement speed, and state based on the position of the gameobject
Visibility: public
Arguments: gameobject(decisions are ,ade based on the distance between the agent and a gameobject passed into the function)
File Name: game.py

Name: doing_astar(bool)
Description: Is true if the user presses the 'A' key meaning the a* demonstration is being displayed
Visibility: public
Name: doing_seek(bool)
Description: Is true if the user presses the 'S' key meaning the seek,wander,and flee demonstration is being displayed
Visibility: public
Name: doing_pursue(bool)
Description: Is true if the user presses the 'P' key meaning the pursue,evade,and arrive demonstration is being displayed
Visibility: public
Name: intialize_astar()

Description: Clears the screen, creates a graph, and sets up start, obstacle, and goal nodes
Visibility: public
Name: seek_behaviour()

Description: Clears the screen and initializes all values needed to demonstrate seek behaviour
Visibility: public
Name: pursue_behaviour()

Description: Clears the screen and initializes all values needed to demonstrate pursue behaviour
Visibility: public
File Name: graphobjects.py

Class Name: Node
Name: parent(Node)
Description: The node that the current node stems from
Visibility: public
Name: isobstacle(bool)
Description: Is true if the node is to represent an obstacle on the graph
Visibility: public
Name: isgoal(bool)
Description: Is true if the node is goal node for the path finding algorithm
Visibility: public
Name: ispath(bool)
Description: Is true if the node is on the path found by the path finding algorithm
Visibility: public
Name: isstart(bool)
Description: Is true if the node is the starting point for the path finding algorithm
Visibility: public
Name: isneighbor(bool)
Description: Is true if the node is a neighbor to any of the nodes on the path. (Used to change the color of neighboring nodes)
Visibility: public
Name: gridpos(tuple)
Description: The nodes position in the search space
Visibility: public
Name: G(int)
Description: The current nodes G score
Visibility: public
Name: H(int)
Description: The current nodes H score
Visibility: public
Name: F(int)
Description: The current nodes F score
Visibility: public
Name: set_parent()
Description: Sets the nodes parent to the node in the argument list
Visibility: public
Arguments: node(The current nodes parent)
Name: get_parent()
Description: Returns the nodes parent
Visibility: public
Arguments: none
Name: draw()
Description: Draws each node to the screen. Node color is based on the type of node
Visibility: public
Arguments: screen(surface to be drawn on),pos(position to draw at)
Class Name: Edge

Name: start(node)

Description: The node the edge starts from
Visibility: public
Name: end(node)

Description: The node the edge ends at
Visibility: public
Name: G(node)

Description: The cost from going from the start node to the end node
Visibility: public
Name: draw()

Description: Draws a line between nodes who share an edge
Visibility: public
Arguments: screen(surface to be drawn on)
Class Name: Graph

Name: nodes(node)

Description: List of all the nodes in the search space
Visibility: public
Name: edges(edge)

Description: List of all the edges in the search space
Visibility: public
Name: reconstructpath()

Description: Returns a list of nodes trailing from the goal nodes parent
Visibility: public
Arguments: start(node the search starts at),goal
Name: manhattan()

Description: Calculates manhattan distance between two nodes
Visibility: public
Arguments: node,goal
Name: sortnodes()

Description: Sorts a list of nodes based on their F score
Visibility: public
Arguments: nodelist
Name: drawneighbors()

Description: Draws all edges connecting the current node and its neighbors
Visibility: public
Arguments: screen,node
Name: createpath()

Description: Labels each node in the path list as a path and draws all of its edges
Visibility: public
Arguments: screen,start,goal
Name: a_star()

Description: Finds the shortest path between two nodes using the a* algorithm
Visibility: public
Arguments: start,goal
Name: initializegraph()

Description: Gives each node a corresponding graph position based on their order in the nodes list. An edge is created connecting each node to its neighbors based on graph position
Visibility: public
Arguments: none
Name: creategraph()

Description: Creates a graph with specified obstacles, a specified size and goal
Visibility: public
Arguments: size,obstacles(list),goal
Name: get_neighbors()

Description: Returns a list of edges containing the neigbors for the node
Visibility: public
Arguments: node
Name: draw()

Description: Draws each node at their graph position scaled to the appropriate screen position
Visibility: public
Arguments: screen
Name: bfs()

Description: Draws each node at their graph position scaled to the appropriate screen position
Visibility: public
Arguments: graph,startnode,goal
File Name: ObjectBehaviour.py

Name: seek()
Description: Returns a vector to be added to an agent to move towards another agent
Visibility: public
Arguments: vec1(current agent),vec2(goal agents position),maximum(maximum velocity scale)
Name: flee()
Description: Returns a vector to be added to an agent to move away from another agent
Visibility: public
Arguments: vec1(current agent),vec2(goal agents position),maximum(maximum velocity scale)
Name: wander()
Description: Returns a vector to be added to an agent to move seeingly aimlessly
Visibility: public
Arguments: vec1(current agent),oldcenter(previous vector from previous frame),maximum(maximum velocity scale)
Name: pursue()
Description: Returns a vector to be added to an agent to move towards agent
Visibility: public
Arguments: vec1(current agent),vec2(goal agents position),maximum(maximum velocity scale)
Name: avoid()
Description: Returns a vector to be added to an agent away from agent
Visibility: public
Arguments: vec1(current agent),vec2(goal agents position),maximum(maximum velocity scale)
Name: distance()
Description: Returns the distance between two vectors
Visibility: public
Arguments: vec1,vec2