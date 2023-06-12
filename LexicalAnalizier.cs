using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Analizier2019.Token;

namespace Analizier2019
{
    public class LexicalAnalizier
    {
        List<Token> tokens;
        public List<Token> Tokens
        {
            get { return tokens; }
            set { tokens = value; }
        }
        public List<Token> GetTokens(string filetext)
        {
            var strWithoutSpaces = filetext.Replace(" ", "");
            List<Token> tokens = new List<Token>();
            string identifier = "";
            string number = "";
            string numbers = "0123456789";//цифры от 0 до 9
            string letters = "abcdefghijklmnopqrstuvwxyz";//строка с ангийским алфавитом
            int count = 0;
            for (int i = 0; i < strWithoutSpaces.Length; i++)
            {
                char currentLexem = strWithoutSpaces[i];

                if (IsSpecialSymbol(currentLexem))//если  специальный символ
                {
                    if ((strWithoutSpaces[i] == '=') && (strWithoutSpaces[i + 1] == '='))
                    {
                        Token token4 = new Token(TokenType.EQUALS);
                        token4.Value = "==";
                        tokens.Add(token4);
                    }
                    Token token = new Token(SpecialSymbols[currentLexem]);
                    token.Value = currentLexem.ToString();
                    tokens.Add(token);
                }
                else if (letters.Contains(strWithoutSpaces[i]))//если буква
                {
                    count++;
                    identifier += strWithoutSpaces[i];
                    if ((IsSpecialWord(identifier)))//если ключевое слово
                    {

                        Token token1 = new Token(SpecialWords[identifier]);
                        token1.Value = identifier;
                        tokens.Add(token1);
                        identifier = "";
                        count = 0;
                    }
                    else if (!letters.Contains(strWithoutSpaces[i + 1]) && count < 8)//если идентификатор
                    {

                        Token token2 = new Token(TokenType.IDENTIFIER);
                        token2.Value = identifier;
                        tokens.Add(token2);
                        identifier = "";
                        count = 0;

                    }
                    else if (count > 8)
                    {
                        MessageBox.Show(" Превышено количество символов в имени переменной: " + identifier);
                        count = 0;
                    }
                }
                else if (numbers.Contains(strWithoutSpaces[i]))//если цифра
                {
                    number += strWithoutSpaces[i];
                    if (!numbers.Contains(strWithoutSpaces[i + 1]))
                    {
                        Token token3 = new Token(TokenType.NUMBER);
                        token3.Value = number;
                        tokens.Add(token3);
                        number = "";
                    }
                }
                else if ((strWithoutSpaces[i] != '\n') && (strWithoutSpaces[i] != '\r'))
                {
                    MessageBox.Show(Convert.ToString(" Введенный символ не опознан: " + strWithoutSpaces[i]));
                }
            }
            return tokens;
        }
    }
}
