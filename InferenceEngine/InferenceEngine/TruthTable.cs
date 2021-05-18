using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable : SearchMethod
    {
        class Model
        {
            private string _symbol;
            private bool _value;

            public Model()
            {
                _symbol = "";
                _value = true;
            }

            public Model(string symbol, bool value)
            {
                _symbol = symbol;
                _value = value;
            }
            public string Symbol
            {
                get
                {
                    return _symbol;
                }
            }


            public bool Value
            {
                get
                {
                    return _value;
                }
            }
        }

        private int _noOfModels;

        public TruthTable(List<Clause> hornClauses)
        {
            _shortName = "TT";
            _longName = "Truth Table";
            _hornClauses = hornClauses;
            _noOfModels = 0;
        }


        private bool TT_Entails(List<Clause> hornClauses, Clause q, List<string> props)
        {
            bool result;
            List<Model> model = new List<Model>();
            // model is initially empty
            result = TT_CheckAll(hornClauses, q, props, model);
            return result;
        }

        private bool TT_CheckAll(List<Clause> hornClauses, Clause Q, List<string> props, List<Model> model)
        {
            // when a row of truth table is done
            if (props.Count == 0)
            {
                // checkif the row satisfy the knowledge base
                if (EvaluateClause(hornClauses, model))
                {
                    // check if the row entails query
                    if(IsQueryTrue(Q.Head, model))
                    {
                        // increase the number of models that entails query
                        _noOfModels++;
                        return true;
                    }

                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return true;
                }
               
            }

            else
            {
                string p = props[0];
                props.RemoveAt(0);

                // Extend model
                List<Model> ExtendTrue = Extend(p, true, model);
                List<Model> ExtendFalse = Extend(p, false, model);

                // recursive
                // copy of props, dont wish to modify the props
                bool result_t = TT_CheckAll(hornClauses, Q, new List<string>(props), ExtendTrue);
                bool result_f = TT_CheckAll(hornClauses, Q, new List<string>(props), ExtendFalse);

                return (result_t && result_f);
            }
        }

        private List<Model> Extend(string symbol, bool value, List<Model> aModel)
        {
            Model extend = new Model(symbol, value);
            // copy of aModel 
            // don't wish to modify the model
            List<Model> newModel = new List<Model>(aModel);
            newModel.Add(extend);

            return newModel;
        }

        // get the bool value of symbol from the current model
        private bool FindValue(string symbol, List<Model> model)
        {
            foreach (Model m in model)
            {
                if (m.Symbol == symbol)
                {
                    return m.Value;
                }
            }

            // fix: true && true = true
            // true && false = false
            // true will not affect the boolean value
            return true;
        }

        // return the value of query in the model 
        private bool IsQueryTrue(string query, List<Model> aModel)
        {
            foreach (Model m in aModel)
            {
                if (m.Symbol == query)
                {
                    return m.Value;
                }
            }
            return false;
        }

        //PL_true in pseudocode
        // the current model is evaluated if it satisfies the knowledge base
        private bool EvaluateClause(List<Clause> aClauses, List<Model> aModel)
        {
            bool endResult = true; 

            foreach (Clause clause in aClauses)
            {
                bool result = false;
                bool b = false;
                // horn clause has only one head
                bool h = FindValue(clause.Head, aModel);
                                
                // find body value
                if (clause.Body != null)
                {
                    // use implication rule
                    //(a=>b)
                    if (clause.Body.Length == 1)
                    {
                        // find b and h 
                        // h is found initially
                        b = FindValue(clause.Body[0], aModel);
                    }

                    //conjunction 
                    // if body has one conjuction
                    // a&b => c
                    else if (clause.Body.Length == 2)
                    {
                        bool b1 = FindValue(clause.Body[0], aModel);
                        bool b2 = FindValue(clause.Body[1], aModel);
                        b = Conjuction(b1, b2);
                    }

                    // conjuctions
                    // if body has more than one conjuction
                    // a&b&c&d=>e
                    else
                    {
                        b = Conjunctions(clause, aModel);
                    }

                    result = Implication(b, h);
                }
                // head only, no body
                else
                {                   
                    result = h;                   
                }

                // break if the result if false
                // don't continue to evaluate the current model
                // because it is not a model of KB
                if(!result)
                {
                    endResult = result;
                    break;
                }

                endResult = result;
            }           

            return endResult;
        }


        public override string Solve(string q)
        {
            Clause Q = new Clause(q);

            List<string> props = new List<string>();

            //Creating a list of propositional symbols
            foreach (Clause c in HornClauses)
            {
                if (!props.Contains(c.Head))
                {
                    props.Add(c.Head);
                }

                if (c.Body != null)
                {
                    for (int i = 0; i < c.Body.Length; i++)
                    {
                        if (!props.Contains(c.Body[i]))
                        {
                            props.Add(c.Body[i]);
                        }
                    }
                }
            }

            bool result = TT_Entails(HornClauses, Q, props);

            if (result)
            {
                return "YES:" + " " + _noOfModels.ToString();
            }

            return "NO";
        }

        // =>
        private bool Implication(bool left, bool right)
        {
            if (left == true && right == false)
            {
                return false;
            }

            return true;
        }

        // &
        private bool Conjuction(bool left, bool right)
        {
            return left && right;
        }

        // a&b&c
        private bool Conjunctions(Clause aClause, List<Model> aModel)
        {
            bool result = false;
            bool and = true;

            for (int i = 0; i < aClause.Body.Length; i++)
            {
                // avoids duplication and recycle code
                result = Conjuction(and, FindValue(aClause.Body[i], aModel));
            }

            return result;
        }
    }
}
