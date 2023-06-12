using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Analizier2019.Token;

namespace Analizier2019
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public LR(List<Token> vvodtoken)
        {
            tokens = vvodtoken;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }
        private void Shift()
        {
            if (nextLex == tokens.Count)
                MessageBox.Show("Список лексем пуст, но анализ не был завершён.");
            if ((lexemStack.Count > 0) && (tokens[nextLex].Type == lexemStack.Peek().Type))
                MessageBox.Show($"После {ConvertLex(lexemStack.Peek().Type)} не может следовать {ConvertLex(GetLexeme(nextLex).Type)}.");
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
            Console.WriteLine(state);
        }
        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }
        public static string ConvertLex(TokenType type)
        {
            string s = "";
            switch (type)
            {
                case TokenType.IDENTIFIER:
                    s = "идентификатор";
                    break;
                case TokenType.NUMBER:
                    s = "литерал";
                    break;
                case TokenType.PLUS:
                    s = "+";
                    break;
                case TokenType.MINUS:
                    s = "-";
                    break;
                case TokenType.MULTIPLY:
                    s = "*";
                    break;
                case TokenType.DIVIDE:
                    s = "/";
                    break;
                case TokenType.EQUAL:
                    s = "=";
                    break;
                case TokenType.MORE:
                    s = ">";
                    break;
                case TokenType.LESS:
                    s = "<";
                    break;
                case TokenType.COMMA:
                    s = ",";
                    break;
                case TokenType.LPAR:
                    s = "(";
                    break;
                case TokenType.RPAR:
                    s = ")";
                    break;
                case TokenType.INT:
                    s = "int";
                    break;
                case TokenType.FLOAT:
                    s = "float";
                    break;
                case TokenType.DOUBLE:
                    s = "double";
                    break;
                case TokenType.BEGIN:
                    s = "{";
                    break;
                case TokenType.END:
                    s = "}";
                    break;
                case TokenType.IF:
                    s = "if";
                    break;
                case TokenType.ELSE:
                    s = "else";
                    break;
                case TokenType.EQUALS:
                    s = "==";
                    break;
                case TokenType.MAIN:
                    s = "main";
                    break;
                default:
                    break;
            }
            return s;
        }
        private void Error(string ojid)
        {
            if (lexemStack.Peek().Type == TokenType.NETERM)
                MessageBox.Show( $"Ожидалось {ojid}, но было получено {lexemStack.Peek().Value}. Состояние:{state}");            
            else
                MessageBox.Show($"Ожидалось {ojid}, но было получено {ConvertLex(lexemStack.Peek().Type)}. Состояние:{state}");            
        }
        private void State0()
        {            
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<прог>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        default:
                            Error("<прог>");
                            break;
                    }
                    break;
                case TokenType.MAIN:
                    GoToState(1);
                    break;
                default:
                    Error("<main>");
                    break;
            }
        }
        private void State1()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MAIN:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(2);
                    break;
                default:
                    Error("<main> <(>");
                    break;
            }
        }
        private void State2()
        {
            
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.RPAR:
                    GoToState(3);
                    break;
                default:
                    Error("<(> <)>");
                    break;
            }
        }
        private void State3()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    Shift();
                    break;
                case TokenType.BEGIN:
                    GoToState(4);
                    break;
                default:
                    Error("<)> <{>");
                    break;
            }
        }
        private void State4()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список операторов>":
                            GoToState(5);
                            break;
                        case "<оператор>":
                            GoToState(6);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<присвоение>":
                            GoToState(9);
                            break;
                        case "<условие>":
                            GoToState(7);
                            break;
                        case "<тип2>":
                            GoToState(11);
                            break;
                        case "<тип>":
                            GoToState(12);
                            break;
                        default:
                            Error("<список операторов> <оператор> <объявление> <присвоение> <условие> <тип2> <тип>");
                            break;
                    }
                    break;
                case TokenType.IF:
                    GoToState(10);
                    break;
                case TokenType.INT:
                    GoToState(13);
                    break;
                case TokenType.FLOAT:
                    GoToState(14);
                    break;
                case TokenType.DOUBLE:
                    GoToState(15);
                    break;                                   
                case TokenType.BEGIN:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.IDENTIFIER:
                            Reduce(0, "<тип>");
                            break;
                        case TokenType.INT:
                            Shift();
                            break;
                        case TokenType.FLOAT:
                            Shift();
                            break;
                        case TokenType.DOUBLE:
                            Shift();
                            break;
                        default:
                            Error("<int> <float> <double> <id>");
                            break;
                    }
                    break;
                default:
                    Error("<if> <int> <float> <double>");
                    break;
            }
        }
        private void State5()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список операторов>":
                            switch (GetLexeme(nextLex).Type)
                            {
                                case TokenType.END:
                                    Shift();
                                    break;
                                case TokenType.SEMICOLON:
                                    GoToState(17);
                                    break;
                                case TokenType.INT:
                                    Shift();
                                    break;
                                case TokenType.FLOAT:
                                    Shift();
                                    break;
                                case TokenType.DOUBLE:
                                    Shift();
                                    break;
                                case TokenType.IF: 
                                    Shift();
                                    break;
                                case TokenType.IDENTIFIER:
                                    Reduce(0, "<тип>");
                                    break;
                                case TokenType.NETERM:
                                    switch (GetLexeme(nextLex).Value)
                                    {
                                        case "<оператор>":
                                            GoToState(17);
                                            break;
                                    }
                                    break;
                                default:
                                    Error("<int> <float> <double> <id> <}>");
                                    break;
                            }
                            break;
                        case "<оператор>":
                            GoToState(17);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<присвоение>":
                            GoToState(9);
                            break;
                        case "<условие>":
                            GoToState(7);
                            break;
                        case "<тип2>":
                            GoToState(11);
                            break;
                        case "<тип>":
                            GoToState(12);
                            break;
                        default:
                            Error("<список операторов> <оператор> <объявление> <присвоение> <условие> <тип2> <тип>");
                            break;

                    }
                    break;
                case TokenType.IF:
                    GoToState(10);
                    break;
                case TokenType.INT:
                    GoToState(13);
                    break;
                case TokenType.FLOAT:
                    GoToState(14);
                    break;
                case TokenType.DOUBLE:
                    GoToState(15);
                    break;
                case TokenType.END:
                    GoToState(16);
                    break;
                default:
                    Error("<if> <int> <float> <double> <}>");
                    break;
            }
        }
        private void State6()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            Reduce(1,"<список операторов>");
                            break;                                
                        default:
                            Error("<оператор>");
                            break;
                    }
                    break;                    
            }
        }
        private void State7()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<условие>":
                            Reduce(1, "<оператор>");
                            break;
                        default:
                            Error("<условие>");
                            break;
                    }
                    break;
            }
        }
        private void State8()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<объявление>":
                            Reduce(1, "<оператор>");
                            break;
                        default:
                            Error("<объявление>");
                            break;
                    }
                    break;
            }
        }
        private void State9()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<присвоение>":
                            Reduce(1, "<оператор>");
                            break;
                        default:
                            Error("<присвоение>");
                            break;
                    }
                    break;
            }
        }
        private void State10()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IF:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(18);
                    break;
                default:
                    Error("<if> <(>");
                    break;
            }
        }
        private void State11()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип2>":
                            Shift();
                            break;
                        case "<список>":
                            GoToState(19);
                            break;
                        default:
                            Error("<тип2> <список>");
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(20);
                    break;
                default:
                    Error("<id>");
                    break;
            }
        }
        private void State12()
        {

            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            Shift();
                            break;
                        default:
                            Error("<тип>");
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(21);
                    break;
                default:
                    Error("<id>");
                    break;
            }
        }
        private void State13()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.INT:
                    Reduce(1, "<тип2>");
                    break;
                default:
                    Error("int");
                    break;
            }
        }
        private void State14()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.FLOAT:
                    Reduce(1, "<тип2>");
                    break;
                default:
                    Error("float");
                    break;
            }
        }
        private void State15()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.DOUBLE:
                    Reduce(1, "<тип2>");
                    break;
                default:
                    Error("double");
                    break;
            }
        }
        private void State16()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Reduce(6, "<прог>");
                    break;
                default:
                    Error("<}>");
                    break;
            }
        }
        private void State17()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            Reduce(2, "<список операторов>");
                            break;
                        default:
                            Error("<оператор>");
                            break;
                    }
                    break;
            }
        }
        private void State18()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог>":
                            GoToState(22);
                            break;
                        case "<операнд>":
                            GoToState(23);
                            break;
                        default:
                            Error("<лог> <операнд>");
                            break;
                    }
                    break;
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(24);
                    break;
                case TokenType.NUMBER:
                    GoToState(25);
                    break;
                default:
                    Error("<(> <id> <lit>");
                    break;
            }
        }
        private void State19()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список>":
                            Shift(); 
                            break;
                        default:
                            Error("<список>");
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(26);
                    break;
                default:
                    Error("<;>");
                    break;
            }
        }
        private void State20()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IDENTIFIER:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.COMMA: 
                            Shift();
                            break;
                        case TokenType.SEMICOLON:
                            Reduce(1,"<список>");
                            break;
                        default:
                            Error("<,> <;>");
                            break;
                    }
                    break;
                case TokenType.COMMA:
                    GoToState(27);
                    break;
                default:
                    Error("<,>");
                    break;
            }
        }
        private void State21()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {                
                case TokenType.IDENTIFIER:
                    Shift();
                    break;
                case TokenType.EQUAL:
                    GoToState(28);
                    break;
                default:
                    Error("<id> <=>");
                    break;
            }
        }
        private void State22()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог>":
                            Shift();
                            break;
                        default:
                            Error("<лог>");
                            break;
                    }
                    break;
                case TokenType.RPAR:
                    GoToState(29);
                    break;
                default:
                    Error("<)>");
                    break;
            }
        }
        private void State23()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                        case "<знак>":
                            GoToState(30);
                            break;
                        default:
                            Error("<операнд> <знак>");
                            break;
                    }
                    break;
                case TokenType.MORE:
                    GoToState(31);
                    break;
                case TokenType.LESS:
                    GoToState(32);
                    break;
                case TokenType.EQUALS:
                    GoToState(33);
                    break;
                default:
                    Error("> < ==");
                    break;
            }
        }
        private void State24()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {             
                case TokenType.IDENTIFIER:
                    Reduce(1,"<операнд>");
                    break;                
                default:
                    Error("<id>");
                    break;
            }
        }
        private void State25()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {                
                case TokenType.NUMBER:
                    Reduce(1, "<операнд>");
                    break;
                default:
                    Error("<lit>");
                    break;
            }
        }
        private void State26()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    Reduce(3,"<объявление>");
                    break;                
                default:
                    Error("<объявление>");
                    break;
            }
        }
        private void State27()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список>":
                            GoToState(34);
                            break;
                        default:
                            Error("<список>");
                            break;
                    }
                    break;
                case TokenType.COMMA:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(20);
                    break;
                default:
                    Error("<,> <id>");
                    break;
            }
        }
        private void State28()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<арифм>":
                            GoToState(35);
                            break;
                        case "<операнд>":
                            GoToState(36);
                            break;
                        default:
                            Error("<арифм> <операнд>");
                            break;
                    }
                    break;
                case TokenType.EQUAL:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(24);
                    break;
                case TokenType.NUMBER:
                    GoToState(25);
                    break;
                default:
                    Error("<=> <id> <lit>");
                    break;
            }
        }
        private void State29()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок операторов>":
                            GoToState(37);
                            break;
                        case "<оператор>":
                            GoToState(39);
                            break;
                        case "<условие>":
                            GoToState(7);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<присвоение>":
                            GoToState(9);
                            break;
                        case "<тип2>":
                            GoToState(11);
                            break;
                        case "<тип>":
                            GoToState(12);
                            break;
                        default:
                            Error("<блок операторов> <оператор> <условие> <объявление> <присвоение> <тип2> <тип>");
                            break;
                    }
                    break;
                case TokenType.RPAR:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.INT:
                            Shift();
                            break;
                        case TokenType.FLOAT: 
                            Shift();
                            break;
                        case TokenType.DOUBLE: 
                            Shift();
                            break;
                        case TokenType.IDENTIFIER:
                            Reduce(0,"<тип>");
                            break;
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(38);
                    break;
                case TokenType.IF:
                    GoToState(10);
                    break;
                case TokenType.INT:
                    GoToState(13);
                    break;
                case TokenType.FLOAT:
                    GoToState(14);
                    break;
                case TokenType.DOUBLE:
                    GoToState(15);
                    break;
                default:
                    Error("<{> <if> <int> <float> <double>");
                    break;
            }
        }
        private void State30()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак>":
                            Shift();
                            break;
                        case "<операнд>":
                            GoToState(40);
                            break;
                        default:
                            Error("<знак> <операнд>");
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(24);
                    break;
                case TokenType.NUMBER:
                    GoToState(25);
                    break;
                default:
                    Error("<id> <lit>");
                    break;
            }
        }
        private void State31()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {                
                case TokenType.MORE:
                    Reduce(1,"<знак>");
                    break;                
                default:
                    Error(">");
                    break;
            }
        }
        private void State32()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LESS:
                    Reduce(1,"<знак>");
                    break;
                default:
                    Error("<");
                    break;
            }
        }
        private void State33()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {                
                case TokenType.RPAR:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.EQUALS:
                            Reduce(1,"<знак>");
                            break;
                        default:
                            Error("<==>");
                            break;
                    }
                    break;
            }
        }
        private void State34()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список>":
                            Reduce(3,"<список>");
                            break;                        
                        default:
                            Error("<список>");
                            break;
                    }
                    break;                
            }
        }
        private void State35()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<арифм>":
                            Shift();
                            break;
                        default:
                            Error("<арифм>");
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(41);
                    break;
                default:
                    Error("<;>");
                    break;
                  
            }
        }
        private void State36()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            switch (GetLexeme(nextLex).Type)
                            {
                                case TokenType.PLUS:
                                    Shift();
                                    break;
                                case TokenType.MINUS:
                                    Shift();
                                    break;
                                case TokenType.MULTIPLY:
                                    Shift();
                                    break;
                                case TokenType.DIVIDE:
                                    Shift();
                                    break;
                                case TokenType.SEMICOLON:
                                    Reduce(1,"<арифм>");
                                    break;
                                default:
                                    Error("<+> <-> <*> </> <;>");
                                    break;
                            }
                            break;
                        case "<а.знак>":
                            GoToState(42);
                            break;
                        default:
                            Error("<операнд> <а.знак>");
                            break;
                    }
                    break;
                case TokenType.PLUS:
                    GoToState(43);
                    break;
                case TokenType.MINUS:
                    GoToState(44);
                    break;
                case TokenType.MULTIPLY:
                    GoToState(45);
                    break;
                case TokenType.DIVIDE:
                    GoToState(46);
                    break;
                default:
                    Error("<+> <-> <*> </>");
                    break;
            }
        }
        private void State37()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок операторов>":
                            switch (GetLexeme(nextLex).Type)
                            {
                                case TokenType.ELSE:
                                    Shift(); 
                                    break;
                                case TokenType.IF:
                                    Reduce(0,"<альт>");
                                    break;
                                case TokenType.IDENTIFIER:
                                    Reduce(0,"<альт>");
                                    break;
                                case TokenType.INT:
                                    Reduce(0, "<альт>");
                                    break;
                                case TokenType.FLOAT:
                                    Reduce(0, "<альт>");
                                    break;
                                case TokenType.DOUBLE:
                                    Reduce(0, "<альт>");
                                    break;
                                default:
                                    Error("<else> <if> <id> <int> <float> <double>");
                                    break;
                            }
                            break;
                        case "<альт>":
                            GoToState(47);
                            break;
                        default:
                            Error("<блок операторов> <альт>");
                            break;
                    }
                    break;
                case TokenType.ELSE:
                    GoToState(48);
                    break;
                default:
                    Error("<else>");
                    break;
            }
        }
        private void State38()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список операторов>":
                            switch (GetLexeme(nextLex).Type)
                            {
                                case TokenType.END:
                                    GoToState(49);
                                    break;
                                case TokenType.NETERM:
                                    switch (GetLexeme(nextLex).Value)
                                    {
                                        case "<оператор>":
                                            GoToState(5);
                                            break;
                                    }
                                    break;
                                case TokenType.IDENTIFIER:
                                    Reduce(0,"<тип>");
                                    break;
                                default:
                                    Error("<}> <оператор>");
                                    break;
                            }
                            break;
                            
                        case "<оператор>":
                            GoToState(6);
                            break;
                        case "<условие>":
                            GoToState(7);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<присвоение>":
                            GoToState(9);
                            break;
                        case "<тип2>":
                            GoToState(11);
                            break;
                        case "<тип>":
                            GoToState(12);
                            break;
                        default:
                            Error("<список операторов> <оператор> <объявление> <условие> <присвоение> <тип2> <тип>");
                            break;
                    }
                    break;
                case TokenType.BEGIN:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.INT:
                            Shift();
                            break;
                        case TokenType.FLOAT:
                            Shift();
                            break;
                        case TokenType.DOUBLE: 
                            Shift();
                            break;
                        case TokenType.IDENTIFIER: 
                            Reduce(0,"<тип>");
                            break;
                        default:
                            Error("<int> <float> <double> <id>");
                            break;
                    }
                    break;
                case TokenType.IF:
                    GoToState(10);
                    break;
                case TokenType.INT:
                    GoToState(13);
                    break;
                case TokenType.FLOAT:
                    GoToState(14);
                    break;
                case TokenType.DOUBLE:
                    GoToState(15);
                    break;
                default:
                    Error("<{> <if> <int> <float> <double>");
                    break;
            }
        }
        private void State39()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            Reduce(1,"<блок операторов>");
                            break;                        
                        default:
                            Error("<блок операторов>");
                            break;
                    }
                    break;                
            }
        }
        private void State40()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Reduce(3,"<лог>");
                            break;
                        default:
                            Error("<операнд>");
                            break;
                    }
                    break;
            }
        }
        private void State41()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    Reduce(5,"<присвоение>");
                    break;
                default:
                    Error("<;>");
                    break;
            }
        }
        private void State42()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<а.знак>":
                            Shift();
                            break;
                        case "<операнд>":
                            GoToState(50);
                            break;
                        default:
                            Error("<а.знак> <операнд>");
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(24);
                    break;
                case TokenType.NUMBER:
                    GoToState(25);
                    break;
                default:
                    Error("<id> <lit>");
                    break;
            }
        }
        private void State43()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.PLUS:
                    Reduce(1, "<а.знак>");
                    break;
                default:
                    Error("<+>");
                    break;
            }
        }
        private void State44()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MINUS:
                    Reduce(1,"<а.знак>");
                    break;
                default:
                    Error("<->");
                    break;
            }
        }
        private void State45()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MULTIPLY:
                    Reduce(1,"<а.знак>");
                    break;
                default:
                    Error("<*>");
                    break;           
            }
        }
        private void State46()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.DIVIDE:
                    Reduce(1, "<а.знак>");
                    break;
                default:
                    Error("</>");
                    break;
            }
        }
        private void State47()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<альт>":
                            Reduce(6, "<условие>");
                            break;
                        default:
                            Error("<альт>");
                            break;
                    }
                    break;
            }
        }
        private void State48()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок операторов>":
                            GoToState(51);
                            break;
                        case "<оператор>":
                            GoToState(39);
                            break;
                        case "<условие>":
                            GoToState(7);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<присвоение>":
                            GoToState(9);
                            break;
                        case "<тип2>":
                            GoToState(11);
                            break;
                        case "<тип>":
                            GoToState(12);
                            break;
                        default:
                            Error("<блок операторов> <оператор> <условие> <объявление> <присвоение> <тип2> <тип>");
                            break;
                    }
                    break;
                case TokenType.ELSE:
                    switch (GetLexeme(nextLex).Type)
                    {
                        case TokenType.BEGIN:
                            Shift();
                            break;
                        case TokenType.IF:
                            Shift();
                            break;
                        case TokenType.INT:
                            Shift();
                            break;
                        case TokenType.FLOAT: 
                            Shift();
                            break;
                        case TokenType.DOUBLE: 
                            Shift();
                            break;
                        case TokenType.IDENTIFIER:
                            Reduce(0,"<тип>");
                            break;
                        default:
                            Error("<if> <int> <float> <double> <id>");
                            break;
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(38);
                    break;
                case TokenType.IF:
                    GoToState(10);
                    break;
                case TokenType.INT:
                    GoToState(13);
                    break;
                case TokenType.FLOAT:
                    GoToState(14);
                    break;
                case TokenType.DOUBLE:
                    GoToState(15);
                    break;
                default:
                    Error("<else> <{> <if> <int> <float> <double>");
                    break;
            }
        }
        private void State49()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список операторов>":
                            Shift();
                            break;
                        default:
                            Error("<список операторов>");
                            break;
                    }
                    break;
                case TokenType.END:
                    GoToState(52);
                    break;
                default:
                    Error("<}>");
                    break;
            }
        }
        private void State50()
        {
            Console.WriteLine(lexemStack.Peek().Value);

            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Reduce(3,"<арифм>");
                            break;                  
                        default:
                            Error("<операнд>");
                            break;
                    }
                    break;               
            }
        }
        private void State51()
        {
            Console.WriteLine(lexemStack.Peek().Value);


            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок операторов>":
                            Reduce(2,"<альт>");
                            break;
                        default:
                            Error("<блок операторов>");
                            break;
                    }
                    break;                
            }
        }
        private void State52()
        {
            Console.WriteLine(lexemStack.Peek().Value);
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Reduce(3,"<блок операторов>");
                    break;
                default:
                    Error("<}>");
                    break;
            }
        }     
        public void Start()
        {
            stateStack.Push(0);
            while (isEnd != true)
                switch (state)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 3:
                        State3();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 7:
                        State7();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 15:
                        State15();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;                    
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                    case 40:
                        State40();
                        break;
                    case 41:
                        State41();
                        break;
                    case 42:
                        State42();
                        break;
                    case 43:
                        State43();
                        break;
                    case 44:
                        State44();
                        break;
                    case 45:
                        State45();
                        break;
                    case 46:
                        State46();
                        break;
                    case 47:
                        State47();
                        break;
                    case 48:
                        State48();
                        break;
                    case 49:
                        State49();
                        break;
                    case 50:
                        State50();
                        break;
                    case 51:
                        State51();
                        break;
                    case 52:
                        State52();
                        break;                   
                }
        }
    }
}
