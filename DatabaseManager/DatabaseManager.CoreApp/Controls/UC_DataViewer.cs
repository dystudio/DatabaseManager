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
using DatabaseManager.Model;
using DatabaseInterpreter.Core;
using DatabaseManager.Core;
using DatabaseManager.Helper;

namespace DatabaseManager.Controls
{
    public delegate void DataFilterHandler(object sender);

    public partial class UC_DataViewer : UserControl, IDbObjContentDisplayer
    {
        private DatabaseObjectDisplayInfo displayInfo;
        private int sortedColumnIndex = -1;
        private SortOrder sortOrder = SortOrder.None;
        private bool isSorting = false;
        private QueryConditionBuilder conditionBuilder;

        public IEnumerable<DataGridViewColumn> Columns => this.dgvData.Columns.Cast<DataGridViewColumn>();
        public QueryConditionBuilder ConditionBuilder => this.conditionBuilder;
        public DataFilterHandler OnDataFilter;

        public UC_DataViewer()
        {
            InitializeComponent();

            this.pagination.PageSize = 50;

            this.dgvData.AutoGenerateColumns = true;
        }

        public void Show(DatabaseObjectDisplayInfo displayInfo)
        {
            this.LoadData(displayInfo);
        }

        private async void LoadData(DatabaseObjectDisplayInfo displayInfo, long pageNum = 1, bool isSort = false)
        {
            this.displayInfo = displayInfo;

            this.pagination.PageNum = pageNum;

            Table table = displayInfo.DatabaseObject as Table;

            int pageSize = this.pagination.PageSize;

            DbInterpreter dbInterpreter = DbInterpreterHelper.GetDbInterpreter(displayInfo.DatabaseType, displayInfo.ConnectionInfo, new DbInterpreterOption());

            string orderColumns = "";

            if (this.dgvData.SortedColumn != null)
            {
                string sortOrder = (this.sortOrder == SortOrder.Descending ? "DESC" : "ASC");
                orderColumns = $"{dbInterpreter.GetQuotedString(this.dgvData.SortedColumn.Name)} {sortOrder}";
            }

            string conditionClause = "";

            if (this.conditionBuilder != null && this.conditionBuilder.Conditions.Count > 0)
            {
                this.conditionBuilder.QuotationLeftChar = dbInterpreter.QuotationLeftChar;
                this.conditionBuilder.QuotationRightChar = dbInterpreter.QuotationRightChar;

                conditionClause = "WHERE " + this.conditionBuilder.ToString();
            }

            (long Total, DataTable Data) result = await dbInterpreter.GetPagedDataTableAsync(table, orderColumns, pageSize, pageNum, conditionClause);

            this.pagination.TotalCount = result.Total;

            this.dgvData.DataSource = DataGridViewHelper.ConvertDataTable(result.Data);

            foreach (DataGridViewColumn column in this.dgvData.Columns)
            {
                Type valueType = column.ValueType;

                if (valueType == typeof(byte[]) || valueType.Name == "SqlGeography")
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            if (this.sortedColumnIndex != -1)
            {
                DataGridViewColumn column = this.dgvData.Columns[this.sortedColumnIndex];

                this.isSorting = true;

                ListSortDirection sortDirection = this.GetSortDirection(this.sortOrder);

                this.dgvData.Sort(column, sortDirection);

                this.isSorting = false;
            }
        }

        private ListSortDirection GetSortDirection(SortOrder sortOrder)
        {
            return sortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        private void pagination_OnPageNumberChanged(long pageNum)
        {
            this.LoadData(this.displayInfo, pageNum);
        }       

        public ContentSaveResult Save(ContentSaveInfo info)
        {
            DataTableHelper.WriteToFile(this.dgvData.DataSource as DataTable, info.FilePath);

            return new ContentSaveResult() { IsOK = true };
        }

        private void dgvData_Sorted(object sender, EventArgs e)
        {
            if (this.isSorting)
            {
                return;
            }

            this.sortedColumnIndex = this.dgvData.SortedColumn.DisplayIndex;
            this.sortOrder = this.dgvData.SortOrder;

            this.LoadData(this.displayInfo, 1, true);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (this.OnDataFilter != null)
            {
                this.OnDataFilter(this);
            }
        }

        public void FilterData(QueryConditionBuilder conditionBuilder)
        {
            this.conditionBuilder = conditionBuilder;

            this.LoadData(this.displayInfo, 1, false);
        }

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }       
    }
}
