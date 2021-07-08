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
    public partial class ФормаРедактирования : Form
    {
        private string nameOfField;
        private TitanDBContext db;
        private int rowIndex;
        private NsgDataItem editObject;
        private Type typeOfCell;
        private object cell;

        public ФормаРедактирования(TitanDBContext db, string nameOfField, int rowIndex, NsgDataItem editObject, object cell, Type typeOfCell)
        {
            InitializeComponent();
            Text = $"Редактирование поля {nameOfField}";
            this.nameOfField = nameOfField;
            this.rowIndex = rowIndex;
            this.db = db;
            this.cell = cell;
            this.editObject = editObject;
            this.typeOfCell = typeOfCell;


            if (typeOfCell == typeof(bool))
            {
                textBox1.Visible = false;
                checkBox1.Visible = true;
                checkBox1.Checked = (bool)cell;
            }
            else if (typeOfCell == typeof(DateTime))
            {
                textBox1.Visible = false;
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;
                dateTimePicker2.ShowUpDown = true;

                if (dateTimePicker1.MaxDate >= (DateTime)cell & (DateTime)cell >= dateTimePicker1.MinDate)
                {
                    dateTimePicker1.Value = ((DateTime)cell).Date;
                    dateTimePicker2.Value = ((DateTime)cell);
                }
                else 
                {
                    dateTimePicker1.Value = DateTime.Today;
                    dateTimePicker2.Value = DateTime.Today;
                }
            }
            else 
            {
                if(cell != null)
                    textBox1.Text = cell.ToString();
                label1.Text = $"Тип редактируемого значения {typeOfCell}";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var OwnerOfThisForm = Owner as ФормаСпискаПоля;

            if (typeOfCell == typeof(bool))
            {
                var field = editObject.GetType().GetProperty(nameOfField);
                field.SetValue(editObject, checkBox1.Checked);
                db.SaveChanges();
                OwnerOfThisForm.UpdateCellInDataGrid(rowIndex, nameOfField, checkBox1.Checked);
            }
            else if (typeOfCell == typeof(DateTime))
            {
                var timeSpan = TimeSpan.Parse(dateTimePicker2.Value.ToString("HH:mm:ss"));
                var field = editObject.GetType().GetProperty(nameOfField);
                field.SetValue(editObject, dateTimePicker1.Value.Date + timeSpan);
                db.SaveChanges();
                OwnerOfThisForm.UpdateCellInDataGrid(rowIndex, nameOfField, dateTimePicker1.Value.Date + timeSpan);
            }
            else 
            {
                var field = editObject.GetType().GetProperty(nameOfField);
                field.SetValue(editObject, Convert.ChangeType(textBox1.Text, typeOfCell));
                db.SaveChanges();
                OwnerOfThisForm.UpdateCellInDataGrid(rowIndex, nameOfField, Convert.ChangeType(textBox1.Text, typeOfCell));
            }
            Close();
        }
    }
}
