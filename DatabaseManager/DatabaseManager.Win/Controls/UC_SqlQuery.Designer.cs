﻿namespace DatabaseManager.Controls
{
    partial class UC_SqlQuery
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabResult = new System.Windows.Forms.TabControl();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.tabPageMessage = new System.Windows.Forms.TabPage();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.queryEditor = new DatabaseManager.Controls.UC_QueryEditor();
            this.resultGridView = new DatabaseManager.Controls.UC_QueryResultGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.tabPageData.SuspendLayout();
            this.tabPageMessage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.queryEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabResult);
            this.splitContainer1.Size = new System.Drawing.Size(432, 407);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.tabPageData);
            this.tabResult.Controls.Add(this.tabPageMessage);
            this.tabResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResult.Location = new System.Drawing.Point(0, 0);
            this.tabResult.Name = "tabResult";
            this.tabResult.SelectedIndex = 0;
            this.tabResult.Size = new System.Drawing.Size(432, 155);
            this.tabResult.TabIndex = 0;
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.resultGridView);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Size = new System.Drawing.Size(424, 129);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // tabPageMessage
            // 
            this.tabPageMessage.Controls.Add(this.resultTextBox);
            this.tabPageMessage.Location = new System.Drawing.Point(4, 22);
            this.tabPageMessage.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMessage.Name = "tabPageMessage";
            this.tabPageMessage.Size = new System.Drawing.Size(424, 129);
            this.tabPageMessage.TabIndex = 0;
            this.tabPageMessage.Text = "Message";
            this.tabPageMessage.UseVisualStyleBackColor = true;
            // 
            // resultTextBox
            // 
            this.resultTextBox.BackColor = System.Drawing.Color.White;
            this.resultTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTextBox.Location = new System.Drawing.Point(0, 0);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(424, 129);
            this.resultTextBox.TabIndex = 0;
            this.resultTextBox.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 409);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(432, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslMessage
            // 
            this.tsslMessage.AutoSize = false;
            this.tsslMessage.BackColor = System.Drawing.Color.Transparent;
            this.tsslMessage.Name = "tsslMessage";
            this.tsslMessage.Size = new System.Drawing.Size(250, 17);
            this.tsslMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queryEditor
            // 
            this.queryEditor.DatabaseType = DatabaseInterpreter.Model.DatabaseType.Unknown;
            this.queryEditor.DbInterpreter = null;
            this.queryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryEditor.Location = new System.Drawing.Point(0, 0);
            this.queryEditor.Name = "queryEditor";
            this.queryEditor.Size = new System.Drawing.Size(432, 250);
            this.queryEditor.TabIndex = 0;
            this.queryEditor.Load += new System.EventHandler(this.queryEditor_Load);
            // 
            // resultGridView
            // 
            this.resultGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultGridView.Location = new System.Drawing.Point(0, 0);
            this.resultGridView.Margin = new System.Windows.Forms.Padding(0);
            this.resultGridView.Name = "resultGridView";
            this.resultGridView.Size = new System.Drawing.Size(424, 129);
            this.resultGridView.TabIndex = 0;
            // 
            // UC_SqlQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UC_SqlQuery";
            this.Size = new System.Drawing.Size(432, 431);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabResult.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.tabPageMessage.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslMessage;
        private System.Windows.Forms.TabControl tabResult;
        private System.Windows.Forms.TabPage tabPageMessage;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private UC_QueryResultGrid resultGridView;
        private UC_QueryEditor queryEditor;
    }
}
