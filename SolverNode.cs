using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    //Node class for our mastermind solver
    class SolverNode:IComparable
    {
        //the code for mastermind
        int[] code;
        //the smallest number of possibilities 
        //this node could remove from possible solutions
        int score = 1296;
        
        public SolverNode(int[] code)
        {
            this.code = code;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SolverNode eq = obj as SolverNode;

            if ( eq.code == this.code)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int IComparable.CompareTo(object obj)
        {
            SolverNode other = (SolverNode)obj;
            return this.Score.CompareTo(other.Score);
        }        

        public int[] Code{
            get{return code;}
        }

        public int Score
        {
            get { return score; }
            set { this.score = value; }
        }
    }
}
