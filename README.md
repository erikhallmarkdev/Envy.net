# Envy.NET

** Note: This version of Envy is incomplete **

## Using the Envy.NET library ##

Once you've build the .DLL from the source code provided in this repo, and you've referenced it in your project. Envy is contained in the `EnvyConfig` namespace.

Once you've set up your project to use Envy, you can start to use the language. First you'll want to create a `Node`, this is your root node, you may decide to load a .envy file from the filesystem into this node with `Envy.FromSource`, or you can begin configuring it directly in code. There are two things that you can add to your node, you can add a `Value`, or another `Node`.

~~~ C#

Node node = new Node();
Value value = new Value("Hello, world!");
node.Add("value", value);

Node secondNode = new Node();
node.Add("NestedNode", secondNode);

~~~

To get access from it's parent node, you can use the `GetNode` method, which takes a string, this string is the name that was assigned to the node when it was added to it's parent. You can also use the `GetNodes` method, which will return all nodes if no argument is passed, or if a string is passed, will return all nodes with that name.

To access a value that's stored in a node is very similar to the node. You can access a value with the `GetValue` method, which takes a string argument that denotes the name assigned to the value. Or you can use the `GetValues` method, which takes no arguments, and will return all values in the node.

The `Value` can be thought of as an array of a class called `Item`, you never have to worry about the item class if you don't want to, it can hold a string, bool, or number *(Currently held in a double)*, and it can implicitly convert to them. If the type to attempt to convert to is not the type held in the item, it will throw an exception. If you'd like to ensure that a Item holds the value that you expect, it contains an enum named `type`.

## Envy syntax ##

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

value ( "Lone value" )
```
