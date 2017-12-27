using System;
using System.Drawing;
using System.Linq;
using CalculationWorks.BusinessModel;
using JetBrains.Annotations;

namespace BcfTreeView.Bcf
{

    partial class NodeRow
    {

        [CanBeNull]
        private Node _node;

        [NotNull]
        public Node Node => _node ?? (_node = new Node(this));

        [CanBeNull]
        public NodeRow NextSiblingRow => NextSiblingRows.SingleOrDefault();

        [NotNull]
        public NodeRow FirstSibling
        {
            get
            {
                var rv = this;
                var current = this;
                while (current != null)
                {
                    rv = current;
                    current = current.PreviousSiblingRow;
                }
                return rv;
            }
        }

        [NotNull]
        public NodeRow LastSibling
        {
            get
            {
                var rv = this;
                var current = this;
                while (current != null)
                {
                    rv = current;
                    current = current.NextSiblingRow;
                }
                return rv;
            }
        }

        /// <summary>
        /// Removes a node from its location. 
        /// </summary>
        public void Unlink()
        {
            using (this.GetScope())
            {
                var nextSibling = NextSiblingRow;
                if (nextSibling != null) nextSibling.PreviousSiblingRow = PreviousSiblingRow;
                PreviousSiblingRow = null;
                ParentNodeRow = null;
                ControlRow = null;
                RootControlRow = null;
            }
        }
        

        public void SetAs(NodeTargetMode mode, [CanBeNull] NodeRow target)
        {
            using (this.GetScope())
            {
                if (target == null)
                {
                    var currentControlRow = Table.DataSet.ControlRow;
                    var firstRootNode = Table.DataSet.NodesTable.FirstOrDefault(n => n.ParentNodeRow == null && n.ControlRow == currentControlRow)?.FirstSibling;
                    if (mode == NodeTargetMode.FirstChild) Link(null, null, firstRootNode);
                    else Link(null, firstRootNode?.LastSibling, null);
                } else
                {
                    switch (mode)
                    {
                        case NodeTargetMode.LastChild:
                            Link(target, target.ChildNodeRows.FirstOrDefault()?.LastSibling, null);
                            break;
                        case NodeTargetMode.FirstChild:
                            Link(target, null, target.ChildNodeRows.FirstOrDefault()?.FirstSibling);
                            break;
                        case NodeTargetMode.PreviousSibling:
                            Link(target.ParentNodeRow, target.PreviousSiblingRow, target);
                            break;
                        case NodeTargetMode.NextSibling:
                            Link(target.ParentNodeRow, target, target.NextSiblingRow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                    }
                }
            }
        }

        private void Link([CanBeNull]NodeRow parentNodeRow, [CanBeNull]NodeRow previousSiblingRow, [CanBeNull]NodeRow nextSiblingRow)
        {
            using (this.GetScope())
            {
                Unlink();
                ParentNodeRow = parentNodeRow;
                if (parentNodeRow == null) RootControlRow = Table.DataSet.ControlRow;
                PreviousSiblingRow = previousSiblingRow;
                if (nextSiblingRow != null) nextSiblingRow.PreviousSiblingRow = this;
                ControlRow = Table.DataSet.ControlRow;
            }
        }

        public bool Contains(Point point)
        {
            return Top <= point.Y && Bottom >= point.Y && Left <= point.X && Right >= point.X;
        }

        public bool ContainsVertical(int y)
        {
            return Top <= y && Bottom >= y;
        }

        public bool IsExpandIndicatoLocation(int x)
        {
            return Left + TreeView.ExpandIndicatorOffsetX <= x && Left + TreeView.ExpandIndicatorOffsetX + TreeView.ExpandIndicatorSize >= x;
        }

        public Rectangle ToFocusRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left + 1, Top + 1, Right - 1, Bottom - 1);
        }

        /// <summary>
        /// Gets or sets a value indicating the node is selected.
        /// </summary>
        /// <remarks>
        /// The property is computed based on rows <see cref="TreeModelDataSet.ControlToSelectedNodesRelation"/>.
        /// </remarks>
        public bool IsSelected
        {
            get => SelectedControlRow != null;
            set => SelectedControlRow = value ? Table.DataSet.ControlRow : null;
        }

        [NotNull]
        internal NodeRow LastVisibleGrandChild
        {
            get
            {
                var current = this;
                var rv = this;
                while (current != null)
                {
                    rv = current;
                    current = current.Expanded ? current.ChildNodeRows.FirstOrDefault()?.LastSibling : null;
                }
                return rv;
            }
        }

        [CanBeNull]
        internal NodeRow FindPrevious()
        {
            if (PreviousSiblingRow != null) return PreviousSiblingRow.LastVisibleGrandChild;
            return ParentNodeRow;
        }

        [CanBeNull]
        internal NodeRow FindNext()
        {
            if (Expanded && ChildNodeRows.Any()) return ChildNodeRows.First().FirstSibling;
            if (NextSiblingRow != null) return NextSiblingRow;

            var current = this;
            while (current != null)
            {
                if (current.NextSiblingRow != null) return current.NextSiblingRow;
                current = current.ParentNodeRow;
            }
            return null;
        }

        internal Point ExpandIndicatorMid(Point expandIndicatorCenterOffset)
        {
            return new Point(Left + expandIndicatorCenterOffset.X, Top + expandIndicatorCenterOffset.Y);
        }
    }
}