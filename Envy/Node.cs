using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EnvyConfig {
  public class Node {
    public static Exception InvalidNameException = new Exception("Given name is invalid");
    
    /// <summary>
    /// How many nodes and values are in the node
    /// </summary>
    public int Count { get { return values.Count + nodes.Count; } }
    /// <summary>
    /// How many nodes are in the node
    /// </summary>
    public int CountNodes { get { return nodes.Count; } }
    /// <summary>
    /// How many values are in the node
    /// </summary>
    public int CountValues { get { return values.Count; } }

    private Dictionary<string, Value> values = new Dictionary<string, Value>();
    private List<KeyValuePair<string, Node>> nodes = new List<KeyValuePair<string, Node>>();

    /// <summary>
    /// Get value with name, name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Value GetValue(string name) {
      return values[name];
    }

    /// <summary>
    /// Returns All values in the form of KeyValuePairs with their names
    /// </summary>
    /// <returns></returns>
    public KeyValuePair<string, Value>[] GetValuePairs() { //TODO: Improve this algorithm, it can probably be made far more efficient.
      var allValuePairs = new List<KeyValuePair<string, Value>>();

      foreach(var kvp in values) {
        allValuePairs.Add(kvp);
      }

      return allValuePairs.ToArray();

    }

    public bool TryGetValue(string name) {
      if (values.ContainsKey(name)) {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Check if value with name exists.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(string name, ref Value value) {
      if (values.ContainsKey(name)) {
        value = GetValue(name);
        return true;
      }

      return false;
      throw new NotImplementedException();
    }

    /// <summary>
    /// Get node with name, name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Node GetNode(string name) {
      List<Node> nds = new List<Node>();
      foreach(var nd in nodes) {
        if(nd.Key == name) {
          return nd.Value;
        }
      }

      return null;
    }

    public bool TryGetNode(string name) {
      foreach(var kvp in nodes) {
        if(kvp.Key == name) {
          return true;
        }
      }

      return true;
    }

    /// <summary>
    /// Check if Node with name exists
    /// </summary>
    /// <param name="name"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool TryGetNode(string name, ref Node node) {
      if (TryGetNode(name)) { 
        node = GetNode(name);
        return true;
      }

      return false;
    }

    /// <summary>
    /// Get all nodes in node
    /// </summary>
    /// <returns></returns>
    public Node[] GetNodes() {
      List<Node> nds = new List<Node>();
      foreach(var nd in nodes) {
        nds.Add(nd.Value);
      }
      return nds.ToArray();
    }

    /// <summary>
    /// Get all nodes in node with name, name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Node[] GetNodes(string name) {
      List<Node> nds = new List<Node>();

      foreach(var node in nodes) {
        if(node.Key == name) {
          nds.Add(node.Value);
        }
      }

      return nds.ToArray();
    }

    /// <summary>
    /// Returns All nodes in the form of KeyValuePairs with their names
    /// </summary>
    /// <returns></returns>
    public KeyValuePair<string, Node>[] GetNodePairs() {
      return nodes.ToArray();
    }

    /// <summary>
    /// Remove value, value
    /// </summary>
    /// <param name="value"></param>
    public void Remove(Value value) {
      foreach(var kvp in values) {
        if(kvp.Value == value) {
          values.Remove(kvp.Key);
          return;
        }
      }
    }

    /// <summary>
    /// Remove node, node
    /// </summary>
    /// <param name="node"></param>
    public void Remove(Node node) {
      foreach(var kvp in nodes) {
        if(kvp.Value == node) {
          nodes.Remove(kvp);
          return;
        }
      }
    }

    /// <summary>
    /// Add node, node with name, name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="node"></param>
    public void Add(string name, Node node = null) {
      if (Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z1-9_]*$")) {
        nodes.Add(new KeyValuePair<string, Node>(name, node)); 
      } else {
        throw InvalidNameException;
      }
    }

    /// <summary>
    /// Add value, value with name, name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void Add(string name, Value value) {
      if(Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z1-9_]*$")) {
        values[name] = value;
      } else {
        throw InvalidNameException;
      }
    }

    public static Node operator+(Node left, Value right) {
      Node node = left;
      left.Add("value" + left.values.Count, right);
      return node;
    }

    public static Node operator-(Node left, Value right) {
      Node node = left;
      node.Remove(right);
      return node;
    }

    public override string ToString() {
      return ToString(0);
    }

    public string ToString(int tabs) {
      string final = new string(' ', tabs * 2);

      foreach (var value in values) {
        final += value.Key + " (" + value.Value.ToString() + ")" + Environment.NewLine + new string(' ', tabs * 2);
      }
      final += Environment.NewLine;

      foreach(var node in nodes) {
        final += new string(' ', tabs * 2) + node.Key + " {" + Environment.NewLine + node.Value.ToString(tabs + 1) + Environment.NewLine + new string(' ', tabs * 2) + "}";
      }

      return final;
    }

  }
}
