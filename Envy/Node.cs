using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EnvyConfig {
  public class Node {
    public static Exception InvalidNameException = new Exception("Given name is invalid");
    //public string Name { get; protected set; }

    public int Count { get { return values.Count + nodes.Count; } }
    public int CountNodes { get { return nodes.Count; } }
    public int CountValues { get { return values.Count; } }

    private Dictionary<string, Value> values = new Dictionary<string, Value>();
    private List<KeyValuePair<string, Node>> nodes = new List<KeyValuePair<string, Node>>();

    public Value GetValue(string name) {
      return values[name];
    }

    public Node GetNode(string name) {
      List<Node> nds = new List<Node>();
      foreach(var nd in nodes) {
        if(nd.Key == name) {
          return nd.Value;
        }
      }

      return null;
    }

    public Node[] GetNodes() {
      List<Node> nds = new List<Node>();
      foreach(var nd in nodes) {
        nds.Add(nd.Value);
      }
      return nds.ToArray();
    }

    public Node[] GetNodes(string name) {
      List<Node> nds = new List<Node>();

      foreach(var node in nodes) {
        if(node.Key == name) {
          nds.Add(node.Value);
        }
      }

      return nds.ToArray();
    }

    public void Remove(Value value) {
      foreach(var kvp in values) {
        if(kvp.Value == value) {
          values.Remove(kvp.Key);
          return;
        }
      }
    }

    public void Remove(Node node) {
      foreach(var kvp in nodes) {
        if(kvp.Value == node) {
          nodes.Remove(kvp);
          return;
        }
      }
    }

    public void Add(string name, Node node = null) {
      if (Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z1-9_]*$")) {
        nodes.Add(new KeyValuePair<string, Node>(name, node)); 
      } else {
        throw InvalidNameException;
      }
    }

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
