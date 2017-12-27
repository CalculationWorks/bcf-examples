using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BcfTreeView.Bcf;
using BcfTreeView.Properties;
using CalculationWorks.BusinessModel;
using JetBrains.Annotations;

namespace BcfTreeView
{
    public partial class TreeView : UserControl
    {

        private static readonly Image ExpandIndicatorExpanded = Resources.Collapse_16xLG;
        private static readonly Image ExpandIndicatorCollapsed = Resources.Expand_16xLG;
        internal const int ExpandIndicatorOffsetX = -20;
        internal const int ExpandIndicatorSize = 16;

        private readonly TreeModelDataSet _model;

        public TreeView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();

            _model = new TreeModelDataSet();
            _model.TransactionFactory.EnforceConstraints = false;
            _model.TransactionFactory.MicroTransactionMode = BcfMicroTransactionMode.Disabled;

            var sf = new StringFormat(StringFormat.GenericTypographic)
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                Trimming = StringTrimming.EllipsisCharacter
            };
            sf.FormatFlags |= StringFormatFlags.LineLimit;

            _model.ControlRow.StringFormat = sf;
            _model.ControlRow.TreeView = this;
            ApplyControlSettings();

            ParentChanged += ApplyControlSettings;
            FontChanged += ApplyControlSettings;
            GotFocus += TreeView_GotOrLostFocus;
            LostFocus += TreeView_GotOrLostFocus;
            _model.DataChanged += _model_DataChanged;
        }

        private void TreeView_GotOrLostFocus(object sender, EventArgs e)
        {
            Invalidate();
        }

        #region hidden control properties

        private new bool AutoScroll { get => base.AutoScroll; set => base.AutoScroll = value; }

        private new Size AutoScrollMinSize { get => base.AutoScrollMinSize; set => base.AutoScrollMinSize = value; }

        private new bool AllowDrop { get => base.AllowDrop; set => base.AllowDrop = value; }

        #endregion

        private void _model_DataChanged(object sender, BcfDataChangedEventArgs e)
        {
            AutoScrollMinSize = _model.ControlRow.AutoScrollMinSize;
            Invalidate();
            var rootNodesRelation = _model.ControlToRootNodesRelation;
            if (e.ChangeSet.UpdatedChildRows.Any(c => c.ChildRelation == rootNodesRelation)) OnRootNodesChanged();
            var selectedRowsRelation = _model.ControlToSelectedNodesRelation;
            if (e.ChangeSet.UpdatedChildRows.Any(c => c.ChildRelation == selectedRowsRelation)) OnSelectedNodesChanged();
            var controlNodesRelation = _model.ControlToAllNodesRelation;
            if (e.ChangeSet.UpdatedChildRows.Any(c => c.ChildRelation == controlNodesRelation)) OnNodesChanged();
            _lastPropertiesChanged = e.ChangeSet.UpdatedCells.Count;
            _totalPropertiesChanged += e.ChangeSet.UpdatedCells.Count;
        }

        #region Collections


        [Category("TreeView")]
        public int NodesCount => _model.ControlRow.AllNodeRows.Count;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<Node> Nodes => _model.ControlRow.AllNodeRows.Select(r => r.Node);

        public event EventHandler NodesChanged;

        protected virtual void OnNodesChanged()
        {
            NodesChanged?.Invoke(this, EventArgs.Empty);
        }


        [Category("TreeView")]
        public int RootNodesCount => _model.ControlRow.RootNodeRows.Count;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<Node> RootNodes => _model.ControlRow.RootNodeRows.Select(r => r.Node);

        public event EventHandler RootNodesChanged;

        protected virtual void OnRootNodesChanged()
        {
            RootNodesChanged?.Invoke(this, EventArgs.Empty);
        }


        [Category("TreeView")]
        public int SelectedNodesCount => _model.ControlRow.SelectedNodes.Count;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<Node> SelectedNodes => _model.ControlRow.SelectedNodes.Select(r => r.Node);

        public event EventHandler SelectedNodesChanged;

        protected virtual void OnSelectedNodesChanged()
        {
            SelectedNodesChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        private void ApplyControlSettings(object sender, EventArgs e) => ApplyControlSettings();

        private void ApplyControlSettings()
        {
            using (_model.GetScope())
            {
                _model.ControlRow.StringMeasureGraphics = CreateGraphics();
                _model.ControlRow.Font = Font;
            };
        }

        public IEnumerable<Node> AddRootNodes(IEnumerable<object> nodeObjetcts)
        {
            var rv = new List<Node>();
            using (_model.GetScope())
            {
                foreach (var nodeObjetct in nodeObjetcts)
                {
                    var nodeRow = _model.NodesTable.AddNewRow();
                    nodeRow.NodeObject = nodeObjetct;
                    nodeRow.SetAs(NodeTargetMode.LastChild, null);
                    nodeRow.HasNonMaterializedChildren = GetChildNodeObjectsInternal(nodeRow.Node).Any();
                    rv.Add(nodeRow.Node);
                }
            };

            if (ActiveNode == null) SetActiveNode(rv.FirstOrDefault(), NodeSelectionMode.SelectExclusive);
            return rv;
        }

        public Node AddRootNode(object nodeObject) => AddRootNodes(new[] { nodeObject }).First();

        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<object, string> NodeObjectToString
        {
            get => _model.ControlRow.NodeObjectToString;
            set => _model.ControlRow.NodeObjectToString = value;
        }

        public event EventHandler<GetChildNodeObjectsEventArgs> GetChildNodeObjects;

        protected virtual void OnGetChildNodeObjects(GetChildNodeObjectsEventArgs e)
        {
            GetChildNodeObjects?.Invoke(this, e);
        }

        [NotNull]
        internal IEnumerable<object> GetChildNodeObjectsInternal([NotNull]Node node)
        {
            var e = new GetChildNodeObjectsEventArgs(node);
            OnGetChildNodeObjects(e);
            return e.ChildObjects ?? BcfArrayHelper.Empty<object>();
        }

        #region active node

        private enum NodeSelectionMode
        {
            // Don't change anything
            None,
            // ClearSelection and select node
            SelectExclusive,
            // select node
            Select,
            // toggle node IsSelected
            Toggle,
            // add nodes from active to clicked to selection
            SelectRange,
        }

        private Node _activeNode;

        [DefaultValue(null)]
        [Browsable(false)]
        public Node ActiveNode
        {
            get => _activeNode;
            set => SetActiveNode(value, NodeSelectionMode.SelectExclusive);
        }

        public event EventHandler ActiveNodeChanged;

        protected virtual void OnActiveNodeChanged()
        {
            ActiveNodeChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool SetActiveNode([CanBeNull]Node node, NodeSelectionMode selectionMode)
        {
            var clearSelection = selectionMode == NodeSelectionMode.SelectExclusive;
            using (_model.GetScope())
            {
                if (clearSelection) _model.ControlRow.SelectedNodes.Clear();
                if (node != null)
                {
                    switch (selectionMode)
                    {
                        case NodeSelectionMode.None:
                            break;
                        case NodeSelectionMode.SelectExclusive:
                            node.IsSelected = true;
                            break;
                        case NodeSelectionMode.Select:
                            node.IsSelected = true;
                            break;
                        case NodeSelectionMode.Toggle:
                            node.IsSelected = !node.IsSelected;
                            break;
                        case NodeSelectionMode.SelectRange:
                            var current = ActiveNode;
                            while (current != node)
                            {
                                current.IsSelected = true;
                                current = current.NodeRow.Top > node.NodeRow.Top ? current.NodeRow.FindPrevious().Node : current.NodeRow.FindNext().Node;
                            }
                            current.IsSelected = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
                    }
                }
            };
            var scrollInto = _activeNode != node;
            _activeNode = node;
            OnActiveNodeChanged();
            if (node == null) return false;
            if (scrollInto) ScrollIntoView(node);
            Invalidate();
            return true;
        }

        public void ScrollIntoView([NotNull] Node node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            var newY = -AutoScrollPosition.Y;
            var newX = -AutoScrollPosition.X;
            if (node.NodeRow.Top < VerticalScroll.Value) newY = node.NodeRow.Top;
            if (node.NodeRow.Bottom > VerticalScroll.Value + ClientSize.Height) newY = (VerticalScroll.Value = node.NodeRow.Bottom - ClientSize.Height);
            if (node.NodeRow.Left < HorizontalScroll.Value) newX = node.NodeRow.Left;
            if (node.NodeRow.Right > HorizontalScroll.Value + ClientSize.Width) newX = Math.Min(HorizontalScroll.Value = node.NodeRow.Right - ClientSize.Width, node.NodeRow.Left);
            AutoScrollPosition = new Point(newX, newY);
        }

        #endregion

        #region paint

        protected override void OnPaint(PaintEventArgs e)
        {
            var clip = new Rectangle(e.ClipRectangle.Location.Subtract(AutoScrollPosition), e.ClipRectangle.Size);
            var nodeRows = _model.NodesTable.Where(n => n.Visible && n.Bottom > clip.Top && n.Top < clip.Bottom).ToArray();

            e.Graphics.TranslateTransform(-HorizontalScroll.Value, -VerticalScroll.Value);

            if (DrawConnectors) DrawConnectorLines(e, nodeRows);
            DrawNodes(e, nodeRows);

            e.Graphics.ResetTransform();
            base.OnPaint(e);
        }

        private void DrawConnectorLines(PaintEventArgs e, NodeRow[] nodeRows)
        {
            var nodesDrawnVerticalConnectorLine = new HashSet<NodeRow>();
            var expandIndicatorCenterOffset = new Point(ExpandIndicatorOffsetX + ExpandIndicatorSize / 2, ExpandIndicatorSize / 2);
            foreach (var nodeRow in nodeRows)
            {
                // connectors horizontal
                if (nodeRow.ParentNodeRow != null)
                {
                    var indicatorMid = nodeRow.ExpandIndicatorMid(expandIndicatorCenterOffset);
                    e.Graphics.DrawLine(SystemPens.ControlDark, indicatorMid, new Point(indicatorMid.X - _model.ControlRow.NodeIndent, indicatorMid.Y));
                }
                // connectors vertical
                var parentNodeRow = nodeRow.ParentNodeRow;
                while (parentNodeRow != null && nodesDrawnVerticalConnectorLine.Add(parentNodeRow))
                {
                    var parentIndicatorMid = parentNodeRow.ExpandIndicatorMid(expandIndicatorCenterOffset);
                    var childExpandIndicatorMid = parentNodeRow.ChildNodeRows.First().LastSibling.ExpandIndicatorMid(expandIndicatorCenterOffset);
                    e.Graphics.DrawLine(SystemPens.ControlDark, parentIndicatorMid, new Point(parentIndicatorMid.X, childExpandIndicatorMid.Y));
                    parentNodeRow = parentNodeRow.ParentNodeRow;
                }
            }
        }

        private void DrawNodes(PaintEventArgs e, NodeRow[] nodeRows)
        {
            foreach (var nodeRow in nodeRows)
            {
                // background
                if (nodeRow.IsSelected)
                {
                    var backBrush = Focused ? SystemBrushes.Highlight : SystemBrushes.ControlDark;
                    e.Graphics.FillRectangle(backBrush, nodeRow.ToRectangle());
                }
                // focus
                if (nodeRow.Node == ActiveNode || (EnableDragDrop && nodeRow.Node == _hot))
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, nodeRow.ToFocusRectangle());
                }
                // expand indicator
                if (nodeRow.CanExpand)
                {
                    var expandImage = nodeRow.Expanded ? ExpandIndicatorExpanded : ExpandIndicatorCollapsed;
                    e.Graphics.DrawImage(expandImage, nodeRow.Left + ExpandIndicatorOffsetX, nodeRow.Top);
                }
                // text
                var textBrush = nodeRow.IsSelected ? SystemBrushes.HighlightText : SystemBrushes.ControlText;
                e.Graphics.DrawString(nodeRow.Text, nodeRow.Font, textBrush, nodeRow.ToRectangle(), nodeRow.ControlRow.StringFormat);
            }
        }

        #endregion

        #region mouse

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var coordinate = e.Location.Subtract(AutoScrollPosition);
                var nodeRow = GetNodeRowFromCoordinate(coordinate.Y);
                if (nodeRow != null)
                {
                    if (nodeRow.IsExpandIndicatoLocation(coordinate.X) && nodeRow.CanExpand)
                    {
                        // expand indicator clicked - toggle node.Expanded
                        nodeRow.Expanded = !nodeRow.Expanded;
                    }
                    else
                    {
                        // node clicked - select node
                        if (ModifierKeys.HasFlag(Keys.Control)) SetActiveNode(nodeRow.Node, NodeSelectionMode.Toggle);
                        else if (ModifierKeys.HasFlag(Keys.Shift)) SetActiveNode(nodeRow.Node, NodeSelectionMode.SelectRange);
                        else SetActiveNode(nodeRow.Node, ActiveNode?.Visible == true ? NodeSelectionMode.SelectExclusive : NodeSelectionMode.Select);
                    }
                }
            }
            base.OnMouseClick(e);
        }

        [CanBeNull]
        private NodeRow GetNodeRowFromCoordinate(int y)
        {
            var nodeRow = _model.ControlRow.AllNodeRows.FirstOrDefault(n => n.Visible && n.ContainsVertical(y));
            return nodeRow;
        }

        [CanBeNull]
        private NodeRow GetNodeRowFromCoordinate(Point cordinate)
        {
            var nodeRow = _model.ControlRow.AllNodeRows.FirstOrDefault(n => n.Visible && n.Contains(cordinate));
            return nodeRow;
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var coordinate = e.Location.Subtract(AutoScrollPosition);
                var node = GetNodeRowFromCoordinate(coordinate);
                if (node != null && node.CanExpand) node.Expanded = !node.Expanded;
            }
            base.OnMouseDoubleClick(e);
        }

        #endregion

        #region drag drop

        private Node _hot;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (EnableDragDrop && e.Button == MouseButtons.Left && SelectedNodes.Any())
            {
                DoDragDrop("X", DragDropEffects.Move);
            }


            base.OnMouseMove(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var point = PointToClient(new Point(drgevent.X, drgevent.Y));
            _hot = GetNodeAt(point.Y);
            drgevent.Effect = _hot != null ? DragDropEffects.Move : DragDropEffects.None;

            base.OnDragOver(drgevent);

            // scroll when dragging near border
            var scrollPosition = Point.Empty.Subtract(AutoScrollPosition);
            if (point.Y < _model.ControlRow.NodeTextMinSize.Height * 2)
                scrollPosition.Y -= point.Y < _model.ControlRow.NodeTextMinSize.Height ? VerticalScroll.LargeChange : VerticalScroll.SmallChange;
            else if (point.Y > Height - _model.ControlRow.NodeTextMinSize.Height * 2)
                scrollPosition.Y += point.Y > Height - _model.ControlRow.NodeTextMinSize.Height ? VerticalScroll.LargeChange : VerticalScroll.SmallChange;

            if (point.X < _model.ControlRow.NodeTextMinSize.Width * 2)
                scrollPosition.X -= point.X < _model.ControlRow.NodeTextMinSize.Width ? HorizontalScroll.LargeChange : HorizontalScroll.SmallChange;
            else if (point.X > Width - _model.ControlRow.NodeTextMinSize.Width * 2)
                scrollPosition.X += point.X > Width - _model.ControlRow.NodeTextMinSize.Width ? HorizontalScroll.LargeChange : HorizontalScroll.SmallChange;

            if (AutoScrollPosition.Add(scrollPosition) != Point.Empty) AutoScrollPosition = scrollPosition;

            Invalidate();
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            ctxDrop.Show(drgevent.X, drgevent.Y);
            base.OnDragDrop(drgevent);
        }

        private void ctxDrop_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason != ToolStripDropDownCloseReason.ItemClicked) _hot = null;
        }

        private void miPreviousSibling_Click(object sender, EventArgs e)
        {
            MoveSelectedNodes(false, NodeTargetMode.PreviousSibling);
        }

        private void miNextSibling_Click(object sender, EventArgs e)
        {
            MoveSelectedNodes(true, NodeTargetMode.NextSibling);
        }

        private void miFirstChild_Click(object sender, EventArgs e)
        {
            using (_model.GetScope())
            {
                if (_hot != null) _hot.Expanded = true;
                MoveSelectedNodes(true, NodeTargetMode.FirstChild);
            }
        }

        private void miLastChild_Click(object sender, EventArgs e)
        {
            using (_model.GetScope())
            {
                if (_hot != null) _hot.Expanded = true;
                MoveSelectedNodes(false, NodeTargetMode.LastChild);
            }
        }

        private void MoveSelectedNodes(bool reverse, NodeTargetMode mode)
        {
            if (_hot == null) return;
            MoveNodes(reverse ? SelectedNodes.Reverse() : SelectedNodes, _hot, mode, ShowError);
            _hot = null;
        }

        public void MoveNodes([NotNull] IEnumerable<Node> nodes, [NotNull] Node target, NodeTargetMode mode, Action<Exception> onError)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (!target.IsValid) throw new ArgumentException("specified target is invalid", nameof(target));
            if (target.NodeRow.ControlRow != _model.ControlRow) throw new ArgumentException("specified target is not in tree", nameof(target));
            using (_model.GetScope(onError))
            {
                foreach (var node in nodes)
                {
                    node.NodeRow.Unlink();
                    node.NodeRow.SetAs(mode, target.NodeRow);
                }
            };
        }

        private void ShowError(Exception ex)
        {
            if (ex is BcfCalculationException) MessageBox.Show("The operation makes a node its own parent or sibling", "Operation undone");
            else MessageBox.Show(ex.ToString(), ex.GetType().Name);
        }

        #endregion

        #region keybord

        private static readonly Keys[] InputKeys = new[] { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space, Keys.Up | Keys.Shift, Keys.Down | Keys.Shift, Keys.Left | Keys.Shift, Keys.Right | Keys.Shift };

        protected override bool IsInputKey(Keys keyData)
        {
            return InputKeys.Contains(keyData) || base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    e.Handled = GetUp();
                    break;
                case Keys.Down:
                    e.Handled = GetDown();
                    break;
                case Keys.Left:
                    e.Handled = CollapseActiveNode() || GetUp();
                    break;
                case Keys.Right:
                    e.Handled = ExpandActiveNode() || GetDown();
                    break;
                case Keys.Space:
                    e.Handled = ToggleIsSelected();
                    break;

            }
            base.OnKeyDown(e);
        }

        private bool ToggleIsSelected()
        {
            if (ActiveNode != null)
            {
                ActiveNode.IsSelected = !ActiveNode.IsSelected;
                return true;
            }
            return false;
        }

        private bool ExpandActiveNode()
        {
            var node = ActiveNode;
            if (node == null || node.Expanded || !node.CanExpand) return false;
            node.Expanded = true;
            return true;
        }

        private bool CollapseActiveNode()
        {
            var node = ActiveNode;
            if (node == null || !node.Expanded || !node.CanExpand) return false;
            node.Expanded = false;
            return true;
        }

        private bool GetDown()
        {
            var other = ActiveNode != null ? ActiveNode.NodeRow.FindNext()?.Node : _model.ControlRow.RootNodeRows.FirstOrDefault()?.FirstSibling.Node;
            return SetActiveNode(other, GetNodeSelectionModeFromModifierKeys());
        }

        private bool GetUp()
        {
            var other = ActiveNode != null ? ActiveNode.NodeRow.FindPrevious()?.Node : _model.ControlRow.RootNodeRows.FirstOrDefault()?.LastSibling.LastVisibleGrandChild.Node;
            return SetActiveNode(other, GetNodeSelectionModeFromModifierKeys());
        }

        private NodeSelectionMode GetNodeSelectionModeFromModifierKeys()
        {
            if (ModifierKeys.HasFlag(Keys.Control)) return NodeSelectionMode.None;
            if (ModifierKeys.HasFlag(Keys.Shift)) return NodeSelectionMode.Select;
            return NodeSelectionMode.SelectExclusive;
        }

        #endregion

        [Category("TreeView")]
        [DefaultValue(typeof(Size), "23, 16")]
        public Size NodeTextMinSize { get => _model.ControlRow.NodeTextMinSize; set => _model.ControlRow.NodeTextMinSize = value; }

        [Category("TreeView")]
        [DefaultValue(typeof(Size), "300, 150")]
        public Size NodeTextMaxSize { get => _model.ControlRow.NodeTextMaxSize; set => _model.ControlRow.NodeTextMaxSize = value; }

        [Category("TreeView")]
        [DefaultValue(16)]
        public int NodeIndent { get => _model.ControlRow.NodeIndent; set => _model.ControlRow.NodeIndent = value; }

        private bool _drawConnectors;

        [Category("TreeView")]
        [DefaultValue(false)]
        public bool DrawConnectors
        {
            get => _drawConnectors;
            set
            {
                _drawConnectors = value;
                Invalidate();
            }
        }

        [Category("TreeView")]
        [DefaultValue(true)]
        public bool EnableDragDrop
        {
            get => AllowDrop;
            set => AllowDrop = value;
        }

        [CanBeNull]
        public Node GetNodeAt(int y)
        {
            y = y - AutoScrollPosition.Y;
            bool IsNodeAt(NodeRow n) => n.Top <= y && n.ClientAreaBottom >= y;
            var nodeRow = _model.ControlRow.RootNodeRows.FirstOrDefault(IsNodeAt);
            while (nodeRow != null)
            {
                if (nodeRow.Top <= y && nodeRow.Bottom >= y) return nodeRow.Node;
                nodeRow = nodeRow.ChildNodeRows.FirstOrDefault(IsNodeAt);
            }
            return null;
        }

        public void WithTransactionScope([NotNull] [InstantHandle]Action<TreeView> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            using (_model.GetScope()) action(this);
        }

        private int _lastPropertiesChanged;

        [Category("TreeView")]
        [Description("Nuber of properties changed within last transaction")]
        public int LastPropertiesChanged => _lastPropertiesChanged;

        private int _totalPropertiesChanged;

        [Category("TreeView")]
        [Description("Nuber of properties changed since control created")]
        public int TotalPropertiesChanged => _totalPropertiesChanged;
    }
}
