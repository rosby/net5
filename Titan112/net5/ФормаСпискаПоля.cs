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
    public partial class ФормаСпискаПоля : Form
    {
        private Type typeOfObject;
        private TitanDBContext db;
        private string nameOfObject;
        private List<NsgDataItem> list;
        private DataTable table = new DataTable();



        public ФормаСпискаПоля( Type typeOfObject, string nameOfObject, TitanDBContext db)
        {
            InitializeComponent();
            this.typeOfObject = typeOfObject;
            this.nameOfObject = nameOfObject;
            this.db = db;
            this.Text = nameOfObject;
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
                var formEdit = new ФормаРедактирования(db, nameOfField, e.RowIndex, list[e.RowIndex], cell, typeOfCell);
                formEdit.Owner = this;
                formEdit.ShowDialog(this);
            }
            else if (!typeOfCell.IsValueType & typeOfCell != typeof(byte[])) 
            {
                var formEdit = new ФормаРедактированияСсылочногоЗнач(db, nameOfField, e.RowIndex, list[e.RowIndex], typeOfCell);
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

            var constructor = typeOfObject.GetConstructor(new Type[] { });
            var ads = constructor.Invoke(new object[] { });

            list = (ads as NsgDataItem).GetDBObjects(db);
;
            int indexRow = 0;
            int indexCell = 0;
            foreach (var obj in list)
            {
                
                table.Rows.Add();
                foreach (var field in properties)
                {
                    if (field.GetValue(obj) != null)
                        table.Rows[indexRow][indexCell] = field.GetValue(obj);
                    indexCell++;
                }
                indexCell = 0;
                indexRow++;
            }

            dataGridView1.DataSource = table;
        }

        public void UpdateCellInDataGrid<T>(int rowIndex, string nameOfColumn, T value)
        {
            table.Rows[rowIndex][nameOfColumn] = value;
        }
    }
}
