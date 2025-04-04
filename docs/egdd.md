# Enchanted Roots

## Elevator Pitch

Enchanted Roots is a point and click game designed to teach the player how to analyze and traverse binary search trees using search, insert, and delete actions. 

## Influences (Brief)

- *Influence #1*: Night in the Woods
  - Medium: Video Game
  - Explanation: NITW serves as an influence to Enchanted Roots mainly due to its artstyle. It contains cute, simplified character designs with a lineless drawing style. The backgrounds contain warm and cohesive colors that give off a cozy atmosphere. Everything is hand-drawn as well, adding to the immersive environment. The cat protagonist is also slightly influenced by this.
- *Influence #2*:  Fairy Tales
  - Medium: Literature
  - Explanation: The main story of Enchanted Roots is influenced by the classic fairy tales that happen in a whimsical setting. Most similarly to ‘Jack and the Beanstalk’, the forest in Enchanted Roots is magical and its trees are constantly growing. However, there are invasive plants that are affecting the health of the forest. 
- *Influence #3*: The Legend of Zelda: Ocarina of Time
  - Medium: Video Game
  - Explanation: The main influence of the bee sidekick character comes from the character Navi in the Legend of Zelda: Ocarina of Time. Here, you have a flying, tiny organism whose purpose is to aid the protagonist throughout the game. They are both told to do this by a force of nature (The Great Deku Tree, and the Enchanted Forest).

## Core Gameplay Mechanics (Brief)

- *Gameplay Mechanic #1*: Traverse the BST to search for the given node
- *Gameplay Mechanic #2*: Insert the new node in the correct place in the BST 
- *Gameplay Mechanic #3*: Remove specific nodes from the BST while maintaining the structure of the BST


# Learning Aspects


## Learning Domains

Learning in Data Structures, specifically Binary Search Trees.

## Target Audiences

Students in computer science.

## Target Contexts

Students in computer science courses that relate to binary search trees (e.g. computer algorithms) can play the game in their free time to practice and refine their knowledge on the topic.

## Learning Objectives

- By the end of the first section, players should be able to determine all nodes that are in the wrong spot in binary search trees that have a height of five or less.
- By the end of the game, players should be able to execute the sequence of actions required to perform a search, insert, and deletion on a binary search tree
- By the end of the second section, players should be able to apply the insertion of given nodes to any binary search tree that has a height of five or less.

## Prerequisite Knowledge

- *Prerequisite Learning Objective #1*: Demonstrate an understanding of Binary Search Trees and their functionality. 
- *Prerequisite Learning Objective #2*: Understand different Binary Search Tree traversal methods and how to insert and remove nodes while preserving the BST structure.

## Assessment Measures

Learning is assessed via the player’s ability to complete each level in the game. The further the player progresses, the stronger their understanding of BSTs becomes.

# What sets this project apart?

- *Reason #1* The concept is considerably original, given that it is very difficult to find any games about binary search trees
- *Reason #2* The game provides a fun, visual way to learn about the different functions of binary search trees.
- *Reason #3* It provides a straightforward learning process, with each section of the game teaching a different function of the binary search tree, eventually leading to a cumulation of everything they learned being challenged in the last section.

# Player Interaction Patterns and Modes

## Player Interaction Pattern


The game is single player, played by tapping and dragging different nodes on the binary search tree presented in front of them. They will utilize search, insertion, and deletion on the tree as instructed by the game. 

## Player Modes

- *Player mode #1*: Interacting with the binary search tree

# Gameplay Objectives

- *Primary Objective #1*:
    - Description: Identify and remove the invasive node from the tree
    - Alignment: This gameplay objective tests the player’s ability to perform the “delete” action on a binary search tree, which is an essential part of understanding a binary search tree
- *Primary Objective #2*:
    - Description: Grow new nodes to keep the tree healthy
    - Alignment: This gameplay objective tests the player’s ability to perform the “insert” action on a binary search tree, which is an essential part of understanding a binary search tree
- *etc.*

# Procedures/Actions

Players will be able to point and click in order to search, insert, and remove nodes from the tree.

# Rules

Players only get three mistakes when traversing/inserting/removing from the tree. If a player is stuck, they can get one hint related to the goal for that level. This means they must plan their traversal wisely or else the tree will be overrun with invasive plants.

# Objects/Entities

We need to design the cat protagonist, the bee sidekick, the enchanted trees, and the nodes that will be interacted with.  

## Core Gameplay Mechanics (Detailed)

- *Traverse the BST to Search for a Given Node*: On the binary search tree in front of them, players will be able to traverse the tree, dragging their mouse to different nodes of the tree. Each node will be highlighted in a way that makes it visible for the players to see that they are on the node. When they find the given node, the player will be able to click on that node.
- *Insert New Node in the Correct Place in the BST *: Players will be given a node on the side of the screen that they are instructed to insert into the Binary Search Tree. To insert the BST, players can click then drag the node to a spot in the BST. If it is the right spot, the node will stay there, if it is not, the node will fly back to the side of the screen.
- *Remove Node with Invasive Plants*: Players will be given one or more nodes to remove from the BST. They can do this by clicking the node and dragging it to the side of the screen. They then need to rearrange the tree to make it balanced again by clicking and dragging different nodes. After they complete their arrangement, they can click a button to check if what they did is right. If it is, everything stays, if not, nodes in the right spot will be highlighted in green and those that are not will be highlighted in red.


    
## Feedback

If players make three mistakes, the tree will be overrun by invasive plants. While the player progresses more through the game

# Story and Gameplay

## Presentation of Rules

The game’s controls are simply point and click. By hovering over a node with the mouse cursor, the node will be highlighted and accompanied by text, describing the action that would occur from the user selecting the node.

## Presentation of Content

The user will be able to visualize the sequence of their actions on the binary search tree, allowing them to visualize and understand the structure of a binary search tree.

## Story (Brief)

Cat protag, little bee sidekick that can pollinate branches (insert node leaf) and turn into a sword (remove node)
Cat wanders into forest and falls asleep, wakes up on top of a tree (Enchanted Roots caused it to grow overnight)
Little bee says that the forest cannot let this cat go until it helps it be freed from the invasive species in its branches (remove these invasive nodes)
Must locate the invasive node (search), kill it (remove) and make sure the tree stays intact (different types of deletions), and add new nodes to help keep the tree alive (insert)

## Storyboarding

![Storyboard 1](../Assets/StoryBoard1.jpg)
![Storyboard 2](../Assets/ConceptArt.jpg)

# Assets Needed

Need to have a tree, multiple circles to indicate the nodes of the tree, links(the lines that connect the nodes to each other). Also we need different scenes and scripts for the game as well. 

## Aesthetics

The game will include hand-drawn cutscenes, extensive lore of the forest and why its trees are enchanted, and whimsical music. It is set in a fantasy world where the trees grow extensively and you are a small cat amongst these magical trees. 

## Graphical

- Characters List
  - Small Cat: ![Small Cat](../Assets/CatChar.png)
  - Bee: ![Bee](../Assets/BeeChar.png)

## Audio

- Music List (Ambient sound)
- *Title Screen*: Cheery title screen placeholder
- *Gameplay*: Ambient calm music placeholder

- Sound List (SFX)
  - [\*Wrong Node\*](https://samplefocus.com/samples/amapiano-rim-snare-weird-2): \*Example 1\*, \*Example 2\*
  - [\*Tapping on a Node\*](https://samplefocus.com/samples/woodblock-acoustic-sample): \*Example 3\*, \*Example 4\*
  - [\*Right node(Putting node in the correct place)\*](https://samplefocus.com/samples/concertmate-clave-drum)


# Metadata

* Document created by Max Mazal <maxmazal@udel.edu>, Nyllise Graham <blu@udel.edu>, Sophia Romero <siromero@udel.edu>, Marc Madlangbayan <marcm@udel.edu>, 
* Template created by Austin Cory Bart <acbart@udel.edu>, Mark Sheriff, Alec Markarian, and Benjamin Stanley.
* Version 0.0.1
