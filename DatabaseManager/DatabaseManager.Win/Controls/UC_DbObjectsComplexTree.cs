﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseInterpreter.Model;
using DatabaseInterpreter.Core;
using DatabaseManager.Core;
using DatabaseManager.Helper;
using DatabaseInterpreter.Utility;
using DatabaseManager.Model;

namespace DatabaseManager.Controls
{
    public delegate void ShowDbObjectContentHandler(DatabaseObjectDisplayInfo content);   
    
    public partial class UC_DbObjectsComplexTree : UserControl, IObserver<FeedbackInfo>
    {
        private DatabaseType databaseType;
        private ConnectionInfo connectionInfo;
        private DbInterpreterOption simpleInterpreterOption = new DbInterpreterOption() { ObjectFetchMode = DatabaseObjectFetchMode.Simple };

        public ShowDbObjectContentHandler OnShowContent;
        public FeedbackHandler OnFeedback;

        public UC_DbObjectsComplexTree()
        {
            InitializeComponent();            
            
            TreeView.CheckForIllegalCrossThreadCalls = false;
            Form.CheckForIllegalCrossThreadCalls = false;
        }

        public async Task LoadTree(DatabaseType dbType, ConnectionInfo connectionInfo)
        {
            this.databaseType = dbType;
            this.connectionInfo = connectionInfo;

            this.tvDbObjects.Nodes.Clear();

            DbInterpreter dbInterpreter = DbInterpreterHelper.GetDbInterpreter(dbType, connectionInfo, simpleInterpreterOption);

            List<Database> databases = await dbInterpreter.GetDatabasesAsync();

            foreach (Database database in databases)
            {
                TreeNode node = DbObjectsTreeHelper.CreateTreeNode(database, true);

                this.tvDbObjects.Nodes.Add(node);
            }
        }

        public void ClearNodes()
        {
            this.tvDbObjects.Nodes.Clear();
        }

        private void tvDbObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.tvDbObjects.SelectedNode = e.Node;

                this.SetMenuItemVisible(e.Node);

                this.contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private bool CanRefresh(TreeNode node)
        {
            return (node.Level <= 3) && !this.IsOnlyHasFakeChild(node);
        }

        private bool CanDelete(TreeNode node)
        {
            return node.Level == 2 || (node.Level == 4 && !(node.Tag is TableColumn));
        }

        private void SetMenuItemVisible(TreeNode node)
        {
            this.tsmiRefresh.Visible = this.CanRefresh(node);
            this.tsmiGenerateScripts.Visible = node.Level == 0 || node.Level == 2 || (node.Level == 4 && node.Tag is TableTrigger);
            this.tsmiConvert.Visible = node.Level == 0;
            this.tsmiClearData.Visible = node.Level == 0;
            this.tsmiEmptyDatabase.Visible = node.Level == 0;
            this.tsmiDelete.Visible = this.CanDelete(node);
            this.tsmiViewData.Visible = node.Tag is Table;
        }

        private ConnectionInfo GetConnectionInfo(string database)
        {
            ConnectionInfo info = ObjectHelper.CloneObject<ConnectionInfo>(this.connectionInfo);
            info.Database = database;
            return info;
        }

        private bool IsOnlyHasFakeChild(TreeNode node)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Name == DbObjectsTreeHelper.FakeNodeName)
            {
                return true;
            }
            return false;
        }

        private TreeNode GetDatabaseNode(TreeNode node)
        {
            while (!(node.Tag is Database))
            {
                return this.GetDatabaseNode(node.Parent);
            }
            return node;
        }

        private DbInterpreter GetDbInterpreter(string database, bool isSimpleMode = true)
        {
            ConnectionInfo connectionInfo = this.GetConnectionInfo(database);
            DbInterpreter dbInterpreter = DbInterpreterHelper.GetDbInterpreter(this.databaseType, connectionInfo, isSimpleMode ? simpleInterpreterOption : new DbInterpreterOption() { ObjectFetchMode = DatabaseObjectFetchMode.Details });
            return dbInterpreter;
        }

        private async Task AddDbObjectNodes(TreeNode parentNode, string database, DatabaseObjectType databaseObjectType = DatabaseObjectType.None, bool createFolderNode = true)
        {
            DbInterpreter dbInterpreter = this.GetDbInterpreter(database);

            SchemaInfo schemaInfo = await dbInterpreter.GetSchemaInfoAsync(new SchemaInfoFilter() { DatabaseObjectType = databaseObjectType });

            this.ClearNodes(parentNode);

            this.AddTreeNodes(parentNode, databaseObjectType, DatabaseObjectType.Table, schemaInfo.Tables, createFolderNode, true);
            this.AddTreeNodes(parentNode, databaseObjectType, DatabaseObjectType.View, schemaInfo.Views, createFolderNode);
            this.AddTreeNodes(parentNode, databaseObjectType, DatabaseObjectType.Function, schemaInfo.Functions, createFolderNode);
            this.AddTreeNodes(parentNode, databaseObjectType, DatabaseObjectType.Procedure, schemaInfo.Procedures, createFolderNode);

            parentNode.Expand();
        }

        private TreeNodeCollection AddTreeNodes<T>(TreeNode node, DatabaseObjectType types, DatabaseObjectType type, List<T> dbObjects, bool createFolderNode = true, bool createFakeNode = false)
            where T : DatabaseObject
        {
            TreeNode targetNode = node;

            if (types.HasFlag(type))
            {
                if (createFolderNode)
                {
                    targetNode = node.AddDbObjectFolderNode(dbObjects);
                }
                else
                {
                    targetNode = node.AddDbObjectNodes(dbObjects);
                }
            }

            if (createFakeNode && targetNode != null)
            {
                foreach (TreeNode child in targetNode.Nodes)
                {
                    child.Nodes.Add(DbObjectsTreeHelper.CreateFakeNode());
                }
            }

            return node.Nodes;
        }

        private void AddTableFakeNodes(TreeNode tableNode, Table table)
        {
            this.ClearNodes(tableNode);

            tableNode.Nodes.Add(DbObjectsTreeHelper.CreateFolderNode("Columns", "Columns", true));
            tableNode.Nodes.Add(DbObjectsTreeHelper.CreateFolderNode("Triggers", "Triggers", true));
            tableNode.Nodes.Add(DbObjectsTreeHelper.CreateFolderNode("Indexes", "Indexes", true));
            tableNode.Nodes.Add(DbObjectsTreeHelper.CreateFolderNode("Keys", "Keys", true));
            tableNode.Nodes.Add(DbObjectsTreeHelper.CreateFolderNode("Constraints", "Constraints", true));

            tableNode.Expand();
        }

        private async Task AddTableObjectNodes(TreeNode treeNode, Table table, DatabaseObjectType databaseObjectType)
        {            
            string nodeName = treeNode.Name;
            string database = this.GetDatabaseNode(treeNode).Name;
            DbInterpreter dbInterpreter = this.GetDbInterpreter(database, false);

            dbInterpreter.Subscribe(this);           

            SchemaInfo schemaInfo = await Task.Run(() => dbInterpreter.GetSchemaInfoAsync(new SchemaInfoFilter() { DatabaseObjectType = databaseObjectType, TableNames = new string[] { table.Name } }));           
         
            this.ClearNodes(treeNode);

            #region Columns           
            if (nodeName == "Columns")
            {
                foreach (TableColumn column in schemaInfo.TableColumns)
                {
                    string text = this.GetColumnText(dbInterpreter, table, column);
                    bool isPrimaryKey = schemaInfo.TablePrimaryKeys.Any(item => item.ColumnName == column.Name);
                    bool isForeignKey = schemaInfo.TableForeignKeys.Any(item => item.ColumnName == column.Name);
                    string imageKeyName = isPrimaryKey ? nameof(TablePrimaryKey) : (isForeignKey ? nameof(TableForeignKey) : nameof(TableColumn));

                    TreeNode node = DbObjectsTreeHelper.CreateTreeNode(column.Name, text, imageKeyName);
                    node.Tag = column;

                    treeNode.Nodes.Add(node);
                }
            }
            #endregion

            if (nodeName == "Triggers")
            {
                treeNode.AddDbObjectNodes(schemaInfo.TableTriggers);
            }

            #region Indexes
            if (nodeName == "Indexes" && schemaInfo.TableIndexes.Any())
            {
                ILookup<string, TableIndex> indexLookup = schemaInfo.TableIndexes.ToLookup(item => item.Name);

                foreach (var kp in indexLookup)
                {
                    string indexName = kp.Key;
                    bool isUnique = kp.Any(item => item.IsUnique);
                    string strColumns = string.Join(",", kp.Select(item => item.ColumnName));
                    string content = isUnique ? $"(Unique, {strColumns})" : $"({strColumns})";

                    string text = $"{indexName}{content}";
                    string imageKeyName = nameof(TableIndex);

                    TreeNode node = DbObjectsTreeHelper.CreateTreeNode(indexName, text, imageKeyName);
                    node.Tag = kp.First();

                    treeNode.Nodes.Add(node);
                }
            }
            #endregion
            if (nodeName == "Keys")
            {
                if (schemaInfo.TablePrimaryKeys.Any() || schemaInfo.TablePrimaryKeys.Any())
                {
                    foreach (TablePrimaryKey key in schemaInfo.TablePrimaryKeys)
                    {
                        TreeNode node = DbObjectsTreeHelper.CreateTreeNode(key);
                        treeNode.Nodes.Add(node);
                    }

                    foreach (TableForeignKey key in schemaInfo.TableForeignKeys)
                    {
                        TreeNode node = DbObjectsTreeHelper.CreateTreeNode(key);
                        treeNode.Nodes.Add(node);
                    }
                }
            }

            #region Constraints
            if (nodeName == "Constraints" && schemaInfo.TableConstraints.Any())
            {
                foreach (TableConstraint constraint in schemaInfo.TableConstraints)
                {
                    TreeNode node = DbObjectsTreeHelper.CreateTreeNode(constraint);
                    treeNode.Nodes.Add(node);
                }
            }
            #endregion          

            treeNode.Expand();

            this.Feedback("");
        }

        private string GetColumnText(DbInterpreter dbInterpreter, Table table, TableColumn column)
        {
            string text = dbInterpreter.ParseColumn(table, column).Replace(dbInterpreter.QuotationLeftChar.ToString(), "").Replace(dbInterpreter.QuotationRightChar.ToString(), "");

            return $"{column.Name} ({text.Replace(column.Name + " ", "").ToLower()})";
        }

        private async void tvDbObjects_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;

            if (!this.IsOnlyHasFakeChild(node))
            {
                return;
            }

            this.tvDbObjects.BeginInvoke(new Action(() => this.LoadChildNodes(node)));
        }

        private void ClearNodes(TreeNode node)
        {
            node.Nodes.Clear();
        }

        private void ShowLoading(TreeNode node)
        {
            string loadingImageKey = "Loading.gif";
            string loadingText = "loading..";

            if(this.IsOnlyHasFakeChild(node))
            {
                node.Nodes[0].ImageKey = loadingImageKey ;
                node.Nodes[0].Text = loadingText;
            }
            else
            {
                node.Nodes.Add(DbObjectsTreeHelper.CreateTreeNode("Loading", loadingText, loadingImageKey));
            }
        }

        private async Task LoadChildNodes(TreeNode node)
        {
            this.ShowLoading(node);          

            object tag = node.Tag;

            if (tag is Database)
            {
                Database database = tag as Database;
                await this.AddDbObjectNodes(node, database.Name, DbObjectsTreeHelper.DefaultObjectType | DatabaseObjectType.Function | DatabaseObjectType.Procedure);
            }
            else if (tag is Table)
            {
                Table table = tag as Table;
                this.AddTableFakeNodes(node, table);
            }
            else if (tag == null)
            {
                string name = node.Name;

                TreeNode parentNode = node.Parent;

                if (parentNode.Tag is Database)
                {
                    string databaseName = parentNode.Name;
                    DatabaseObjectType databaseObjectType = DbObjectsTreeHelper.GetDbObjectTypeByFolderName(name);
                    if (databaseObjectType != DatabaseObjectType.None)
                    {
                        await this.AddDbObjectNodes(node, databaseName, databaseObjectType, false);
                    }
                }
                else if (parentNode.Tag is Table)
                {
                    DatabaseObjectType databaseObjectType = DatabaseObjectType.None;
                    switch (name)
                    {
                        case "Columns":
                            databaseObjectType = DatabaseObjectType.TableColumn | DatabaseObjectType.TablePrimaryKey | DatabaseObjectType.TableForeignKey;
                            break;
                        case "Triggers":
                            databaseObjectType = DatabaseObjectType.TableTrigger;
                            break;
                        case "Indexes":
                            databaseObjectType = DatabaseObjectType.TableIndex;
                            break;
                        case "Keys":
                            databaseObjectType = DatabaseObjectType.TablePrimaryKey | DatabaseObjectType.TableForeignKey;
                            break;
                        case "Constraints":
                            databaseObjectType = DatabaseObjectType.TableConstraint;
                            break;
                    }

                    await this.AddTableObjectNodes(node, parentNode.Tag as Table, databaseObjectType);
                }
            }
        }

        private async void tsmiRefresh_Click(object sender, EventArgs e)
        {
            await this.RefreshNode();
        }

        private async Task RefreshNode()
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            TreeNode node = this.GetSelectedNode();

            if (this.CanRefresh(node))
            {
                await this.LoadChildNodes(node);
            }
        }

        private bool IsValidSelectedNode()
        {
            TreeNode node = this.GetSelectedNode();

            return node != null;
        }

        private TreeNode GetSelectedNode()
        {
            return this.tvDbObjects.SelectedNode;
        }

        private async void tsmiGenerateScripts_Click(object sender, EventArgs e)
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            TreeNode node = this.GetSelectedNode();

            await this.GenerateScripts(node);
        }

        private async Task GenerateScripts(TreeNode node)
        {
            object tag = node.Tag;

            if (tag is Database)
            {
                Database database = tag as Database;

                frmGenerateScripts frmGenerateScripts = new frmGenerateScripts(this.databaseType, this.GetConnectionInfo(database.Name));
                frmGenerateScripts.ShowDialog();
            }
            else if (tag is DatabaseObject)
            {
                string databaseName = this.GetDatabaseNode(node).Name;

                await this.GenerateObjectScript(databaseName, tag as DatabaseObject);
            }
        }

        private async Task GenerateObjectScript(string database, DatabaseObject obj)
        {
            string typeName = obj.GetType().Name;
            DbInterpreter dbInterpreter = this.GetDbInterpreter(database, false);

            if (obj is Table)
            {
                dbInterpreter.Option.GetTableAllObjects = true;
            }

            DatabaseObjectType databaseObjectType = (DatabaseObjectType)Enum.Parse(typeof(DatabaseObjectType), typeName);

            SchemaInfoFilter filter = new SchemaInfoFilter() { DatabaseObjectType = databaseObjectType };
            filter.GetType().GetProperty($"{typeName}Names").SetValue(filter, new string[] { obj.Name });

            SchemaInfo schemaInfo = await dbInterpreter.GetSchemaInfoAsync(filter);
            string script = dbInterpreter.GenerateSchemaScripts(schemaInfo).ToString();

            if (this.OnShowContent != null)
            {
                this.OnShowContent(new DatabaseObjectDisplayInfo() { Name = obj.Name, DatabaseType = this.databaseType, DatabaseObject = obj, Content = script, ConnectionInfo = dbInterpreter.ConnectionInfo });
            }
        }

        private void tsmiConvert_Click(object sender, EventArgs e)
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            TreeNode node = this.GetSelectedNode();           

            this.ConvertDatabase(node);
        }

        private void ConvertDatabase(TreeNode node)
        {
            Database database = node.Tag as Database;

            frmConvert frmConvert = new frmConvert(this.databaseType, this.GetConnectionInfo(database.Name));
            frmConvert.ShowDialog();
        }

        private async void tsmiClearData_Click(object sender, EventArgs e)
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            if (MessageBox.Show("Are you sure to clear all data of the database?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TreeNode node = this.GetSelectedNode();

                await this.ClearData(node.Name);
            }
        }

        private async Task ClearData(string database)
        {
            DbInterpreter dbInterpreter = this.GetDbInterpreter(database);
            dbInterpreter.Subscribe(this);

            await Task.Run(()=> dbInterpreter.ClearDataAsync());

            if (!dbInterpreter.HasError)
            {
                MessageBox.Show("Data has been cleared.");
            }
        }

        private void Feedback(FeedbackInfo info)
        {
            if (this.OnFeedback != null)
            {
                this.OnFeedback(info);
            }
        }

        private void Feedback(string message)
        {
            this.Feedback(new FeedbackInfo() { Message = message });
        }

        private async void tsmiEmptyDatabase_Click(object sender, EventArgs e)
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            if (MessageBox.Show("Are you sure to delelte all objects of the database?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                frmDatabaseObjectTypeSelector selector = new frmDatabaseObjectTypeSelector();

                if (selector.ShowDialog() == DialogResult.OK)
                {
                    TreeNode node = this.GetSelectedNode();

                    await Task.Run(() => this.EmptyDatabase(node.Name, selector.DatabaseObjectType));

                    await this.LoadChildNodes(node);

                    this.Feedback("");
                }
            }
        }

        private async Task EmptyDatabase(string database, DatabaseObjectType databaseObjectType)
        {
            DbInterpreter dbInterpreter = this.GetDbInterpreter(database);
            dbInterpreter.Subscribe(this);

            await dbInterpreter.EmptyDatabaseAsync(databaseObjectType);

            if (!dbInterpreter.HasError)
            {
                MessageBox.Show("Seleted database objects have been deleted.");
            }
        }

        private async void tvDbObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                await this.RefreshNode();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                this.DeleteNode();
            }
        }

        private void DeleteNode()
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            TreeNode node = this.GetSelectedNode();

            if (this.CanDelete(node))
            {
                if (MessageBox.Show("Are you sure to delete this object?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DropDbObject(node);
                }
            }
        }

        private async void DropDbObject(TreeNode node)
        {
            string database = this.GetDatabaseNode(node).Name;
            DatabaseObject dbObject = node.Tag as DatabaseObject;

            DbInterpreter dbInterpreter = this.GetDbInterpreter(database);
            dbInterpreter.Subscribe(this);

            await dbInterpreter.Drop(dbObject);

            if (!dbInterpreter.HasError)
            {
                node.Parent.Nodes.Remove(node);
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            this.DeleteNode();
        }

        private void tsmiViewData_Click(object sender, EventArgs e)
        {
            if (!this.IsValidSelectedNode())
            {
                return;
            }

            TreeNode node = this.GetSelectedNode();

            this.ViewData(node);
        }

        private void ViewData(TreeNode node)
        {
            string database = this.GetDatabaseNode(node).Name;
            Table table = node.Tag as Table;            

            if (this.OnShowContent != null)
            {
                this.OnShowContent(new DatabaseObjectDisplayInfo() { Name = table.Name, DatabaseType = this.databaseType, DatabaseObject = table, DisplayType= DatabaseObjectDisplayType.Data, ConnectionInfo = this.GetConnectionInfo(database) });
            }
        }

        public void OnNext(FeedbackInfo value)
        {
            this.Feedback(value);
        }

        public void OnError(Exception error)
        {           
        }

        public void OnCompleted()
        {            
        }

        private void tvDbObjects_AfterExpand(object sender, TreeViewEventArgs e)
        {
            this.Feedback("");
        }

        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}