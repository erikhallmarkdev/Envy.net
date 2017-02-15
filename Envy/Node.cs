using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
