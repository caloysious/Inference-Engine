using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    // goal driven
    public class BackwardChaining : SearchMethod
    {
        private List<string> _agenda;
        private List<string> _entailed;
        private List<string> _alwaysTrue;

        public BackwardChaining(List<Clause> hornClauses)
        {
            _shortName = "BC";
            _longName = "Backward Chaining";
            _hornClauses = hornClauses;
            // net symbols to be processed
            _agenda = new List<string>();
            // processed symbols
            _entailed = new List<string>();
            // head only
            _alwaysTrue = new List<string>();

            foreach(Clause c in _hornClauses)
            {
                // check for empty body = head only
                // add to always true
                if(c.Body == null)
                {
                    _alwaysTrue.Add(c.Head);
                }
            }
        }

        public override string Solve(string q)
        {
            string result = "";
            // starts from goal
            _agenda.Add(q);
           
            while(!(_agenda.Count == 0))
            {
                // get symbols from the last
                // the last symbol is recently added symbols
                string p = _agenda[_agenda.Count - 1];
                _agenda.RemoveAt(_agenda.Count - 1);

                // we are going to process this symbols
                _entailed.Add(p);

                // if it is a always-true sumbols
                // we found the goal
                if (!_alwaysTrue.Contains(p))
                {
                    List<string> temp = new List<string>();

                    foreach (Clause c in _hornClauses)
                    {
                        // find the clause that has p has as head
                        if(c.Head == p)
                        {
                            if (c.Body != null)
                            {
                                // get symbols from the body
                                for (int i = 0; i < c.Body.Length; i++)
                                {
                                    temp.Add(c.Body[i]);
                                }
                            }                          
                        }
                    }

                    // if all symbols is processed,
                    // but no root then return false
                    if(temp.Count ==0 )
                    {
                        result = "NO";
                        return result;
                    }
                    else
                    {
                        for (int i = 0; i < temp.Count; i++)
                        {
                            // avoids duplication in 
                            //processed symbols(entailed) and next-processs(agenda) ymbols 
                            if (!_entailed.Contains(temp[i]))
                            {
                                if(!_agenda.Contains(temp[i]))
                                {
                                    _agenda.Add(temp[i]);
                                }

                            }
                        }
                    }

                }
            }

            // found root
            // print result from root to goal
            result = "YES: ";

            for (int i = _entailed.Count -1; i >= 0; i--)
            {
                if(i ==0)
                {
                    result = result + _entailed[i];
                }
                else
                {
                    result = result + _entailed[i] + ',' + ' ';
                }
            }

            return result;
        }
    }
}
