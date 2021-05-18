using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ForwardChaining : SearchMethod
    {
        private Dictionary<Clause, int> _count;
        private Dictionary<string, bool> _inferred;
        private List<string> _agenda;
        private List<string> _out;

        public ForwardChaining(List<Clause> hornClauses)
        {
            _shortName = "FC";
            _longName = "Forward Chaining";
            _hornClauses = hornClauses;
            // stores the number of unknown premises in a clause
            _count = new Dictionary<Clause, int>();
            // stores symbols, initally all the symbols are false
            _inferred = new Dictionary<string, bool>();
            // agenda stores all the always true
            _agenda = new List<string>();
            // output symbols
            _out = new List<string>();
        }

        public override string Solve(string q)
        {
            string result = "";

            foreach (Clause c in _hornClauses)
            {
                if (!_inferred.Keys.Contains(c.Head))
                {
                    _inferred.Add((c.Head), false);
                }

                // no body 
                // always true
                if (c.Body == null)
                {
                    _agenda.Add(c.Head);
                    _count.Add(c, 0);
                }
                else
                {
                    _count.Add(c, c.Body.Length);
                    for (int i = 0; i < c.Body.Length; i++)
                    {
                        if (!_inferred.Keys.Contains(c.Body[i]))
                        {
                            _inferred.Add((c.Body[i]), false);
                        }
                    }
                }

            }

            while (!(_agenda.Count == 0))
            {
                // get the next symbol
                string p = _agenda[0];

                if (!_out.Contains(p))
                {
                    _out.Add(p);
                }
                _agenda.RemoveAt(0);

                // set true
                _inferred[p] = true;

                foreach (Clause c in _hornClauses)
                {
                    if (c.Body != null)
                    {
                    //if the body has p then decrement count c
                    for (int i = 0; i < c.Body.Length; i++)
                        {
                            if (c.Body[i] == p)
                            {
                                _count[c]--;
                                // if count == 0 then checks if head has q
                                if (_count[c] == 0)
                                {
                                    // avoids duplication
                                    // save output symbols
                                    if (!_out.Contains(c.Head))
                                    {
                                        _out.Add(c.Head);
                                    }

                                    // found goal
                                    if (c.Head == q)
                                    {
                                        // print output symbols
                                        result = "YES: ";
                                        for (int o = 0; o < _out.Count; o++)
                                        {
                                            if (o == (_out.Count-1))
                                            {
                                                result = result + _out[o];
                                            }
                                            else
                                            {
                                                result = result + _out[o] + ',' + ' ';
                                            }
                                        }
                                        return result;
                                    }
                                    _agenda.Add(c.Head);

                                }
                            }
                        }
                    }
                    else // check for head-only clauses
                    {
                        // found goal
                        if (c.Head == q)
                        {
                            // avoids duplication
                            // save output symbols
                            if (!_out.Contains(c.Head))
                            {
                                _out.Add(c.Head);
                            }

                            result = "YES: ";
                            for (int i = 0; i < _out.Count; i++)
                            {
                                if (i == (_out.Count - 1))
                                {
                                    result = result + _out[i];
                                }
                                else
                                {
                                    result = result + _out[i] + ',' + ' ';
                                }
                            }
                            return result;
                        }
                    }

                }
            }

            result = "NO";
            return result;
        }
    }
}
