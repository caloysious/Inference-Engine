//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace InferenceEngine
//{
//    public class TruthTable : SearchMethod
//    {

//        private Dictionary<string, bool[]> _tt; //Truth Table
//        private Dictionary<Clause, bool[]> _kB; //Knowledge Base implemented from horn clauses
//        private int _noOfModels; //no of models of KB

//        /// <summary>
//        /// Constructor intiralises Truth table and knowledge base
//        /// </summary>
//        /// <param name="hornClauses"></param>
//        public TruthTable(List<Clause> hornClauses)
//        {
//            _shortName = "TT";
//            _longName = "Truth Table";
//            _hornClauses = hornClauses;
//            _kB = new Dictionary<Clause, bool[]>();
//            _tt = new Dictionary<string, bool[]>();
//            CreateTruthTable();
//            CreateKB();
//            _noOfModels = 0;
//        }

//        /// <summary>
//        /// Creating a list of props that are in the KB
//        /// Ensuring no duplication 
//        /// </summary>
//        private void CreateTruthTable()
//        {
//            List<string> props = new List<string>();

//            //Creating a list of propositional symbols
//            foreach (Clause c in HornClauses)
//            {
//                if (!props.Contains(c.Head))
//                {
//                    props.Add(c.Head);
//                }

//                if (c.Body != null)
//                {
//                    for (int i = 0; i < c.Body.Length; i++)
//                    {
//                        if (!props.Contains(c.Body[i]))
//                        {
//                            props.Add(c.Body[i]);
//                        }
//                    }
//                }
//            }

//            //Temporary truth table initiated using no. of propositional symbols
//            bool[,] tempTT = new bool[props.Count, (int)Math.Pow(2, (props.Count))];

//            int count = 0; //(0 - n) n : no. of propositional symbols
//            int noOfTrues = 0;

//            for (int i = tempTT.GetLength(0) - 1; i >= 0; i--)
//            {
//                //noOfTrues is the number of Trues in a column (2^count) 
//                noOfTrues = (int)(Math.Pow(2, count));

//                // j controls the column
//                for (int j = 0; j < tempTT.GetLength(1); j = j + (noOfTrues * 2))
//                {
//                    int jtemp = j;

//                    for (int t = noOfTrues; t > 0; t--)
//                    {
//                        tempTT[i, jtemp] = true;
//                        jtemp++;
//                    }
//                }
//                //Increase count to match the no of trues in each column (2^count)
//                count++;
//            }

//            //Copying tempTT values to _tt under its relevant propositional symbols
//            for (int i = tempTT.GetLength(0) - 1; i >= 0; i--)
//            {
//                bool[] array = new bool[(int)Math.Pow(2, (props.Count))];
//                List<bool> coumnOftempTT = new List<bool>();
//                for (int j = 0; j < tempTT.GetLength(1); j++)
//                {
//                    coumnOftempTT.Add(tempTT[i, j]);
//                    array = coumnOftempTT.ToArray();
//                }
//                _tt.Add(props[i], array);
//            }
//        }

//        /// <summary>
//        /// Creates the knowledge base based on the horn clauses and truth table values
//        /// </summary>
//        private void CreateKB()
//        {
//            foreach (Clause c in HornClauses)
//            {
//                if (c.Body == null) //head only (e.g : a)
//                {
//                    bool[] bools = _tt[c.Head];
//                    _kB.Add(c, bools);
//                }
//                else //Body with single or multiple propositional symbols
//                {
//                    if (c.Body.Length == 1) //one propositional symbol in body
//                    {
//                        bool[] H1 = _tt[c.Head];
//                        bool[] B1 = _tt[c.Body[0]];
//                        bool[] implices = new bool[H1.Length];


//                        for (int i = 0; i < H1.Length; i++)
//                        {
//                            //implimenting implication
//                            if (H1[i] == false && B1[i] == true)
//                            {
//                                implices[i] = false;
//                            }
//                            else
//                            {
//                                implices[i] = true;
//                            }
//                        }
//                        _kB.Add(c, implices);

//                    }
//                    else //multiple propositional symbols in body
//                    {

//                        bool[] H1 = _tt[c.Head];
//                        bool[] and = new bool[H1.Length];
//                        bool[] value = new bool[H1.Length];

//                        //implimenting conjunction
//                        Dictionary<string, bool[]> Bodies = new Dictionary<string, bool[]>();
//                        foreach (string b in c.Body)
//                        {
//                            Bodies.Add(b, _tt[b]);
//                        }

//                        for (int i = 0; i < H1.Length; i++)
//                        {
//                            value[i] = true;
//                            foreach (string key in Bodies.Keys)
//                            {
//                                bool[] booleanArray = Bodies[key];
//                                value[i] = value[i] & booleanArray[i];
//                            }
//                            and[i] = value[i];
//                        }

//                        //implimenting implication
//                        bool[] implices = new bool[H1.Length];

//                        for (int k =0; k< H1.Length; k++)
//                        {
//                            if (H1[k] == false && and[k] == true)
//                            {
//                                implices[k] = false;
//                            }
//                            else
//                            {
//                                implices[k] = true;
//                            }
//                        }
//                        _kB.Add(c, implices);

//                    }
//                }
//            }

//        }

//        /// <summary>
//        /// Executes the search method and returns result as a string
//        /// </summary>
//        /// <param name="q"></param>
//        /// <returns></returns>
//        public override string Solve(string q)
//        {
//            if (_tt.ContainsKey(q)) //if querey is one of the propositon symbol
//            {
//                bool[] q_bool = _tt[q]; //find truth table values for q 
//                bool[] c = new bool[q_bool.Length];

//                for (int i = 0; i < q_bool.Length; i++)
//                {
//                    c[i] = true;
//                    foreach (Clause key in _kB.Keys)
//                    {
//                        bool[] booleanArray = _kB[key];
//                        c[i] = c[i] & booleanArray[i]; //check if in the given model all KB values are true
//                    }

//                    if ((c[i] == true) && (q_bool[i] == true)) //all KB values are true and querey value is true increase noOfModels by one
//                    {
//                        _noOfModels++;
//                    }
//                }

//                //Output dpending on the number of models 
//                if (_noOfModels == 0)
//                {
//                    return ("NO");
//                }
//                else
//                {
//                    return ("YES : " + _noOfModels);
//                }

//            }
//            else
//            {
//                return ("NO");
//            }


//        }
//    }
//}
