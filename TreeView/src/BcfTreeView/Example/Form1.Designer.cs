namespace Example {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new BcfTreeView.TreeView();
            this.chbCase = new System.Windows.Forms.CheckBox();
            this.pgControl = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pgSelectedNodes = new System.Windows.Forms.PropertyGrid();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.numLevels = new System.Windows.Forms.NumericUpDown();
            this.btnExpandLevels = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLevels)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(350, 520);
            this.panel1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(2, 2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(344, 514);
            this.treeView1.TabIndex = 0;
            this.treeView1.SelectedNodesChanged += new System.EventHandler(this.treeView1_SelectedNodesChanged);
            this.treeView1.GetChildNodeObjects += new System.EventHandler<BcfTreeView.GetChildNodeObjectsEventArgs>(this.treeView1_GetChildNodeObjects);
            // 
            // chbCase
            // 
            this.chbCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbCase.AutoSize = true;
            this.chbCase.Checked = true;
            this.chbCase.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chbCase.Location = new System.Drawing.Point(368, 12);
            this.chbCase.Name = "chbCase";
            this.chbCase.Size = new System.Drawing.Size(177, 17);
            this.chbCase.TabIndex = 0;
            this.chbCase.Text = "Upper/Lower/Unchanged Case";
            this.chbCase.ThreeState = true;
            this.chbCase.UseVisualStyleBackColor = true;
            this.chbCase.CheckStateChanged += new System.EventHandler(this.chbCase_CheckStateChanged);
            // 
            // pgControl
            // 
            this.pgControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgControl.Location = new System.Drawing.Point(3, 3);
            this.pgControl.Name = "pgControl";
            this.pgControl.Size = new System.Drawing.Size(339, 392);
            this.pgControl.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(368, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(353, 424);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pgControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(345, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Control";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pgSelectedNodes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(345, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Selected Nodes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pgSelectedNodes
            // 
            this.pgSelectedNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSelectedNodes.Location = new System.Drawing.Point(3, 3);
            this.pgSelectedNodes.Name = "pgSelectedNodes";
            this.pgSelectedNodes.Size = new System.Drawing.Size(339, 392);
            this.pgSelectedNodes.TabIndex = 4;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(368, 35);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(177, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete selected node(s)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(368, 85);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(163, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh Propertygrids";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // numLevels
            // 
            this.numLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numLevels.Location = new System.Drawing.Point(368, 62);
            this.numLevels.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numLevels.Name = "numLevels";
            this.numLevels.Size = new System.Drawing.Size(73, 20);
            this.numLevels.TabIndex = 3;
            this.numLevels.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnExpandLevels
            // 
            this.btnExpandLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandLevels.Location = new System.Drawing.Point(447, 59);
            this.btnExpandLevels.Name = "btnExpandLevels";
            this.btnExpandLevels.Size = new System.Drawing.Size(163, 23);
            this.btnExpandLevels.TabIndex = 4;
            this.btnExpandLevels.Text = "Expand n- levels";
            this.btnExpandLevels.UseVisualStyleBackColor = true;
            this.btnExpandLevels.Click += new System.EventHandler(this.btnExpandLevels_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(551, 35);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(177, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 544);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnExpandLevels);
            this.Controls.Add(this.numLevels);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chbCase);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLevels)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BcfTreeView.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chbCase;
        private System.Windows.Forms.PropertyGrid pgControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid pgSelectedNodes;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.NumericUpDown numLevels;
        private System.Windows.Forms.Button btnExpandLevels;
        private System.Windows.Forms.Button btnReset;
    }
}

