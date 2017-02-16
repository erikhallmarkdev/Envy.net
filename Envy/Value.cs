using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvyConfig {

  /// <summary>
  /// Works as a single value, or an array of many items. First index can be accessed directly, the rest require an index
  /// </summary>
  public class Value : IEnumerable<Item> { 
    /// <summary>
    /// Number of items in the value
    /// </summary>
    public int Count { get { return values.Count;  } }

    private List<Item> values = new List<Item>();
    
    public Value(Item value) {
      Add(value);
    }

    /// <summary>
    /// This really shouldn't be used, but the parser needs it
    /// </summary>
    public Value() {

    }

    /// <summary>
    /// The item at index, 0
    /// </summary>
    public Item value {
      get {
        return GetValue(0);
      }
      set {
        values[0] = value;
      }
    }

    /// <summary>
    /// Item at index, index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item this[int index] {
      get {
        return GetValue(index);
      }

      set {
        values[index] = value;
      }
    }

    /// <summary>
    /// Get item at, index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item GetValue(int index) {
      return values[index];
    }

    /// <summary>
    /// Set item at, index to value
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void SetValue(int index, Item value) {
      values[index] = value;
    }

    /// <summary>
    /// Remove item, item
    /// </summary>
    /// <param name="item"></param>
    public void Remove(Item item) {
      values.Remove(item);
    }

    /// <summary>
    /// Remove the item at index, index
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index) {
      values.Remove(values[index]);
    }

    /// <summary>
    /// Add item, item
    /// </summary>
    /// <param name="item"></param>
    public void Add(Item item) {
      values.Add(item);
    }

    public IEnumerator<Item> GetEnumerator() {
      return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public override string ToString() {
      string final = " ";
      int index = 0;
      foreach(var ev in values) {
        index++;
        final += ev.ToString() + (index < values.Count ? ", " : "");
      }

      return final;
    }

    /// <summary>
    /// Add item, right to the value
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Value operator +(Value left, Item right) {
      Value val = left;
      val.Add(right);
      return val;
    }

    /// <summary>
    /// Removes item, right from the value
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Value operator -(Value left, Item right) {
      Value val = left;
      val.Remove(right);
      return val;
    }
  }
}
