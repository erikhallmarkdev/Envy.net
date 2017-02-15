using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvyConfig {
  //TODO: Convert the number type to do something more efficent than store everything in a double.
  public class Item {
    /// <summary>
    /// Type of value (Bool, Number, or String)
    /// </summary>
    public enum ValueType {
      Bool = 0, //Literal "true" or "false" in source. 
      Number = 1, //Any possible number 0-9.0-9
      String = 2 //String of characters, denoted by being surronded by quotation marks
    }  

    static Exception CannotConvertException = new Exception("Cannot convert to given type");

    /// <summary>
    /// Value of the item
    /// </summary>
    public object value { get; protected set; }
    /// <summary>
    /// Type of item
    /// </summary>
    public ValueType type { get; protected set; }

    public Item(double val) {
      type = ValueType.Number;
      value = val;
    }

    public Item(bool val) {
      type = ValueType.Bool;
      value = val;
    }

    public Item(string val) {
      type = ValueType.String;
      value = val;
    }

    public static implicit operator double(Item value) {
      if (value.type == ValueType.Number) {
        return (double)value.value;
      }
      else {
        throw CannotConvertException;
      }
    }

    public static implicit operator string(Item value) {
      if (value.type == ValueType.String) {
        return (string)value.value;
      }
      else {
        throw CannotConvertException;
      }

    }

    public static implicit operator bool(Item value) {
      if (value.type == ValueType.Bool) {
        return (bool)value.value;
      }
      else {
        throw CannotConvertException;
      }
    }

    public static implicit operator Item(double value) {
      return new Item(value);
    }

    public static implicit operator Item(bool value) {
      return new Item(value);
    }

    public static implicit operator Item(string value) {
      return new Item(value);
    }

    public override string ToString() {

      switch (type) {
        case ValueType.Bool:
          return (bool)value ? "true" : "false";

        case ValueType.Number:
          return value.ToString();

        case ValueType.String:
          return '"' + value.ToString() + '"';
      }

      return value.ToString();
    }

  }
}