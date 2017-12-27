using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculationWorks.BusinessModel;
using JetBrains.Annotations;

namespace BcfTreeView.Bcf
{
    internal static class Util
    {

        public static Size NodeSize(StringFormat sf, Font f, Graphics g, string text, Size minSize, Size maxSize)
        {
            var size = Size.Ceiling(g.MeasureString(text, f, maxSize, sf));
            size += new Size(2, 2); // focus
            return new Size(Math.Max(size.Width, minSize.Width) + 2, Math.Max(size.Height, minSize.Height));
        }

        public static readonly BcfTrigger<bool> NodeExpandedTrigger = new BcfTrigger<bool>((cell, value, update) =>
        {
            var nodeRow = (NodeRow)cell.OwningRow;
            if (!nodeRow.Expanded && !nodeRow.ChildNodesInitialized && nodeRow.HasNonMaterializedChildren)
            {
                nodeRow.ChildNodesInitialized = true;
                var childObjects = nodeRow.ControlRow.TreeView.GetChildNodeObjectsInternal(nodeRow.Node);
                nodeRow.Table.DataSet.CurrentTransaction.SetEnforceConstraints(false);
                foreach (var childObject in childObjects)
                {
                    var childRow = nodeRow.Table.AddNewRow();
                    childRow.NodeObject = childObject;
                    childRow.HasNonMaterializedChildren = nodeRow.ControlRow.TreeView.GetChildNodeObjectsInternal(childRow.Node).Any();
                    childRow.SetAs(NodeTargetMode.LastChild, nodeRow);
                }
                nodeRow.HasNonMaterializedChildren = false;
            }
            update(); // necessary to write through - NodeExpanded is not computed
        });

        /// <summary>
        /// Gets a disposable transaction scope to use inside a using statement.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="onError"></param>
        /// <exception cref="ArgumentNullException"><paramref name="model"/> is <see langword="null" />.</exception>
        /// <returns></returns>
        [CanBeNull]
        public static Scope GetScope([NotNull] this BcfDataSet model, Action<Exception> onError = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (onError != null || model.CurrentTransaction?.EnforceConstraints != false) return new Scope(model.BeginTransaction(enforceConstraints: false), onError);
            return Scope.None;
        }

        /// <summary>
        /// Gets a disposable transaction scope to use inside a using statement.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        [CanBeNull]
        public static Scope GetScope([NotNull] this BcfRow row, Action<Exception> onError = null) => GetScope(row?.Table.DataSet, onError);
    }
}
