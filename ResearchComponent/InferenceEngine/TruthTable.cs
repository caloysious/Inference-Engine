using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable
    {
        private List<LogicalExpression> _kb;
        private string _q;
        private int _modelCount;
        private string _shortName;
        private string _longName;

        /// <summary>
        /// Constructor
        /// initialize
        /// </summary>
        public TruthTable(List<LogicalExpression> KB, string q)
        {
            _kb = KB;
            _q = q;
            _shortName = "gtt";
            _longName = "Generic Truth Table";
        }

        /// <summary>
        /// Returns all the symbols occured in a logical expression
        /// </summary>
        private List<string> ExtractSymbols(List<LogicalExpression> kB, string q)
        {
            List<string> symbols = new List<string>();
            List<string> notnull = new List<string>();

            // add query
            if (q != null)
            {
                symbols.Add(q);
            }

            // loop through each epression and get all symbols
            foreach (LogicalExpression l in kB)
            {
                // if it is a symbol
                if (l.Symbol != null)
                {
                    if (!symbols.Contains(l.Symbol))
                    {
                        symbols.Add(l.Symbol);
                    }
                }
                else // if it is a sentence
                {
                    notnull = ExtractSymbols(l.Children, null);
                    foreach (string s in notnull)
                    {
                        if (!symbols.Contains(s))
                        {
                            symbols.Add(s);
                        }
                    }
                }
            }
            return symbols;

        }

        /// <summary>
        /// initializing the truth table
        /// </summary>
        private bool TT_Entails(List<LogicalExpression> kB, string q)
        {
            List<string> symbols = ExtractSymbols(kB, q);
            Dictionary<string, bool> model = new Dictionary<string, bool>();
            return TT_CheckAll(kB, q, symbols, model);

        }

        /// <summary>
        /// extend a list of models until it completes a row
        /// then check the model if they entails KB
        /// then check if KB entails query
        /// increase number of models if KB entails query
        /// </summary>
        private bool TT_CheckAll(List<LogicalExpression> kB, string q, List<string> symbols, Dictionary<string, bool> model)
        {
            // if Empty ? (symbols)
            if (symbols.Count == 0)
            {
                //if PL - True ? (KB, model)
                if (PL_True(kB, model))
                {
                    // return PL - True ? (alpha, model)
                    if (PL_True(q, model))
                    {
                        // if the model of KB entals query 
                        _modelCount++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                // else return true
                else
                {
                    return true;
                }
            }
            else
            { 
                // P = First(symbols);
                // rest = Rest(symbols);
                string P = symbols[0];
                symbols.RemoveAt(0);
                // creating copy of symbols - dont wish to modify the current symbols
                // return (TT-Check-All(KB, alpha, rest, Extend(P, true, model)) and  (TT- Check - All(KB, alpha, rest, Extend(P, false, model)))
                return TT_CheckAll(kB, q, new List<string>(symbols), Extend(P, true, model)) && TT_CheckAll(KB, q, new List<string>(symbols), Extend(P, false, model));
            }
        }

        /// <summary>
        /// return the model key value
        /// </summary>
        private bool PL_True(string symbol, Dictionary<string, bool> model)
        {
            if (model.ContainsKey(symbol))
            {
                return model[symbol];
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// return the total result of the model if they entails the kb
        /// </summary>
        private bool PL_True(List<LogicalExpression> kB, Dictionary<string, bool> model)
        {
            bool endresult = true;
            bool result = true;

            foreach (LogicalExpression expression in kB)
            {
                result = result && PL_True(expression, model);
                if (result == false)
                {
                    endresult = result;
                    break;
                }
            }

            return endresult;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool PL_True(LogicalExpression expression, Dictionary<string, bool> model)
        {
            bool result = true;

            if (expression.Symbol != null)
            {
                result = PL_True(expression.Symbol, model);

            }
            else
            {

                // conjuction
                if (expression.Connective == "&")
                {
                    foreach (LogicalExpression child in expression.Children)
                    {
                        result = result && PL_True(child, model);
                    }
                }

                // disjunction
                if (expression.Connective == "||")
                {
                    result = false;
                    foreach (LogicalExpression child in expression.Children)
                    {
                        result = result || PL_True(child, model);
                    }
                }

                // implication
                if (expression.Connective == "=>")
                {
                    if (PL_True(expression.Children[0], model) && !PL_True(expression.Children[1], model))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

                // biconditional
                if (expression.Connective == "<=>")
                {
                    if (PL_True(expression.Children[0], model) == PL_True(expression.Children[1], model))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }

            }

            return result;
        }

        /// <summary>
        /// Extend the list of models by adding a new model
        /// </summary>
        private Dictionary<string, bool> Extend(string p, bool value, Dictionary<string, bool> model)
        {
            Dictionary<string, bool> newModel = new Dictionary<string, bool>(model);
            if (p != null)
            {
                newModel.Add(p, value);
            }

            return newModel;
        }

        /// <summary>
        /// return the result of TT_Entails
        /// </summary>
        public string Solve()
        {
            bool result = TT_Entails(this.KB, this.Q);
            if (result)
            {
                return ("YES" + ": " + _modelCount);
            }

            return "N0";
        }

        /// <summary>
        /// List of Logical Expression as the knowledge base
        /// </summary>
        public List<LogicalExpression> KB { get => _kb; set => _kb = value; }

        /// <summary>
        /// Query symbol
        /// </summary>
        public string Q { get => _q; set => _q = value; }

        /// <summary>
        /// Short Name of search method.
        /// </summary>
        public string ShortName { get => _shortName; }

        /// <summary>
        /// Long Name of the search method.
        /// </summary>
        public string LongName { get => _longName; }

    }
}
