using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EnvyConfig {
  public static class Envy{

    static Exception InvalidCharacter = new Exception("Encountered unexpected character in parse");

    private static string nameMatch = @"^[a-zA-Z][a-zA-Z1-9_]*$";
    private static string nameCharMatch = @"^[a-zA-Z1-9_]+$";

    private static string numberMatch = @"-?[0-9]+(.[0-9]+)?";
    private static string numberSignleMatch = @"[0-9\-.]";

    private enum Lexeme { NAME, VALUE_BEGIN, VALUE_END, BLOCK_BEGIN, BLOCK_END, BOOL, STRING, NUMBER, NOTHING }

    private class Token{

      public Lexeme lexeme;
      public string value;

      public Token(Lexeme _lexeme, string _value) {
        lexeme = _lexeme;
        value = _value;
      }

    }

    public static Node FromSource(string source) {
      Node node = new Node();
      var tokens = tokenize(source);
      node = Parse(tokens.ToArray());
      return node;
    }

    private static Node Parse(Token[] tokens) {  
      Node final = new Node();

      string name = "";
      int i = 0;
      while(i < tokens.Length) {
        Token token = tokens[i];
        switch (token.lexeme) {
          case Lexeme.NAME:
            name = token.value;
            i++;
            break;
          case Lexeme.BLOCK_BEGIN:
            if(name == "") {
              Console.WriteLine($"Parse.Node: Invalid symbol { token.value }");
              break;
            }

            Node newNode;
            ParseNode(tokens, ref i, i + 1, out newNode);
            final.Add(name, newNode);
            name = "";
            break;
          case Lexeme.VALUE_BEGIN:
            if(name == "") {
              Console.WriteLine($"Parse.Value: Invalid symbol ${ token.value }");
              break;
            }
            Value value;
            ParseValue(tokens, ref i, i + 1, out value);
            final.Add(name, value);
            name = "";
            break;

          default:
            Console.WriteLine($"Parse: Invalid symbol { token.value } : { i }");
            i++;
            break;
        }
      }

      return final;
    }

    private static void ParseValue(Token[] tokens, ref int index, int start, out Value value) {
      value = new Value();

      for(int i = start; i < tokens.Length; i++) {
        Token token = tokens[i];
        switch (token.lexeme) {
          case Lexeme.BOOL:
            value.Add(token.value == "true" ? true : false);
            break;
          case Lexeme.NUMBER:
            if(Regex.IsMatch(token.value, numberMatch)) {
              double number = double.Parse(token.value);
              value.Add(number); 
            }
            break;
          case Lexeme.STRING:
            value.Add(token.value);
            break;
          case Lexeme.VALUE_END:
            index = i + 1;
            return;

          default:
            Console.WriteLine($"ParseValue: Invalid symbol { token.value }");
            break;
        }
      }
    }

    private static void ParseNode(Token[] tokens, ref int index, int start, out Node node) {
      node = new Node();

      string name = "";

      int i = start;
      while (i < tokens.Length) {
        Token token = tokens[i];
        switch (token.lexeme) {
          case Lexeme.NAME:
            name = token.value;
            i++;

            break;
          case Lexeme.BLOCK_BEGIN:
            if(name == "") {
              Console.WriteLine($"ParseNode.Node: Invalid symbol { token.value }");
            }
            Node newNode;
            ParseNode(tokens, ref i, i + 1, out newNode);
            node.Add(name, newNode);
            name = "";

            break;
          case Lexeme.VALUE_BEGIN:
            if(name == "") {
              Console.WriteLine($"ParseNode.Value: Invalid symbol { token.value }");
            }
            Value newValue;
            ParseValue(tokens, ref i, i + 1, out newValue);
            node.Add(name, newValue);
            name = "";

            break;
          case Lexeme.BLOCK_END:
            index = i + 1;
            return;
          default:
            Console.WriteLine($"ParseNode: Invalid symbol { token.value }");
            break;
        }
      }
    }

    private static List<Token> tokenize(string source) {
      var tokens = new List<Token>();
      int state = 0; //0 = start, 1 = text(litteral or name), 2 = string, 3 = number, 4 = comment, 5 = emit token
      Lexeme currentLex = Lexeme.NOTHING;
      string current = "";

      int i = 0;
      while (i < source.Length + 1) {
        char c =  i < source.Length ? source[i] : ' ';
        switch (state) {
          case 0: //Start

            if (char.IsWhiteSpace(c)) {
              i++;
              break;
            }
            
            if (isSpecial(c)) {
              switch (c) {
                case '{':
                  currentLex = Lexeme.BLOCK_BEGIN;
                  current = c.ToString();
                  i++;
                  state = 5;
                  break;
                case '}':
                  currentLex = Lexeme.BLOCK_END;
                  current = c.ToString();
                  i++;
                  state = 5;
                  break;
                case ')':
                  currentLex = Lexeme.VALUE_END;
                  current = c.ToString();
                  state = 5;
                  i++;
                  break;
                case ',':
                  i++;
                  break;
                case '(':
                  currentLex = Lexeme.VALUE_BEGIN;
                  current = c.ToString();
                  state = 5;
                  i++;
                  break;
                case '#':
                  currentLex = Lexeme.NOTHING;
                  state = 4;
                  i++;
                  break;
                case '"':
                  currentLex = Lexeme.STRING;
                  state = 2;
                  i++;
                  break;

                default:
                  Console.WriteLine($"Invalid character ${ c }");
                  throw new Exception("Invalid character: " + c); 
              }
            }

            if(Regex.IsMatch(c.ToString(), nameMatch)) {
              current += c;
              state = 1;
              i++;
            }

            if (Regex.IsMatch(c.ToString(), numberSignleMatch)) {
              current += c;
              currentLex = Lexeme.NUMBER;
              state = 3;
              i++;
            }

            break;
          case 1: //Text (Name or literal)
            if(Regex.IsMatch(c.ToString(), nameCharMatch)) {
              current += c;
              i++;
            } else {
              if (current == "true" || current == "false") {
                currentLex = Lexeme.BOOL;
              } else {
                currentLex = Lexeme.NAME;
              }

              state = 5;
            }
            break;

          case 2: //string
            if (c != '"') {
              current += c;
              i++;
            } else {
              if (IsEscaped(current)) { //Todo: Remove escape characters from string
                current += c;
                i++;
                break;
              }

              currentLex = Lexeme.STRING;
              state = 5;
              i++;
            }
            break;
          case 3: //number
            if(char.IsNumber(c) || c == '.') {
              current += c;
              i++;
            } else {
              currentLex = Lexeme.NUMBER;
              state = 5;
            }
            break;
          case 4: //comment
            if(c != '\n') {
              i++;
            } else {
              state = 0;
            }
            break;
          case 5: //emit token
            tokens.Add(new Token(currentLex, current));
            current = "";
            currentLex = Lexeme.NOTHING;
            state = 0;
            break;
        }
      }

      return tokens;
    }

    static bool IsEscaped(string text) {
      bool escaped = false;
      for(int i = text.Length - 1; i > 0; i--) {
        if (text[i] == '\\') {
          escaped = !escaped;
        } else {
          return escaped;
        }
      }
      return escaped;
    } 

    static bool isSpecial(char c) {
      switch (c) {
        case '{':
        case '}':
        case ')':
        case ',':
        case '(':
        case '#':
        case '"':
          return true;
        default:
          return false;

      }
    }
  }
}
