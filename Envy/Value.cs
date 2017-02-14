using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvyConfig {

  /// <summary>
  /// Works as a single value, or an array of many items. First index can be accessed directly, the rest require an index
  /// </summary>
  public class Value : IEnumerable<Item> { //TODO: Make IEnumerable
    //public string Name { get; protected set; }
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

    public Item value {
      get {
        return GetValue(0);
      }
      set {
        values[0] = value;
      }
    }

    public Item this[int index] {
      get {
        return GetValue(index);
      }

      set {
        values[index] = value;
      }
    }

    public Item GetValue(int index) {
      return values[index];
    }

    public void SetValue(int index, Item value) {
      values[index] = value;
    }

    /*
    public void SetValue(int index, EnvyValue value) {
      
    }

    public void SetValue(EnvyValue value) {
      SetValue(0, value);
    }

    */

    public void Remove(Item item) {
      values.Remove(item);
    }

    public void Remove(int index) {
      values.Remove(values[index]);
    }

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

    public static Value operator +(Value left, Item right) {
      Value val = left;
      val.Add(right);
      return val;
    }

    public static Value operator -(Value left, Item right) {
      Value val = left;
      val.Remove(right);
      return val;
    }
  }
}
