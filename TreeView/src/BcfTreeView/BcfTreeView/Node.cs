using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcfTreeView.Bcf;
using CalculationWorks.BusinessModel;

namespace BcfTreeView
{

    /// <summary>
    /// public node object
    /// </summary>
    public class Node
    {
        private readonly NodeRow _nodeRow;

        internal Node(NodeRow nodeRow)
        {
            _nodeRow = nodeRow ?? throw new ArgumentNullException(nameof(nodeRow));
        }

        /// <summary>
        /// The object node was created for
        /// </summary>
        public object NodeObject => NodeRow.NodeObject;

        /// <summary>
        /// Gets a value indicating the node is current (e.g. not deleted)
        /// </summary>
        public bool IsValid => NodeRow.RowState == BcfRowState.Valid;

        /// <summary>
        /// Gets the node text evaluated by <see cref="TreeView.NodeObjectToString"/>.
        /// </summary>
        /// <remarks>
        /// This property is computed and cached.
        /// </remarks>
        public string Text => NodeRow.Text;

        /// <summary>
        /// Gets or sets the node is expanded. 
        /// </summary>
        /// <remarks>
        /// The property has a <see cref="BcfTrigger{T}"/> to lazy load child nodes when it is expanded for the first time.
        /// </remarks>
        public bool Expanded
        {
            get => NodeRow.Expanded;
            set => NodeRow.Expanded = value;
        }

        /// <summary>
        /// Gets a value indicating the node can expand
        /// </summary>
        public bool CanExpand => NodeRow.CanExpand;


        public void ExpandNode(int levels = 0)
        {
            Expanded = true;
            IEnumerable<NodeRow> nodeRows = NodeRow.ChildNodeRows.Immutable;
            using (NodeRow.GetScope())
            {
                for (int i = 0; i < levels; i++)
                {
                    foreach (var row in nodeRows) row.Expanded = true;
                    nodeRows = nodeRows.SelectMany(r => r.ChildNodeRows.Immutable);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the node is selected.
        /// </summary>
        /// <remarks>
        /// This property wraps <see cref="NodeRow.IsSelected"/> which is computed but not cached. 
        /// </remarks>
        public bool IsSelected
        {
            get => NodeRow.IsSelected;
            set => NodeRow.IsSelected = value;
        }

        /// <summary>
        /// Gets the node sibling index.
        /// </summary>
        /// <remarks>
        /// This property is computed and cached.
        /// </remarks>
        public int SiblingIndex => _nodeRow.SiblingIndex;

        /// <summary>
        /// Gets the node level.
        /// </summary>
        /// <remarks>
        /// This property is computed and cached.
        /// </remarks>
        public int Level => _nodeRow.Level;

        internal NodeRow NodeRow => _nodeRow;

        public void Delete()
        {
            // may be already deleted by parent-node delete cascade
            if (IsValid)
            {
                using (NodeRow.GetScope())
                {
                    NodeRow.Unlink();
                    NodeRow.Delete();
                };
            }
        }

        public Font Font
        {
            get => NodeRow.Font;
            set => NodeRow.Font = value;
        }

        /// <summary>
        /// Get the child nodes
        /// </summary>
        public IEnumerable<Node> ChildNodes => NodeRow.ChildNodeRows.OrderBy(n => n.SiblingIndex).Select(r => r.Node);

        /// <summary>
        /// Gets the nuber of materialized child nodes
        /// </summary>
        public int ChildNodeCount => NodeRow.ChildNodeRows.Count;

        /// <summary>
        /// Gets the parent node
        /// </summary>
        public Node ParentNode => NodeRow.ParentNodeRow?.Node;

        /// <summary>
        /// Gets the previous sibling
        /// </summary>
        public Node PreviousSibling => NodeRow.PreviousSiblingRow?.Node;

        /// <summary>
        /// Gets the next sibling
        /// </summary>
        public Node NextSibling => NodeRow.NextSiblingRow?.Node;

        /// <summary>
        /// Gets a value indicating the node is visible (parent nodes are expanded)
        /// </summary>
        /// <remarks>
        /// This property is computed and cached.
        /// </remarks>
        public bool Visible => NodeRow.Visible;

        public Point Location => new Point(NodeRow.Left, NodeRow.Top);
    }

}
