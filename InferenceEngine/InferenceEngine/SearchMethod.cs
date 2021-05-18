using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public abstract class SearchMethod
    {
        protected string _shortName;
        protected string _longName;
        protected List<Clause> _hornClauses;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchMethod() { }

        /// <summary>
        /// Executes the search method and returns result as a string
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public abstract string Solve(string q);

        /// <summary>
        /// Short Name of search method. Used to find search Method
        /// </summary>
        public string ShortName
        {
            get
            {
                return _shortName;
            }
        }

        /// <summary>
        /// Long Name of the search method
        /// </summary>
        public string LongName
        {
            get
            {
                return _longName;
            }
        }

        /// <summary>
        /// Horn Clauses that belong to the Knowledge Base
        /// </summary>
        public List<Clause> HornClauses
        {
            get
            {
                return _hornClauses;
            }
        }

    }
}
