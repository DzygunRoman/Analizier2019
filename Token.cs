using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizier2019
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format(" Тип {0} Значение {1} ", Type, Value);
        }

        public enum TokenType//типы токенов встречающихся в анализируемом фрагменте кода программы
        {
            MAIN, BEGIN, END, LPAR, RPAR, INT, IDENTIFIER, NUMBER, COMMA, SEMICOLON, FLOAT, DOUBLE,
            EQUAL, IF, ELSE, PLUS, MINUS, MULTIPLY, DIVIDE, LESS, EQUALS, MORE, NETERM
        }
        static TokenType[] Delimiters = new TokenType[]
        {
            TokenType.PLUS, TokenType.MINUS, TokenType.MULTIPLY,
            TokenType.DIVIDE,TokenType.EQUAL, TokenType.MORE,
            TokenType.LESS, TokenType.COMMA, TokenType.LPAR, TokenType.RPAR

        };
        public static bool IsDelimiter(Token token)
        {
            return Delimiters.Contains(token.Type);
        }
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
            { "main", TokenType.MAIN },
            { "int", TokenType.INT },
            { "if", TokenType.IF },
            { "else", TokenType.ELSE },
            { "==", TokenType.EQUALS},
            { "float",TokenType.FLOAT},
            { "double", TokenType.DOUBLE}
        };
        public static bool IsSpecialWord(string word)//ключевое слово
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (SpecialWords.ContainsKey(word));
        }
        public static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
            { ';', TokenType.SEMICOLON },
            { ',', TokenType.COMMA},
            { '(', TokenType.LPAR },
            { ')', TokenType.RPAR },
            { '+', TokenType.PLUS },
            { '=', TokenType.EQUAL },
            { '>', TokenType.MORE },
            { '{', TokenType.BEGIN },
            { '}', TokenType.END },
            { '-', TokenType.MINUS},
            { '*', TokenType.MULTIPLY},
            { '/', TokenType.DIVIDE},
            { '<', TokenType.LESS}
        };
        public static bool IsSpecialSymbol(char ch)//разделитель
        {
            return SpecialSymbols.ContainsKey(ch);
        }
    }
}
