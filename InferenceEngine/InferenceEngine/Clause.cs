using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Clause
    {
        private string _head;   // e.g : d4
        private string[] _body; // e.g : a1&b2&c3

        /// <summary>
        /// Constructor
        /// Takes a clause as a string and sperates body and head
        /// </summary>
        /// <param name="clause"></param>
        public Clause(string clause)
        {           
            char[] trim = { '=', '>'};
            

            //Removing empty spaces 
            clause = clause.Replace(" ", string.Empty);

            //Seperating head and body
            string[] text = clause.Split(trim, StringSplitOptions.RemoveEmptyEntries);

            if (text.Length == 2) //There is a body
            {
                _head = text[1];
                _body = text[0].Split('&', '^');
            }
            else //There is no body
            {
                _body = null;
                _head = text[0];
            }
        }

        /// <summary>
        /// Head of a clause
        /// Horn clauses always have one head
        /// </summary>
        public string Head
        {
            get
            {
                return _head;
            }
            set
            {
                _head = value;
            }
        }

        /// <summary>
        /// Body of a clause
        /// Horn clauses can have 1.No body 2.Single body 3.Multiple bodies
        /// </summary>
        public string[] Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }


    }
}
