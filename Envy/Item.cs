using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvyConfig {

  /// <summary>
  /// What type of data is held in this value
  /// </summary>
  //TODO: Convert the number type to do something more efficent than store everything in a decmial.
  public class Item {
    public enum ValueType {
      Bool = 0, //Literal "true" or "false" in source. 
      Number = 1, //Any possible number 0-9.0-9
      String = 2 //String of characters, denoted by being surronded by quotation marks
    }  

    static Exception CannotConvertException = new Exception("Cannot convert to given type");

    public object value { get; protected set; }
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

    /*public static implicit operator float(Item value) {
      if (value.type == ValueType.Number) {
        return (float)value.value;
      }
      else {
        throw CannotConvertException;
      }
    }

    public static implicit operator int(Item value) {
      if (value.type == ValueType.Number) {
        return (int)value.value;
      }
      else {
        throw CannotConvertException;
      }
    }*/

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