using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class LogicalExpression
    {
        private string _symbol;
        private string _connective;
        private List<LogicalExpression> _children = new List<LogicalExpression>();
        private string _original;

        public LogicalExpression()
        {

        }

        public LogicalExpression(string sentence)
        {
            _original = sentence;
            //Console.WriteLine(sentence);
            sentence = sentence.Trim();
            if (sentence.Contains("<=>") || sentence.Contains("=>") || sentence.Contains("&") || sentence.Contains("~") || sentence.Contains("||"))
            {
                SentenceParser(sentence);
            }
            else
            {
                _symbol = sentence;
                _connective = null;
                _children = null;
            }
        }

        private void SentenceParser(string sentence)
        {
            int bracketCounter = 0; //number of brackets in a sentence
            int operatorIndex = -1;
            bool flag1 = true; //triggers
            bool flag2 = true; // triggers

            sentence.Trim();

            for (int i = 0; i < sentence.Length; i++)
            {
                char c = sentence.ElementAt(i);

                if (c == '(')
                {
                    bracketCounter++;
                }
                else if (c == ')')
                {
                    bracketCounter--;
                }C:\Users\vanew\Desktop\Assignment 2\ResearchComponent\InferenceEngine\LogicalExpression.cs
                else if ((c == '<') && bracketCounter == 0) // biconditional is found
                {
                    operatorIndex = i;
                    flag1 = false;
                    flag2 = false;
                }
                else if ((c == '=' && c + 1 == '>') && bracketCounter == 0 && flag2) //implication is found
                {
                    operatorIndex = i;
                    flag1 = false;
                    flag2 = false;
                }
                else if ((c == '&') && bracketCounter == 0 && flag1 && flag2) // conjunction is found
                {
                    operatorIndex = i;
                    flag1 = false;
                    flag2 = false;
                }
                else if ((c == '|' && c + 1 == '|') && bracketCounter == 0 && flag1 && flag2) // disjunction is found
                {
                    operatorIndex = i;
                    flag1 = false;
                    flag2 = false;
                }
                else if (c == '~' && bracketCounter == 0 && operatorIndex < 0 && flag1 && flag2) //neegation is found
                {
                    operatorIndex = i;
                }
            }

            if (operatorIndex < 0)
            {
                sentence = sentence.Trim();
                if (sentence.ElementAt(0) == '(' && sentence.ElementAt(sentence.Length - 1) == ')') 
                {
                    // recursive
                    SentenceParser(sentence.Substring(1, sentence.Length - 2));
                }
            }


            else
            {
                // biconditional
                if (sentence.ElementAt(operatorIndex) == '<') 
                {
                    string first = sentence.Substring(0, operatorIndex);
                    string second = sentence.Substring(operatorIndex + 3);
                    first = first.Trim();
                    second = second.Trim();
                    LogicalExpression child1 = new LogicalExpression(first);
                    LogicalExpression child2 = new LogicalExpression(second);
                    _children.Add(child1);
                    _children.Add(child2);
                    _connective = "<=>";

                }
                // implication
                else if (sentence.ElementAt(operatorIndex) == '=')
                {

                    string first = sentence.Substring(0, operatorIndex);
                    string second = sentence.Substring(operatorIndex + 2);
                    first = first.Trim();
                    second = second.Trim();
                    LogicalExpression child1 = new LogicalExpression(first);
                    LogicalExpression child2 = new LogicalExpression(second);
                    _children.Add(child1);
                    _children.Add(child2);
                    _connective = "=>";
                }
                // conjunction
                else if (sentence.ElementAt(operatorIndex) == '&')
                {
                    string first = sentence.Substring(0, operatorIndex);
                    string second = sentence.Substring(operatorIndex + 1);
                    first = first.Trim();
                    second = second.Trim();
                    LogicalExpression child1 = new LogicalExpression(first);
                    LogicalExpression child2 = new LogicalExpression(second);
                    _children.Add(child1);
                    _children.Add(child2);
                    _connective = "&";

                }
                // disjunction
                else if (sentence.ElementAt(operatorIndex) == '|')
                {

                    string first = sentence.Substring(0, operatorIndex);
                    string second = sentence.Substring(operatorIndex + 2);
                    first = first.Trim();
                    second = second.Trim();
                    LogicalExpression child1 = new LogicalExpression(first);
                    LogicalExpression child2 = new LogicalExpression(second);
                    _children.Add(child1);
                    _children.Add(child2);
                    _connective = "||";

                }
                // negation
                else if (sentence.ElementAt(operatorIndex) == '~')
                {
                    string first = sentence.Substring(operatorIndex + 1);
                    first = first.Trim();
                    LogicalExpression child = new LogicalExpression(first);
                    _children.Add(child);
                    _connective = "~";
                }

            }
        }

        public string Symbol
        {
            get
            {
                return _symbol;
            }
        }
        public string Connective
        {
            get
            {
                return _connective;
            }
        }

        public List<LogicalExpression> Children
        {
            get
            {
                return _children;
            }
        }

        public string OriginalString
        {
            get
            {
                return _original;
            }
        }
    }
}
