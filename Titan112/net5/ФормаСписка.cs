using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Titan112Data;

namespace net5
{
    public partial class ФормаСписка : Form
    {
        public ФормаСписка()
        {
            InitializeComponent();
        }

        public List<Type> объектыДляАдминистрирования = new List<Type>();

        private void ФормаСписка_Load(object sender, EventArgs e)
        {
            объектыДляАдминистрирования.Add(typeof(ActualBalanceReg));
            объектыДляАдминистрирования.Add(typeof(AlarmCarsDb));
            объектыДляАдминистрирования.Add(typeof(AlarmInfoDb));
            объектыДляАдминистрирования.Add(typeof(AlarmItemDb));
            объектыДляАдминистрирования.Add(typeof(AlarmCancelReasonDb));
            объектыДляАдминистрирования.Add(typeof(CarItemDb));
            объектыДляАдминистрирования.Add(typeof(Discount));
            объектыДляАдминистрирования.Add(typeof(GuardianItemDb));
            объектыДляАдминистрирования.Add(typeof(ImageDb));
            объектыДляАдминистрирования.Add(typeof(ImageDocument));
            объектыДляАдминистрирования.Add(typeof(InformationDb));
            объектыДляАдминистрирования.Add(typeof(PaymentDoc));
            объектыДляАдминистрирования.Add(typeof(PaymentRequest));
            объектыДляАдминистрирования.Add(typeof(RegistrationRecord));
            объектыДляАдминистрирования.Add(typeof(RegistrationRequestDb));
            объектыДляАдминистрирования.Add(typeof(SlaveUser));
            объектыДляАдминистрирования.Add(typeof(TarifChangeRequestDb));
            объектыДляАдминистрирования.Add(typeof(TarifItemDb));
            объектыДляАдминистрирования.Add(typeof(TarifPackDb));
            объектыДляАдминистрирования.Add(typeof(UserItemDb));
            объектыДляАдминистрирования.Add(typeof(UserSubsctription));
            объектыДляАдминистрирования.Add(typeof(UserTokens));

            dataGridView1.Columns.Add("Column1", "Объекты для администрирования");
            for (var i = 0; i < объектыДляАдминистрирования.Count; i++)
            {
                dataGridView1.Rows.Add(объектыДляАдминистрирования[i], "");
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var текИндексСтроки = e.RowIndex;
            var формаСпискаПоля = new ФормаСпискаПоля(объектыДляАдминистрирования, текИндексСтроки);
            формаСпискаПоля.ShowDialog(this);
        }
    }
}
