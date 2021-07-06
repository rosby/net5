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
        private List<Type> объектыДляАдминистрирования;
        private int текИндексСтроки;

        public ФормаСпискаПоля(List<Type> объектыДляАдминистрирования, int текИндексСтроки)
        {
            InitializeComponent();
            this.объектыДляАдминистрирования = объектыДляАдминистрирования;
            this.текИндексСтроки = текИндексСтроки;
        }

        private void ФормаСпискаПоля_Load(object sender, EventArgs e)
        {
            var индексКолонки = 1;
            List<NsgDataItem> list = new List<NsgDataItem>();
            var списокПолей = объектыДляАдминистрирования[текИндексСтроки].GetProperties();
            foreach (var field in списокПолей)
            {
                dataGridView1.Columns.Add($"{индексКолонки}",$"{field.Name}");
                индексКолонки++;
            }

            var db = new TitanDBContext();
            var конструктор = объектыДляАдминистрирования[текИндексСтроки].GetConstructor(new Type[] { });
            var ads = конструктор.Invoke(new Object[] { });

            if (ads is ActualBalanceReg) 
            {
                var actual = (ActualBalanceReg)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is AlarmCarsDb)
            {
                var actual = (AlarmCarsDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is AlarmInfoDb)
            {
                var actual = (AlarmInfoDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is AlarmItemDb)
            {
                var actual = (AlarmItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is AlarmCancelReasonDb)
            {
                var actual = (AlarmCancelReasonDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is AlarmItemDb)
            {
                var actual = (AlarmItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is CarItemDb)
            {
                var actual = (CarItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is Discount)
            {
                var actual = (Discount)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is GuardianItemDb)
            {
                var actual = (GuardianItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is ImageDb)
            {
                var actual = (ImageDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is ImageDocument)
            {
                var actual = (ImageDocument)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is InformationDb)
            {
                var actual = (InformationDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is PaymentDoc)
            {
                var actual = (PaymentDoc)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is PaymentRequest)
            {
                var actual = (PaymentRequest)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is RegistrationRecord)
            {
                var actual = (RegistrationRecord)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is RegistrationRequestDb)
            {
                var actual = (RegistrationRequestDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is SlaveUser)
            {
                var actual = (SlaveUser)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is TarifChangeRequestDb)
            {
                var actual = (TarifChangeRequestDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is TarifItemDb)
            {
                var actual = (TarifItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is TarifPackDb)
            {
                var actual = (TarifPackDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is UserItemDb)
            {
                var actual = (UserItemDb)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is UserSubsctription)
            {
                var actual = (UserSubsctription)ads;
                list = actual.GetDBObjects(db);
            }

            if (ads is UserTokens)
            {
                var actual = (UserTokens)ads;
                list = actual.GetDBObjects(db);
            }


            int indexRow = 0;
            int indexCell = 0;
            foreach (var obj in list) 
            {
                dataGridView1.Rows.Add();
                foreach (var field in списокПолей) 
                {
                    if(field.GetValue(obj) != null)
                        dataGridView1.Rows[indexRow].Cells[indexCell].Value = field.GetValue(obj).ToString();
                    indexCell++;
                }
                indexCell = 0;
                indexRow++;
            }
        }
    }
}
