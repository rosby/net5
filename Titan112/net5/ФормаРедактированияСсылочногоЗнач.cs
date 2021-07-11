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
    public partial class ФормаРедактированияСсылочногоЗнач : Form
    {
        private Type typeOfObject;
        private TitanDBContext db;
        private DataTable table = new DataTable();
        private List<NsgDataItem> list;
        private string nameOfField;
        private int rowIndex;
        private NsgDataItem editObject;



        public ФормаРедактированияСсылочногоЗнач(TitanDBContext db, string nameOfField, int rowIndex, NsgDataItem editObject, Type typeOfObject)
        {
            InitializeComponent();

            this.typeOfObject = typeOfObject;
            this.db = db;
            this.nameOfField = nameOfField;
            this.rowIndex = rowIndex;
            this.editObject = editObject;

            var constructor = typeOfObject.GetConstructor(new Type[] { });
            var ads = constructor.Invoke(new object[] { });
            list = (ads as NsgDataItem).GetDBObjects(db);

            table.Columns.Add("0", typeOfObject);

            foreach (var ObjectDB in list) 
            {
                table.Rows.Add(ObjectDB);
            }

            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Width = 350;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var OwnerOfThisForm = Owner as ФормаОбъектаDB;
            var index = dataGridView1.CurrentRow.Index;

            var field = editObject.GetType().GetProperty(nameOfField);
            field.SetValue(editObject, table.Rows[index]["0"]);
            db.SaveChanges();
            OwnerOfThisForm.UpdateCellInDataGrid(nameOfField, table.Rows[index]["0"]);

            Close();

        }
    }
}
