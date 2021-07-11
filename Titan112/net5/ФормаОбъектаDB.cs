using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titan112Data;

namespace net5
{
    public partial class ФормаОбъектаDB : Form
    {
        private TitanDBContext db;
        private Type typeOfObject;
        private NsgDataItem editObject;
        private DataTable table = new DataTable();


        public ФормаОбъектаDB(TitanDBContext db, Type typeOfObject, NsgDataItem editObject)
        {
            InitializeComponent();
            Text = $"Редактирование объекта \"{editObject}\"";
            this.db = db;
            this.typeOfObject = typeOfObject;
            this.editObject = editObject;
            DataGridFill();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var nameOfField = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            var cell = table.Rows[e.RowIndex][e.ColumnIndex];
            var typeOfCell = cell.GetType();
            if (typeOfCell == typeof(DBNull)) typeOfCell = table.Columns[e.ColumnIndex].DataType;

            if (typeOfCell.IsPrimitive | typeOfCell == typeof(string) | typeOfCell == typeof(bool) | typeOfCell == typeof(DateTime))
            {
                var formEdit = new ФормаРедактирования(db, nameOfField, e.RowIndex, editObject, cell, typeOfCell);
                formEdit.Owner = this;
                formEdit.ShowDialog(this);
            }
            else if (!typeOfCell.IsValueType & typeOfCell != typeof(byte[]))
            {
                var formEdit = new ФормаРедактированияСсылочногоЗнач(db, nameOfField, e.RowIndex, editObject, typeOfCell);
                formEdit.Owner = this;
                formEdit.ShowDialog(this);
            }

        }

        public void DataGridFill() 
        {
            var properties = typeOfObject.GetProperties();

            foreach (var field in properties)
            {
                table.Columns.Add($"{field.Name}", field.PropertyType);
            }

            int indexCell = 0;
            table.Rows.Add();
            foreach (var field in properties)
            {
                if (field.GetValue(editObject) != null)
                    table.Rows[0][indexCell] = field.GetValue(editObject);
                indexCell++;
            }
            dataGridView1.DataSource = table;
        }

        public void UpdateCellInDataGrid<T>(string nameOfColumn, T value)
        {
            table.Rows[0][nameOfColumn] = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataColumn column in table.Columns)
            {
                var field = editObject.GetType().GetProperty(column.ColumnName);
                if(table.Rows[0][column.ColumnName] is DBNull)
                    field.SetValue(editObject, null);
                else
                    field.SetValue(editObject, table.Rows[0][column.ColumnName]);
            }
            db.SaveChanges();
            (Owner as ФормаСпискаПоля).UpdateDataGrid();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
