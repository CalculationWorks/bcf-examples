﻿//---------------------------------------------------------------------------------
// <auto-generated>
//    Code was generated by CalculationWorks BCF Editor 
//    http://www.calculationworks.com/
//
//    Manual changes to this file will be overwritten when the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------
using BcfTreeView.Bcf;
using CalculationWorks.BusinessModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CalculationWorks.BusinessModel.Design;
namespace BcfTreeView.Bcf {
    partial class TreeModelDataSet : BcfDataSet {
        public TreeModelDataSet()
            : this(new BcfTreeView.Bcf.TreeModelDataSetSetup()) { }

        internal  TreeModelDataSet(BcfTreeView.Bcf.TreeModelDataSetSetup dataSetSetup)
            : base(dataSetSetup, GetTableFactory()) {
            InitializeSchemaProperties();
            Initialized();
        }
        partial void Initialized();
        private void InitializeSchemaProperties() {
            ControlsTable = (ControlsTable)Tables["Controls"];
            NodesTable = (NodesTable)Tables["Nodes"];
            ControlToAllNodesRelation = (BcfRelation<TreeModelRelationOptions>)Relations["ControlToAllNodes"];
            NodeToNextSiblingsRelation = (BcfRelation<TreeModelRelationOptions>)Relations["NodeToNextSiblings"];
            NodeToChildNodesRelation = (BcfRelation<TreeModelRelationOptions>)Relations["NodeToChildNodes"];
            ControlToSelectedNodesRelation = (BcfRelation<TreeModelRelationOptions>)Relations["ControlToSelectedNodes"];
            ControlToRootNodesRelation = (BcfRelation<TreeModelRelationOptions>)Relations["ControlToRootNodes"];
            ControlsTable.InitializeSchemaProperties();
            NodesTable.InitializeSchemaProperties();
        }
        public ControlsTable ControlsTable { get; private set; }
        public NodesTable NodesTable { get; private set; }
        public BcfRelation<TreeModelRelationOptions> ControlToAllNodesRelation { get; private set; }
        public BcfRelation<TreeModelRelationOptions> NodeToNextSiblingsRelation { get; private set; }
        public BcfRelation<TreeModelRelationOptions> NodeToChildNodesRelation { get; private set; }
        public BcfRelation<TreeModelRelationOptions> ControlToSelectedNodesRelation { get; private set; }
        public BcfRelation<TreeModelRelationOptions> ControlToRootNodesRelation { get; private set; }
        private static BcfTableFactory GetTableFactory() {
            BcfTableFactory factory = new BcfTableFactory();
            factory.Register("Controls", (builder) => new ControlsTable(builder));
            factory.Register("Nodes", (builder) => new NodesTable(builder));
            return factory;
        }
    }
    partial class ControlsTable : BcfTable<ControlRow, TreeModelTableOptions> {
        public new TreeModelDataSet DataSet { get { return (TreeModelDataSet)base.DataSet; } }
        public BcfColumn<int, TreeModelColumnOptions> ColumnNodeIndent { get; private set; }
        public BcfColumn<Size, TreeModelColumnOptions> ColumnNodeTextMinSize { get; private set; }
        public BcfColumn<Size, TreeModelColumnOptions> ColumnNodeTextMaxSize { get; private set; }
        public BcfColumn<Font, TreeModelColumnOptions> ColumnFont { get; private set; }
        public BcfColumn<StringFormat, TreeModelColumnOptions> ColumnStringFormat { get; private set; }
        public BcfColumn<Graphics, TreeModelColumnOptions> ColumnStringMeasureGraphics { get; private set; }
        public BcfColumn<Size, TreeModelColumnOptions> ColumnAutoScrollMinSize { get; private set; }
        public BcfColumn<Func<object, string>, TreeModelColumnOptions> ColumnNodeObjectToString { get; private set; }
        public BcfColumn<TreeView, TreeModelColumnOptions> ColumnTreeView { get; private set; }
        internal void InitializeSchemaProperties() {
            ColumnNodeIndent = (BcfColumn<int, TreeModelColumnOptions>)Columns["NodeIndent"];
            ColumnNodeTextMinSize = (BcfColumn<Size, TreeModelColumnOptions>)Columns["NodeTextMinSize"];
            ColumnNodeTextMaxSize = (BcfColumn<Size, TreeModelColumnOptions>)Columns["NodeTextMaxSize"];
            ColumnFont = (BcfColumn<Font, TreeModelColumnOptions>)Columns["Font"];
            ColumnStringFormat = (BcfColumn<StringFormat, TreeModelColumnOptions>)Columns["StringFormat"];
            ColumnStringMeasureGraphics = (BcfColumn<Graphics, TreeModelColumnOptions>)Columns["StringMeasureGraphics"];
            ColumnAutoScrollMinSize = (BcfColumn<Size, TreeModelColumnOptions>)Columns["AutoScrollMinSize"];
            ColumnNodeObjectToString = (BcfColumn<Func<object, string>, TreeModelColumnOptions>)Columns["NodeObjectToString"];
            ColumnTreeView = (BcfColumn<TreeView, TreeModelColumnOptions>)Columns["TreeView"];
        }
        protected internal ControlsTable(BcfTableBuilder builder) : base(builder) { }
        protected override ControlRow CreateRow(BcfRowBuilder builder) {
            return new ControlRow(builder);
        }
    }
    partial class ControlRow : BcfRow {
        protected internal ControlRow([JetBrains.Annotations.NotNull] BcfRowBuilder builder) : base(builder) { }
        [JetBrains.Annotations.NotNull] public new ControlsTable Table {
            get { return (ControlsTable)base.Table; }
        }
        public int NodeIndent {
            get { return GetValue(Table.ColumnNodeIndent); }
            set { SetValue(Table.ColumnNodeIndent, value); }
        }
        public Size NodeTextMinSize {
            get { return GetValue(Table.ColumnNodeTextMinSize); }
            set { SetValue(Table.ColumnNodeTextMinSize, value); }
        }
        public Size NodeTextMaxSize {
            get { return GetValue(Table.ColumnNodeTextMaxSize); }
            set { SetValue(Table.ColumnNodeTextMaxSize, value); }
        }
        [JetBrains.Annotations.CanBeNull] public Font Font {
            get { return GetValue(Table.ColumnFont); }
            set { SetValue(Table.ColumnFont, value); }
        }
        [JetBrains.Annotations.NotNull] public StringFormat StringFormat {
            get { return GetValue(Table.ColumnStringFormat); }
            set { SetValue(Table.ColumnStringFormat, value); }
        }
        [JetBrains.Annotations.CanBeNull] public Graphics StringMeasureGraphics {
            get { return GetValue(Table.ColumnStringMeasureGraphics); }
            set { SetValue(Table.ColumnStringMeasureGraphics, value); }
        }
        public Size AutoScrollMinSize {
            get { return GetValue(Table.ColumnAutoScrollMinSize); }
        }
        [JetBrains.Annotations.NotNull] public Func<object, string> NodeObjectToString {
            get { return GetValue(Table.ColumnNodeObjectToString); }
            set { SetValue(Table.ColumnNodeObjectToString, value); }
        }
        [JetBrains.Annotations.CanBeNull] public TreeView TreeView {
            get { return GetValue(Table.ColumnTreeView); }
            set { SetValue(Table.ColumnTreeView, value); }
        }
        [JetBrains.Annotations.NotNull] public BcfChildRelationCell<NodeRow> AllNodeRows {
            get { return (BcfChildRelationCell<NodeRow>)GetChildRows(Table.DataSet.ControlToAllNodesRelation); }
        }
        [JetBrains.Annotations.NotNull] public BcfChildRelationCell<NodeRow> SelectedNodes {
            get { return (BcfChildRelationCell<NodeRow>)GetChildRows(Table.DataSet.ControlToSelectedNodesRelation); }
        }
        [JetBrains.Annotations.NotNull] public BcfChildRelationCell<NodeRow> RootNodeRows {
            get { return (BcfChildRelationCell<NodeRow>)GetChildRows(Table.DataSet.ControlToRootNodesRelation); }
        }
    }
    partial class NodesTable : BcfTable<NodeRow, TreeModelTableOptions> {
        public new TreeModelDataSet DataSet { get { return (TreeModelDataSet)base.DataSet; } }
        public BcfColumn<int, TreeModelColumnOptions> ColumnLevel { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnSiblingIndex { get; private set; }
        public BcfColumn<bool, TreeModelColumnOptions> ColumnVisible { get; private set; }
        public BcfColumn<bool, TreeModelColumnOptions> ColumnCanExpand { get; private set; }
        public BcfColumn<bool, TreeModelColumnOptions> ColumnExpanded { get; private set; }
        public BcfColumn<bool, TreeModelColumnOptions> ColumnChildNodesInitialized { get; private set; }
        public BcfColumn<bool, TreeModelColumnOptions> ColumnHasNonMaterializedChildren { get; private set; }
        public BcfColumn<object, TreeModelColumnOptions> ColumnNodeObject { get; private set; }
        public BcfColumn<string, TreeModelColumnOptions> ColumnText { get; private set; }
        public BcfColumn<Size, TreeModelColumnOptions> ColumnSize { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnTop { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnLeft { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnBottom { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnRight { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnClientAreaBottom { get; private set; }
        public BcfColumn<int, TreeModelColumnOptions> ColumnClientAreaRight { get; private set; }
        public BcfColumn<Font, TreeModelColumnOptions> ColumnFont { get; private set; }
        public BcfColumn<Font, TreeModelColumnOptions> ColumnFontSet { get; private set; }
        internal void InitializeSchemaProperties() {
            ColumnLevel = (BcfColumn<int, TreeModelColumnOptions>)Columns["Level"];
            ColumnSiblingIndex = (BcfColumn<int, TreeModelColumnOptions>)Columns["SiblingIndex"];
            ColumnVisible = (BcfColumn<bool, TreeModelColumnOptions>)Columns["Visible"];
            ColumnCanExpand = (BcfColumn<bool, TreeModelColumnOptions>)Columns["CanExpand"];
            ColumnExpanded = (BcfColumn<bool, TreeModelColumnOptions>)Columns["Expanded"];
            ColumnChildNodesInitialized = (BcfColumn<bool, TreeModelColumnOptions>)Columns["ChildNodesInitialized"];
            ColumnHasNonMaterializedChildren = (BcfColumn<bool, TreeModelColumnOptions>)Columns["HasNonMaterializedChildren"];
            ColumnNodeObject = (BcfColumn<object, TreeModelColumnOptions>)Columns["NodeObject"];
            ColumnText = (BcfColumn<string, TreeModelColumnOptions>)Columns["Text"];
            ColumnSize = (BcfColumn<Size, TreeModelColumnOptions>)Columns["Size"];
            ColumnTop = (BcfColumn<int, TreeModelColumnOptions>)Columns["Top"];
            ColumnLeft = (BcfColumn<int, TreeModelColumnOptions>)Columns["Left"];
            ColumnBottom = (BcfColumn<int, TreeModelColumnOptions>)Columns["Bottom"];
            ColumnRight = (BcfColumn<int, TreeModelColumnOptions>)Columns["Right"];
            ColumnClientAreaBottom = (BcfColumn<int, TreeModelColumnOptions>)Columns["ClientAreaBottom"];
            ColumnClientAreaRight = (BcfColumn<int, TreeModelColumnOptions>)Columns["ClientAreaRight"];
            ColumnFont = (BcfColumn<Font, TreeModelColumnOptions>)Columns["Font"];
            ColumnFontSet = (BcfColumn<Font, TreeModelColumnOptions>)Columns["FontSet"];
        }
        protected internal NodesTable(BcfTableBuilder builder) : base(builder) { }
        protected override NodeRow CreateRow(BcfRowBuilder builder) {
            return new NodeRow(builder);
        }
    }
    partial class NodeRow : BcfRow {
        protected internal NodeRow([JetBrains.Annotations.NotNull] BcfRowBuilder builder) : base(builder) { }
        [JetBrains.Annotations.NotNull] public new NodesTable Table {
            get { return (NodesTable)base.Table; }
        }
        public int Level {
            get { return GetValue(Table.ColumnLevel); }
        }
        public int SiblingIndex {
            get { return GetValue(Table.ColumnSiblingIndex); }
        }
        public bool Visible {
            get { return GetValue(Table.ColumnVisible); }
        }
        public bool CanExpand {
            get { return GetValue(Table.ColumnCanExpand); }
        }
        public bool Expanded {
            get { return GetValue(Table.ColumnExpanded); }
            set { SetValue(Table.ColumnExpanded, value); }
        }
        public bool ChildNodesInitialized {
            get { return GetValue(Table.ColumnChildNodesInitialized); }
            set { SetValue(Table.ColumnChildNodesInitialized, value); }
        }
        public bool HasNonMaterializedChildren {
            get { return GetValue(Table.ColumnHasNonMaterializedChildren); }
            set { SetValue(Table.ColumnHasNonMaterializedChildren, value); }
        }
        [JetBrains.Annotations.CanBeNull] public object NodeObject {
            get { return GetValue(Table.ColumnNodeObject); }
            set { SetValue(Table.ColumnNodeObject, value); }
        }
        [JetBrains.Annotations.NotNull] public string Text {
            get { return GetValue(Table.ColumnText); }
        }
        public Size Size {
            get { return GetValue(Table.ColumnSize); }
        }
        public int Top {
            get { return GetValue(Table.ColumnTop); }
        }
        public int Left {
            get { return GetValue(Table.ColumnLeft); }
        }
        public int Bottom {
            get { return GetValue(Table.ColumnBottom); }
        }
        public int Right {
            get { return GetValue(Table.ColumnRight); }
        }
        public int ClientAreaBottom {
            get { return GetValue(Table.ColumnClientAreaBottom); }
        }
        public int ClientAreaRight {
            get { return GetValue(Table.ColumnClientAreaRight); }
        }
        [JetBrains.Annotations.CanBeNull] public Font Font {
            get { return GetValue(Table.ColumnFont); }
            set { SetValue(Table.ColumnFont, value); }
        }
        [JetBrains.Annotations.CanBeNull] public Font FontSet {
            get { return GetValue(Table.ColumnFontSet); }
            set { SetValue(Table.ColumnFontSet, value); }
        }
        [JetBrains.Annotations.NotNull] public BcfChildRelationCell<NodeRow> NextSiblingRows {
            get { return (BcfChildRelationCell<NodeRow>)GetChildRows(Table.DataSet.NodeToNextSiblingsRelation); }
        }
        [JetBrains.Annotations.NotNull] public BcfChildRelationCell<NodeRow> ChildNodeRows {
            get { return (BcfChildRelationCell<NodeRow>)GetChildRows(Table.DataSet.NodeToChildNodesRelation); }
        }
        [JetBrains.Annotations.CanBeNull] public ControlRow ControlRow {
            get { return (ControlRow)GetParentRow(Table.DataSet.ControlToAllNodesRelation); }
            set { SetParentRow(Table.DataSet.ControlToAllNodesRelation, value); }
        }
        [JetBrains.Annotations.CanBeNull] public NodeRow PreviousSiblingRow {
            get { return (NodeRow)GetParentRow(Table.DataSet.NodeToNextSiblingsRelation); }
            set { SetParentRow(Table.DataSet.NodeToNextSiblingsRelation, value); }
        }
        [JetBrains.Annotations.CanBeNull] public NodeRow ParentNodeRow {
            get { return (NodeRow)GetParentRow(Table.DataSet.NodeToChildNodesRelation); }
            set { SetParentRow(Table.DataSet.NodeToChildNodesRelation, value); }
        }
        [JetBrains.Annotations.CanBeNull] public ControlRow SelectedControlRow {
            get { return (ControlRow)GetParentRow(Table.DataSet.ControlToSelectedNodesRelation); }
            set { SetParentRow(Table.DataSet.ControlToSelectedNodesRelation, value); }
        }
        [JetBrains.Annotations.CanBeNull] public ControlRow RootControlRow {
            get { return (ControlRow)GetParentRow(Table.DataSet.ControlToRootNodesRelation); }
            set { SetParentRow(Table.DataSet.ControlToRootNodesRelation, value); }
        }
    }
}
namespace BcfTreeView.Bcf {
    public partial class TreeModelColumnOptions {
        public TreeModelColumnOptions(
        ) { }
    }
}
namespace BcfTreeView.Bcf {
    public partial class TreeModelTableOptions {
        public TreeModelTableOptions(
        ) { }
    }
}
namespace BcfTreeView.Bcf {
    public partial class TreeModelRuleOptions {
        public TreeModelRuleOptions(
        ) { }
    }
}
namespace BcfTreeView.Bcf {
    public partial class TreeModelRelationOptions {
        public TreeModelRelationOptions(
        ) { }
    }
}
