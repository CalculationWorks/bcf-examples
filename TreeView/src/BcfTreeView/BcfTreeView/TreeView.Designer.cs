namespace BcfTreeView {
    partial class TreeView {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.ctxDrop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPreviousSibling = new System.Windows.Forms.ToolStripMenuItem();
            this.miNextSibling = new System.Windows.Forms.ToolStripMenuItem();
            this.miFirstChild = new System.Windows.Forms.ToolStripMenuItem();
            this.miLastChild = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDrop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctxDrop
            // 
            this.ctxDrop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPreviousSibling,
            this.miNextSibling,
            this.miFirstChild,
            this.miLastChild});
            this.ctxDrop.Name = "ctxDrop";
            this.ctxDrop.Size = new System.Drawing.Size(159, 114);
            this.ctxDrop.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ctxDrop_Closed);
            // 
            // miPreviousSibling
            // 
            this.miPreviousSibling.Name = "miPreviousSibling";
            this.miPreviousSibling.Size = new System.Drawing.Size(158, 22);
            this.miPreviousSibling.Text = "Previous Sibling";
            this.miPreviousSibling.Click += new System.EventHandler(this.miPreviousSibling_Click);
            // 
            // miNextSibling
            // 
            this.miNextSibling.Name = "miNextSibling";
            this.miNextSibling.Size = new System.Drawing.Size(158, 22);
            this.miNextSibling.Text = "Next Sibling";
            this.miNextSibling.Click += new System.EventHandler(this.miNextSibling_Click);
            // 
            // miFirstChild
            // 
            this.miFirstChild.Name = "miFirstChild";
            this.miFirstChild.Size = new System.Drawing.Size(158, 22);
            this.miFirstChild.Text = "First Child";
            this.miFirstChild.Click += new System.EventHandler(this.miFirstChild_Click);
            // 
            // miLastChild
            // 
            this.miLastChild.Name = "miLastChild";
            this.miLastChild.Size = new System.Drawing.Size(158, 22);
            this.miLastChild.Text = "Last Child";
            this.miLastChild.Click += new System.EventHandler(this.miLastChild_Click);
            // 
            // TreeView
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TreeView";
            this.ctxDrop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ctxDrop;
        private System.Windows.Forms.ToolStripMenuItem miPreviousSibling;
        private System.Windows.Forms.ToolStripMenuItem miNextSibling;
        private System.Windows.Forms.ToolStripMenuItem miFirstChild;
        private System.Windows.Forms.ToolStripMenuItem miLastChild;
    }
}
