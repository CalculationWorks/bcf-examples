using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcfTreeView
{

    public class GetChildNodeObjectsEventArgs : EventArgs
    {

        public Node Node { get; }

        internal GetChildNodeObjectsEventArgs(Node node)
        {
            Node = node;
        }

        public IEnumerable<object> ChildObjects { get; set; }
    }
}
