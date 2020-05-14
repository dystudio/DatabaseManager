﻿using DatabaseInterpreter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseInterpreter.Core
{
    public class MySqlScriptGenerator : DbScriptGenerator
    {
        public MySqlScriptGenerator(DbInterpreter dbInterpreter) : base(dbInterpreter) { }

        #region Generate Schema Scripts 

        public override ScriptBuilder GenerateSchemaScripts(SchemaInfo schemaInfo)
        {
            ScriptBuilder sb = new ScriptBuilder();
            MySqlInterpreter mySqlInterpreter = this.dbInterpreter as MySqlInterpreter;
            string dbCharSet = mySqlInterpreter.DbCharset;
            string notCreateIfExistsClause = mySqlInterpreter.NotCreateIfExistsClause;

            #region Function           
            sb.AppendRange(this.GenerateScriptDbObjectScripts<Function>(schemaInfo.Functions));
            #endregion

            #region Create Table
            foreach (Table table in schemaInfo.Tables)
            {
                this.FeedbackInfo(OperationState.Begin, table);

                string tableName = table.Name;
                string quotedTableName = this.GetQuotedObjectName(table);

                IEnumerable<TableColumn> tableColumns = schemaInfo.TableColumns.Where(item => item.TableName == tableName).OrderBy(item => item.Order);

                IEnumerable<TablePrimaryKey> primaryKeys = schemaInfo.TablePrimaryKeys.Where(item => item.TableName == tableName);
                IEnumerable<TableForeignKey> foreignKeys = schemaInfo.TableForeignKeys.Where(item => item.TableName == tableName);
                IEnumerable<TableIndex> indexes = schemaInfo.TableIndexes.Where(item => item.TableName == tableName).OrderBy(item => item.Order);

                this.RestrictColumnLength(tableColumns, primaryKeys.SelectMany(item => item.Columns));
                this.RestrictColumnLength(tableColumns, foreignKeys.SelectMany(item => item.Columns));
                this.RestrictColumnLength(tableColumns, indexes.SelectMany(item => item.Columns));

                #region Table

                string tableScript =
$@"
CREATE TABLE {notCreateIfExistsClause} {quotedTableName}(
{string.Join("," + Environment.NewLine, tableColumns.Select(item => this.dbInterpreter.ParseColumn(table, item)))}
){(!string.IsNullOrEmpty(table.Comment) ? ($"comment='{this.dbInterpreter.ReplaceSplitChar(ValueHelper.TransferSingleQuotation(table.Comment))}'") : "")}
DEFAULT CHARSET={dbCharSet}" + this.scriptsDelimiter;

                sb.AppendLine(new CreateDbObjectScript<Table>(tableScript));

                #endregion

                #region Primary Key
                if (this.option.TableScriptsGenerateOption.GeneratePrimaryKey && primaryKeys.Count() > 0)
                {
                    TablePrimaryKey primaryKey = primaryKeys.FirstOrDefault();

                    if (primaryKey != null)
                    {
                        sb.AppendLine(this.AddPrimaryKey(primaryKey));
                    }
                }
                #endregion

                List<string> foreignKeysLines = new List<string>();

                #region Foreign Key
                if (this.option.TableScriptsGenerateOption.GenerateForeignKey)
                {
                    foreach (TableForeignKey foreignKey in foreignKeys)
                    {
                        sb.AppendLine(this.AddForeignKey(foreignKey));
                    }
                }

                #endregion

                #region Index
                if (this.option.TableScriptsGenerateOption.GenerateIndex)
                {
                    foreach (TableIndex index in indexes)
                    {
                        sb.AppendLine(this.AddIndex(index));
                    }
                }
                #endregion              

                sb.AppendLine();

                this.FeedbackInfo(OperationState.End, table);
            }
            #endregion            

            #region View           
            sb.AppendRange(this.GenerateScriptDbObjectScripts<View>(schemaInfo.Views));

            #endregion

            #region Trigger           
            sb.AppendRange(this.GenerateScriptDbObjectScripts<TableTrigger>(schemaInfo.TableTriggers));
            #endregion

            #region Procedure           
            sb.AppendRange(this.GenerateScriptDbObjectScripts<Procedure>(schemaInfo.Procedures));
            #endregion

            if (this.option.ScriptOutputMode.HasFlag(GenerateScriptOutputMode.WriteToFile))
            {
                this.AppendScriptsToFile(sb.ToString(), GenerateScriptMode.Schema, true);
            }

            return sb;
        }

        private void RestrictColumnLength<T>(IEnumerable<TableColumn> columns, IEnumerable<T> children) where T : SimpleColumn
        {
            var childColumns = columns.Where(item => children.Any(t => item.Name == t.ColumnName)).ToList();

            childColumns.ForEach(item =>
            {
                if (DataTypeHelper.IsCharType(item.DataType) && item.MaxLength > MySqlInterpreter.KeyIndexColumnMaxLength)
                {
                    item.MaxLength = MySqlInterpreter.KeyIndexColumnMaxLength;
                }
            });
        }

        private string GetRestrictedLengthName(string name)
        {
            if (name.Length > MySqlInterpreter.NameMaxLength)
            {
                return name.Substring(0, MySqlInterpreter.NameMaxLength);
            }

            return name;
        }

        #endregion

        #region Data Script    

        public override Task<string> GenerateDataScriptsAsync(SchemaInfo schemaInfo)
        {
            return base.GenerateDataScriptsAsync(schemaInfo);
        }

        protected override string GetBytesConvertHexString(object value, string dataType)
        {
            string hex = string.Concat(((byte[])value).Select(item => item.ToString("X2")));
            return $"UNHEX('{hex}')";
        }
        #endregion

        #region Alter Table
        public override Script RenameTable(Table table, string newName)
        {
            return new AlterDbObjectScript<Table>($"ALTER TABLE {this.GetQuotedString(table.Name)} RENAME {this.GetQuotedString(newName)};");
        }

        public override Script SetTableComment(Table table, bool isNew = true)
        {
            return new AlterDbObjectScript<Table>($"ALTER TABLE {this.GetQuotedString(table.Name)} COMMENT = '{this.dbInterpreter.ReplaceSplitChar(ValueHelper.TransferSingleQuotation(table.Comment))}';");
        }

        public override Script AddTableColumn(Table table, TableColumn column)
        {
            return new CreateDbObjectScript<TableColumn>($"ALTER TABLE {this.GetQuotedString(table.Name)} ADD { this.dbInterpreter.ParseColumn(table, column)};");
        }

        public override Script RenameTableColumn(Table table, TableColumn column, string newName)
        {
            return new AlterDbObjectScript<TableColumn>($"ALTER TABLE {this.GetQuotedString(table.Name)} CHANGE {this.GetQuotedString(column.Name)} {newName} {this.dbInterpreter.ParseDataType(column)};");
        }

        public override Script AlterTableColumn(Table table, TableColumn column)
        {
            return new AlterDbObjectScript<TableColumn>($"ALTER TABLE {this.GetQuotedString(table.Name)} MODIFY COLUMN {this.dbInterpreter.ParseColumn(table, column)}");
        }

        public override Script SetTableColumnComment(Table table, TableColumn column, bool isNew = true)
        {
            return new AlterDbObjectScript<TableColumn>($"ALTER TABLE {this.GetQuotedString(column.TableName)} MODIFY COLUMN {this.dbInterpreter.ParseColumn(table, column)}");
        }

        public override Script DropTableColumn(TableColumn column)
        {
            return new DropDbObjectScript<TableColumn>($"ALTER TABLE {this.GetQuotedString(column.TableName)} DROP COLUMN {this.GetQuotedString(column.Name)};");
        }

        public override Script AddPrimaryKey(TablePrimaryKey primaryKey)
        {
            string columnNames = string.Join(",", primaryKey.Columns.Select(item => this.GetQuotedString(item.ColumnName)));

            string sql = $"ALTER TABLE {this.GetQuotedString(primaryKey.TableName)} ADD CONSTRAINT { this.GetQuotedString(this.GetRestrictedLengthName(primaryKey.Name))} PRIMARY KEY ({columnNames})";

            if (!string.IsNullOrEmpty(primaryKey.Comment))
            {
                sql += $" COMMENT '{primaryKey.Comment}'";
            }

            return new CreateDbObjectScript<TablePrimaryKey>(sql + this.scriptsDelimiter);
        }

        public override Script DropPrimaryKey(TablePrimaryKey primaryKey)
        {
            return new DropDbObjectScript<TablePrimaryKey>($"ALTER TABLE {this.GetQuotedString(primaryKey.TableName)} DROP PRIMARY KEY");
        }

        public override Script AddForeignKey(TableForeignKey foreignKey)
        {
            string columnNames = string.Join(",", foreignKey.Columns.Select(item => this.GetQuotedString(item.ColumnName)));
            string referenceColumnName = string.Join(",", foreignKey.Columns.Select(item => $"{ this.GetQuotedString(item.ReferencedColumnName)}"));

            string sql = $"ALTER TABLE {this.GetQuotedString(foreignKey.TableName)} ADD CONSTRAINT { this.GetQuotedString(this.GetRestrictedLengthName(foreignKey.Name))} FOREIGN KEY ({columnNames}) REFERENCES { this.GetQuotedString(foreignKey.ReferencedTableName)}({referenceColumnName})";

            if (foreignKey.UpdateCascade)
            {
                sql += " ON UPDATE CASCADE";
            }
            else
            {
                sql += " ON UPDATE NO ACTION";
            }

            if (foreignKey.DeleteCascade)
            {
                sql += " ON DELETE CASCADE";
            }
            else
            {
                sql += " ON DELETE NO ACTION";
            }

            return new CreateDbObjectScript<TableForeignKey>(sql + this.scriptsDelimiter);
        }

        public override Script DropForeignKey(TableForeignKey foreignKey)
        {
            return new DropDbObjectScript<TableForeignKey>($"ALTER TABLE {this.GetQuotedString(foreignKey.TableName)} DROP FOREIGN KEY {this.GetQuotedString(foreignKey.Name)}");
        }
        public override Script AddIndex(TableIndex index)
        {
            string columnNames = string.Join(",", index.Columns.Select(item => $"{this.GetQuotedString(item.ColumnName)}"));

            string type = "";

            if (index.Type == IndexType.Unique.ToString())
            {
                type = "UNIQUE";
            }
            else if (index.Type == IndexType.FullText.ToString())
            {
                type = "FULLTEXT";
            }

            string sql = $"ALTER TABLE {this.GetQuotedString(index.TableName)} ADD {type} INDEX {this.GetQuotedString(this.GetRestrictedLengthName(index.Name))} ({columnNames})";

            if (!string.IsNullOrEmpty(index.Comment))
            {
                sql += $" COMMENT('{index.Comment}')";
            }

            return new CreateDbObjectScript<TableIndex>(sql + this.scriptsDelimiter);
        }

        public override Script DropIndex(TableIndex index)
        {
            return new DropDbObjectScript<TableIndex>($"ALTER TABLE {this.GetQuotedString(index.TableName)} DROP INDEX {this.GetQuotedString(index.Name)}");
        }

        public override Script AddConstraint(TableConstraint constraint)
        {
            return new Script("");
        }

        public override Script DropConstraint(TableConstraint constraint)
        {
            return new Script("");
        }

        #endregion

        #region Database Operation

        public override Script DropUserDefinedType(UserDefinedType userDefinedType)
        {
            return new Script("");
        }

        public override Script DropTable(Table table)
        {
            return new DropDbObjectScript<Table>(this.GetDropSql(nameof(Table), table));
        }

        public override Script DropView(View view)
        {
            return new DropDbObjectScript<View>(this.GetDropSql(nameof(View), view));
        }

        public override Script DropTrigger(TableTrigger trigger)
        {
            return new DropDbObjectScript<View>(this.GetDropSql("trigger", trigger));
        }

        public override Script DropFunction(Function function)
        {
            return new DropDbObjectScript<Function>(this.GetDropSql(nameof(Function), function));
        }

        public override Script DropProcedure(Procedure procedure)
        {
            return new DropDbObjectScript<Procedure>(this.GetDropSql(nameof(Procedure), procedure));
        }

        private string GetDropSql(string typeName, DatabaseObject dbObject)
        {
            return $"DROP {typeName.ToUpper()} IF EXISTS {this.GetQuotedObjectName(dbObject)};";
        }

        #endregion
    }
}