using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class InferenceEngine
    {
        private List<Clause> _hornClauses;
        private string _q;
        private string[] _clausestrings;

        /// <summary>
        /// Constructor takes in raw data and intialises clauses for KB and querey 
        /// </summary>
        /// <param name="data"></param>
        public InferenceEngine(string[] data)
        {
            _hornClauses = new List<Clause>();
            PopulateHornClauses(data);
        }

        /// <summary>
        /// Trimming string and splitting into seperate string of clauses
        /// </summary>
        /// <param name="data"></param>
        private void PopulateHornClauses(string[] data)
        {
            char[] trim = { ';' };

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].ToLower().Equals("tell"))
                {
                    _clausestrings = data[i + 1].Split(trim, StringSplitOptions.RemoveEmptyEntries);
                }
                else if (data[i].ToLower().Equals("ask"))
                {
                    _q = data[i + 1];
                }
            }

            //Creating a list of horn Clauses 
            foreach (string s in _clausestrings)
            {
                Clause c = new Clause(s);
                _hornClauses.Add(c);
            }
        }

        /// <summary>
        /// Horn Clauses or TELL
        /// </summary>
        public List<Clause> HornClauses
        {
            get
            {
                return _hornClauses;
            }
        }

        /// <summary>
        /// Query or ASK
        /// </summary>
        public string Q
        {
            get
            {
                return _q;
            }
        }


    }
}
