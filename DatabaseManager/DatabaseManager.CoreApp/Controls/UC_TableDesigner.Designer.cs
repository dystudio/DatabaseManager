﻿namespace DatabaseManager.Controls
{
    partial class UC_TableDesigner
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTableComment = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabColumns = new System.Windows.Forms.TabPage();
            this.ucColumns = new DatabaseManager.Controls.UC_TableColumns();
            this.tabForeignKeys = new System.Windows.Forms.TabPage();
            this.ucForeignKeys = new DatabaseManager.Controls.UC_TableForeignKeys();
            this.tabIndexes = new System.Windows.Forms.TabPage();
            this.ucIndexes = new DatabaseManager.Controls.UC_TableIndexes();
            this.tabConstraints = new System.Windows.Forms.TabPage();
            this.ucConstraints = new DatabaseManager.Controls.UC_TableConstraints();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cboOwner = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabColumns.SuspendLayout();
            this.tabForeignKeys.SuspendLayout();
            this.tabIndexes.SuspendLayout();
            this.tabConstraints.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Table Name:";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(94, 6);
            this.txtTableName.Margin = new System.Windows.Forms.Padding(4);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(417, 23);
            this.txtTableName.TabIndex = 2;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(601, 10);
            this.lblOwner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(49, 17);
            this.lblOwner.TabIndex = 3;
            this.lblOwner.Text = "Owner:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Comment:";
            // 
            // txtTableComment
            // 
            this.txtTableComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableComment.Location = new System.Drawing.Point(94, 45);
            this.txtTableComment.Margin = new System.Windows.Forms.Padding(4);
            this.txtTableComment.Name = "txtTableComment";
            this.txtTableComment.Size = new System.Drawing.Size(714, 23);
            this.txtTableComment.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabColumns);
            this.tabControl1.Controls.Add(this.tabForeignKeys);
            this.tabControl1.Controls.Add(this.tabIndexes);
            this.tabControl1.Controls.Add(this.tabConstraints);
            this.tabControl1.Location = new System.Drawing.Point(7, 76);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 540);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabColumns
            // 
            this.tabColumns.Controls.Add(this.ucColumns);
            this.tabColumns.Location = new System.Drawing.Point(4, 26);
            this.tabColumns.Margin = new System.Windows.Forms.Padding(4);
            this.tabColumns.Name = "tabColumns";
            this.tabColumns.Padding = new System.Windows.Forms.Padding(4);
            this.tabColumns.Size = new System.Drawing.Size(803, 510);
            this.tabColumns.TabIndex = 0;
            this.tabColumns.Text = "Columns";
            this.tabColumns.UseVisualStyleBackColor = true;
            // 
            // ucColumns
            // 
            this.ucColumns.BackColor = System.Drawing.Color.White;
            this.ucColumns.DatabaseType = DatabaseInterpreter.Model.DatabaseType.Unknown;
            this.ucColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucColumns.Location = new System.Drawing.Point(4, 4);
            this.ucColumns.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ucColumns.Name = "ucColumns";
            this.ucColumns.Size = new System.Drawing.Size(795, 502);
            this.ucColumns.TabIndex = 0;
            this.ucColumns.UserDefinedTypes = null;
            // 
            // tabForeignKeys
            // 
            this.tabForeignKeys.Controls.Add(this.ucForeignKeys);
            this.tabForeignKeys.Location = new System.Drawing.Point(4, 26);
            this.tabForeignKeys.Margin = new System.Windows.Forms.Padding(4);
            this.tabForeignKeys.Name = "tabForeignKeys";
            this.tabForeignKeys.Padding = new System.Windows.Forms.Padding(4);
            this.tabForeignKeys.Size = new System.Drawing.Size(803, 510);
            this.tabForeignKeys.TabIndex = 1;
            this.tabForeignKeys.Text = "Foreign Keys";
            this.tabForeignKeys.UseVisualStyleBackColor = true;
            // 
            // ucForeignKeys
            // 
            this.ucForeignKeys.DatabaseType = DatabaseInterpreter.Model.DatabaseType.Unknown;
            this.ucForeignKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucForeignKeys.Location = new System.Drawing.Point(4, 4);
            this.ucForeignKeys.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ucForeignKeys.Name = "ucForeignKeys";
            this.ucForeignKeys.Size = new System.Drawing.Size(795, 502);
            this.ucForeignKeys.TabIndex = 0;
            this.ucForeignKeys.Table = null;
            // 
            // tabIndexes
            // 
            this.tabIndexes.Controls.Add(this.ucIndexes);
            this.tabIndexes.Location = new System.Drawing.Point(4, 26);
            this.tabIndexes.Margin = new System.Windows.Forms.Padding(4);
            this.tabIndexes.Name = "tabIndexes";
            this.tabIndexes.Size = new System.Drawing.Size(803, 510);
            this.tabIndexes.TabIndex = 2;
            this.tabIndexes.Text = "Indexes";
            this.tabIndexes.UseVisualStyleBackColor = true;
            // 
            // ucIndexes
            // 
            this.ucIndexes.DatabaseType = DatabaseInterpreter.Model.DatabaseType.Unknown;
            this.ucIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucIndexes.Location = new System.Drawing.Point(0, 0);
            this.ucIndexes.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ucIndexes.Name = "ucIndexes";
            this.ucIndexes.Size = new System.Drawing.Size(803, 510);
            this.ucIndexes.TabIndex = 0;
            this.ucIndexes.Table = null;
            // 
            // tabConstraints
            // 
            this.tabConstraints.Controls.Add(this.ucConstraints);
            this.tabConstraints.Location = new System.Drawing.Point(4, 26);
            this.tabConstraints.Margin = new System.Windows.Forms.Padding(4);
            this.tabConstraints.Name = "tabConstraints";
            this.tabConstraints.Size = new System.Drawing.Size(803, 510);
            this.tabConstraints.TabIndex = 3;
            this.tabConstraints.Text = "Constraints";
            this.tabConstraints.UseVisualStyleBackColor = true;
            // 
            // ucConstraints
            // 
            this.ucConstraints.DatabaseType = DatabaseInterpreter.Model.DatabaseType.Unknown;
            this.ucConstraints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucConstraints.Location = new System.Drawing.Point(0, 0);
            this.ucConstraints.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ucConstraints.Name = "ucConstraints";
            this.ucConstraints.Size = new System.Drawing.Size(803, 510);
            this.ucConstraints.TabIndex = 0;
            this.ucConstraints.Table = null;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "DataType";
            this.dataGridViewComboBoxColumn1.HeaderText = "Data Type";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.Width = 120;
            // 
            // cboOwner
            // 
            this.cboOwner.FormattingEnabled = true;
            this.cboOwner.Location = new System.Drawing.Point(656, 6);
            this.cboOwner.Margin = new System.Windows.Forms.Padding(4);
            this.cboOwner.Name = "cboOwner";
            this.cboOwner.Size = new System.Drawing.Size(153, 25);
            this.cboOwner.TabIndex = 9;
            // 
            // UC_TableDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cboOwner);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtTableComment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblOwner);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UC_TableDesigner";
            this.Size = new System.Drawing.Size(821, 620);
            this.Load += new System.EventHandler(this.UC_TableDesigner_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabColumns.ResumeLayout(false);
            this.tabForeignKeys.ResumeLayout(false);
            this.tabIndexes.ResumeLayout(false);
            this.tabConstraints.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTableComment;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabForeignKeys;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.TabPage tabColumns;
        private UC_TableColumns ucColumns;
        private System.Windows.Forms.ComboBox cboOwner;
        private System.Windows.Forms.TabPage tabIndexes;
        private System.Windows.Forms.TabPage tabConstraints;
        private UC_TableIndexes ucIndexes;
        private UC_TableForeignKeys ucForeignKeys;
        private UC_TableConstraints ucConstraints;
    }
}
