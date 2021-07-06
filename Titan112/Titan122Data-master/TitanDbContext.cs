using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    public partial class TitanDBContext : DbContext
    {
        public DbSet<UserItemDb> Users { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<TarifItemDb> Tarifs { get; set; }
        public DbSet<RegistrationRequestDb> RegistrationRequests { get; set; }
        public DbSet<InformationDb> Information { get; set; }
        public DbSet<AlarmCancelReasonDb> AlarmCancelReasons { get; set; }
        public DbSet<GuardianItemDb> Guardians { get; set; }
        public DbSet<PaymentDoc> PaymentDocs { get; set; }
        public DbSet<ActualBalanceReg> ActualBalanceRegs { get; set; }
        public DbSet<RegistrationRecord> RegistrationJournal { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<AlarmItemDb> Alarms { get; set; }
        public DbSet<CarItemDb> Cars { get; set; }
        public DbSet<AlarmCarsDb> AlarmCars { get; set; }
        public DbSet<ImageDb> Images { get; set; }
        public DbSet<ImageDocument> ImageDocuments { get; set; }
        public DbSet<TarifPackDb> TarifPacks { get; set; }
        public DbSet<SlaveUser> SlaveUsers { get; set; }
        public DbSet<UserSubsctription> UserSubsctriptions { get; set; }
        public DbSet<TarifChangeRequestDb> TarifChangeRequests { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public TitanDBContext()
            : base()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();            // создаем базу данных при первом обращении
            //var count = Users.Count<UserItemDb>();
        }

        public void CheckDB()
        {
            createEmptyDB();

            //Database.EnsureCreated();
        }
        private void createEmptyDB()
        {
            //DELETE AND CREATE DB STRUCTURE
            //Database.EnsureDeleted();
            Database.EnsureCreated();

            TarifItemDb.InitialFill(Tarifs, this);
            
            InformationDb.InitialFill(Information, this);
            AlarmCancelReasonDb.InitialFill(AlarmCancelReasons, this);
            Discount.InitialFill(Discounts, this);
            
            SaveChanges();
        }
        public TitanDBContext(DbContextOptions<TitanDBContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();            // создаем базу данных при первом обращении
            var count = Users.Count<UserItemDb>();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseLazyLoadingProxies()
        //        .UseSqlServer(@"Server = ALEX2020; Integrated Security = SSPI; " +
        //        "Initial Catalog=Titan112_copy");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "server=10.10.4.219;user=nsg-user;password=NSGtitan002!;database=Titan112;";
            ServerVersion version = ServerVersion.AutoDetect(connection);
            optionsBuilder.UseLazyLoadingProxies().UseMySql(connection, version);
        }
    }
}
