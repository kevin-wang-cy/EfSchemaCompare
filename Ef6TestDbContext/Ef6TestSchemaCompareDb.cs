﻿#region licence
// =====================================================
// EfSchemeCompare Project - project to compare EF schema to SQL schema
// Filename: TestEf6SchemaCompareDb.cs
// Date Created: 2015/10/31
// © Copyright Selective Analytics 2015. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using EfPocoClasses.ClassTypes;
using EfPocoClasses.DataTypes;
using EfPocoClasses.Relationships;

namespace Ef6TestDbContext
{
    public class TestEf6SchemaCompareDb : DbContext
    {
        public const string EfDatabaseConfigName = "TestEf6SchemaCompareDb";

        public TestEf6SchemaCompareDb()
        {
        }

        public TestEf6SchemaCompareDb(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }


        public DbSet<DataTop> DataTops { get; set; }
        public DbSet<DataChild> DataChilds { get; set; }
        public DbSet<DataManyChildren> DataManyChildrens { get; set; }
        public DbSet<DataSingleton> DataSingletons { get; set; }
        public DbSet<DataCompKey> DataCompKeys { get; set; }
        public DbSet<DataComplex> DataComplexs { get; set; }
        public DbSet<DataIntDouble> DataIntDoubles { get; set; }
        public DbSet<DataStringByte> DataStringBytes { get; set; }
        public DbSet<DataDate> DataDates { get; set; }
        public DbSet<DataGuidEnum> DataGuidEnums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<DataTop>()
                .HasOptional(x => x.SingletonNullable)
                .WithRequired(x => x.Parent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DataTop>()
                .HasOptional(x => x.ZeroOrOneData)
                .WithRequired()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DataChild>()
                .Property(t => t.MyInt)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DataSingleton>()
                .HasRequired(x => x.Parent)
                .WithOptional(x => x.SingletonNullable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DataTop>()
                .HasMany(r => r.ManyChildren)
                .WithMany(r => r.ManyParents)
                .Map(m =>
                {
                    m.MapLeftKey("DataTopId");
                    m.MapRightKey("DataManyChildrenId");
                    m.ToTable("NonStandardManyToManyTableName");
                });

            modelBuilder.Entity<DataIntDouble>().Property(x => x.DataDecimalSmallPrecision).HasPrecision(5, 3);

            base.OnModelCreating(modelBuilder);
        }
    }
}