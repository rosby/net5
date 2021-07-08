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
        private TitanDBContext db;
        public List<Type> объектыДляАдминистрирования = new List<Type>();
        public List<string> названияОбъектовДляАдминистрирования = new List<string>();


        public ФормаСписка()
        {
            InitializeComponent();
            db = new TitanDBContext();

            #region Названия типов
            названияОбъектовДляАдминистрирования.Add("Баланс");
            названияОбъектовДляАдминистрирования.Add("Тревога автомобилей");
            названияОбъектовДляАдминистрирования.Add("Тревоги");
            названияОбъектовДляАдминистрирования.Add("Причины отмены сигнала тревоги");
            названияОбъектовДляАдминистрирования.Add("Автомобили");
            названияОбъектовДляАдминистрирования.Add("Скидки");
            названияОбъектовДляАдминистрирования.Add("Охранники");
            названияОбъектовДляАдминистрирования.Add("Изображения");
            названияОбъектовДляАдминистрирования.Add("Изображения документов");
            названияОбъектовДляАдминистрирования.Add("Информация");
            названияОбъектовДляАдминистрирования.Add("Платежные документы");
            названияОбъектовДляАдминистрирования.Add("Платежные запросы");
            названияОбъектовДляАдминистрирования.Add("Регистрационные записи");
            названияОбъектовДляАдминистрирования.Add("Регистрационные запросы");
            названияОбъектовДляАдминистрирования.Add("Подчиненные пользователи");
            названияОбъектовДляАдминистрирования.Add("Запросы на смену тарифа");
            названияОбъектовДляАдминистрирования.Add("Тарифы");
            названияОбъектовДляАдминистрирования.Add("Тарифные пакеты");
            названияОбъектовДляАдминистрирования.Add("Пользователи");
            названияОбъектовДляАдминистрирования.Add("Пользовательские подписки");
            названияОбъектовДляАдминистрирования.Add("Пользовательские токены");

            #endregion

            #region Добавление типов
            объектыДляАдминистрирования.Add(typeof(ActualBalanceReg));
            объектыДляАдминистрирования.Add(typeof(AlarmCarsDb));
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
            #endregion

            dataGridView1.Columns.Add("Column1", "Объекты для администрирования");
            dataGridView1.Columns["Column1"].Width = 450;
            for (var i = 0; i < объектыДляАдминистрирования.Count; i++)
            {
                dataGridView1.Rows.Add($"{названияОбъектовДляАдминистрирования[i]} ({объектыДляАдминистрирования[i]})", "");
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var текИндексСтроки = e.RowIndex;
            var формаСпискаПоля = new ФормаСпискаПоля(объектыДляАдминистрирования[текИндексСтроки], названияОбъектовДляАдминистрирования[текИндексСтроки], db);
            формаСпискаПоля.Show();
        }
    }
}
