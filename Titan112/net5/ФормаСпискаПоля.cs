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
        private PropertyInfo[] properties;
        private int Page = 1;
        private int Skip = 0;
        private const int Take = 20;
        private int MaxPages = 1;


        public ФормаСпискаПоля(Type typeOfObject, string nameOfObject, TitanDBContext db)
        {
            InitializeComponent();
            this.typeOfObject = typeOfObject;
            this.nameOfObject = nameOfObject;
            this.db = db;
            Text = nameOfObject;
            toolStripLabel1.Text = $"Страница {Page}";
            properties = typeOfObject.GetProperties();


            foreach (var field in properties)
            {
                table.Columns.Add($"{field.Name}", field.PropertyType);
            }

            DataGridFill();

            if (list.Count % Take == 0)
                MaxPages = list.Count / Take;
            else
                MaxPages = (list.Count / Take) + 1;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var formEdit = new ФормаОбъектаDB(db, typeOfObject, list[e.RowIndex + (Take * (Page - 1))]);
            formEdit.Owner = this;
            formEdit.ShowDialog(this);
        }

        public void DataGridFill()
        {
            var constructor = typeOfObject.GetConstructor(new Type[] { });
            var ads = constructor.Invoke(new object[] { });

            list = (ads as NsgDataItem).GetDBObjects(db);

            int indexRow = 0;
            int indexCell = 0;
            foreach (var obj in list.Skip(Skip).Take(Take))
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

        public void UpdateDataGrid()
        {
            table.Clear();
            DataGridFill();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Page > 1) 
            {
                Page--;
                if (Page == 1)
                    Skip = 0;
                else
                    Skip = (Page - 1) * Take;

                UpdateDataGrid();
                toolStripLabel1.Text = $"Страница {Page}";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Page < MaxPages)
            {
                Page++;
                Skip = (Page - 1) * Take;

                UpdateDataGrid();
                toolStripLabel1.Text = $"Страница {Page}";
            }
        }

    }
}
