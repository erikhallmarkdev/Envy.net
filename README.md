# Envy.NET

** Note: This version of Envy is incomplete **

Envy is a node/value configuration language I designed for writing configuration files on my personal projects. It's a straight forward language with a straight forward syntax.

Envy has three types of data it can store *numbers, strings, and bools*. And it stores data in a hierarchy of *nodes, values, and items*.

The simplest thing Envy stores is the item, and item can hold one *number*, one *string*, or one *bool*.

Items are stored inside of values, a value is essentially an array, it can hold as many items as you'd like to put in it.

Values are stored inside of nodes, nodes can store values as well as nodes, and identifies each with a name. Each value in a node has a unique name, but there can be multiple nodes of the same name within a given node.

Here's some example Envy code.

```
node {
  nestedNode {
    value ( "string", 1, -1, 1.1, -1.1, true, false )
    value_2 ( "Hello, world!" )
  }

  nestedNode {
    value ( "Random string" )
  }

}
```
