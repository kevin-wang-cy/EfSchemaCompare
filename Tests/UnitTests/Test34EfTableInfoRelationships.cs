﻿#region licence
// =====================================================
// EfSchemeCompare Project - project to compare EF schema to SQL schema
// Filename: Test30EfTableInfo.cs
// Date Created: 2015/10/31
// © Copyright Selective Analytics 2015. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CompareCore.EFInfo;
using Ef6Compare.Internal;
using NUnit.Framework;
using Tests.EfClasses;
using Tests.EfClasses.ClassTypes;
using Tests.EfClasses.Relationships;
using Tests.Helpers;

namespace Tests.UnitTests
{
    public class Test34EfTableInfoRelationships
    {

        private ICollection<EfTableInfo> _efInfos;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            using (var db = new EfSchemaCompareDb())
            {
                var decoder = new Ef6MetadataDecoder(Assembly.GetAssembly(typeof(EfSchemaCompareDb)));
                _efInfos = decoder.GetAllEfTablesWithColInfo(db);
            }
        }

        [Test]
        public void Test01GetEfTableColumnInfo()
        {
            //SETUP

            //EXECUTE

            //VERIFY
            _efInfos.Count.ShouldEqual(12);
        }

        [Test]
        public void Test10DataTopNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataTop));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataTop");
            CollectionAssert.AreEquivalent(new[] { "DataTopId", "MyString", "DataSingletonId", "Key1", "Key2" }, efInfo.NormalCols.Select(x => x.ClrColumName));
            efInfo.NormalCols.Single(x => x.IsPrimaryKey).ClrColumName.ShouldEqual("DataTopId");
        }

        [Test]
        public void Test11DataTopRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataTop));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            CollectionAssert.AreEquivalent(new[] { "Children", "CompositeKeyData", "ManyChildren", "ManyCompKeys", "SingletonNullable", "ZeroOrOneData" }, efInfo.RelationshipCols.Select(x => x.ClrColumnName));
            CollectionAssert.AreEquivalent(new[] { typeof(ICollection<DataChild>), typeof(DataCompKey), typeof(ICollection<DataManyChildren>), typeof(ICollection<DataManyCompKey>), typeof(DataSingleton), typeof(DataZeroOrOne) }, 
                efInfo.RelationshipCols.Select(x => x.ClrColumnType));
        }

        [Test]
        public void Test20DataChildNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataChild));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataChild");
            CollectionAssert.AreEquivalent(new[] { "DataChildId", "MyInt", "MyString", "DataTopId" }, efInfo.NormalCols.Select(x => x.ClrColumName));
            efInfo.NormalCols.Single(x => x.IsPrimaryKey).ClrColumName.ShouldEqual("DataChildId");      
        }

        [Test]
        public void Test21DataChildRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataChild));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            CollectionAssert.AreEquivalent(new[] { "Parent" }, efInfo.RelationshipCols.Select(x => x.ClrColumnName));
            CollectionAssert.AreEquivalent(new[] { typeof(DataTop) }, efInfo.RelationshipCols.Select(x => x.ClrColumnType));
        }


        [Test]
        public void Test30DataManyChildrenNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataManyChildren));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataManyChildren");
            CollectionAssert.AreEquivalent(new[] { "DataManyChildrenId", "MyInt" }, efInfo.NormalCols.Select(x => x.ClrColumName));
            efInfo.NormalCols.Single(x => x.IsPrimaryKey).ClrColumName.ShouldEqual("DataManyChildrenId");
            
        }

        [Test]
        public void Test31DataManyChildrenRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataManyChildren));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            CollectionAssert.AreEquivalent(new[] { "ManyParents" }, efInfo.RelationshipCols.Select(x => x.ClrColumnName));
            CollectionAssert.AreEquivalent(new[] { typeof(ICollection<DataTop>) }, efInfo.RelationshipCols.Select(x => x.ClrColumnType));
        }

        [Test]
        public void Test40DataSingletonNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataSingleton));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataSingleton");
            CollectionAssert.AreEquivalent(new[] { "DataSingletonId", "MyDateTime", "NonStandardForeignKeyName" }, efInfo.NormalCols.Select(x => x.ClrColumName));
            efInfo.NormalCols.Single(x => x.IsPrimaryKey).ClrColumName.ShouldEqual("DataSingletonId");
        }

        [Test]
        public void Test41DataSingletonRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataSingleton));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            CollectionAssert.AreEquivalent(new[] { "Parent" }, efInfo.RelationshipCols.Select(x => x.ClrColumnName));
            CollectionAssert.AreEquivalent(new[] { typeof(DataTop) }, efInfo.RelationshipCols.Select(x => x.ClrColumnType));
        }

        [Test]
        public void Test50DataCompKeyNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataCompKey));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("NonStandardCompKeyTable");
            CollectionAssert.AreEquivalent(new[] { "Key1", "Key2", "MyEnum" }, efInfo.NormalCols.Select(x => x.ClrColumName));
            CollectionAssert.AreEquivalent(new[] { "Key1", "Key2" }, efInfo.NormalCols.Where(x => x.IsPrimaryKey).Select(x => x.ClrColumName));
            CollectionAssert.AreEquivalent(new[] { 1,2 }, efInfo.NormalCols.Where(x => x.IsPrimaryKey).Select(x => x.PrimaryKeyOrder));
        }

        [Test]
        public void Test51DataCompKeyRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataCompKey));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.RelationshipCols.Count().ShouldEqual(0);
        }

        [Test]
        public void Test60DataManyCompKeyNormalColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataManyCompKey));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataManyCompKey");
            CollectionAssert.AreEquivalent(new[] { "ManyKey1", "ManyKey2"}, efInfo.NormalCols.Select(x => x.ClrColumName));
            CollectionAssert.AreEquivalent(new[] { "ManyKey1", "ManyKey2" }, efInfo.NormalCols.Where(x => x.IsPrimaryKey).Select(x => x.ClrColumName));
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, efInfo.NormalCols.Where(x => x.IsPrimaryKey).Select(x => x.PrimaryKeyOrder));
        }

        [Test]
        public void Test61DataCompKeyRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataManyCompKey));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.RelationshipCols.Count().ShouldEqual(1);
            CollectionAssert.AreEquivalent(new[] { "ManyParents" }, efInfo.RelationshipCols.Select(x => x.ClrColumnName));
            CollectionAssert.AreEquivalent(new[] { typeof(ICollection<DataTop>) }, efInfo.RelationshipCols.Select(x => x.ClrColumnType));
        }

        [Test]
        public void Test80DataZeroOrOneColsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataZeroOrOne));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.TableName.ShouldEqual("DataZeroOrOne");
            efInfo.NormalCols.Count.ShouldEqual(2);
            //foreach (var col in efInfo.NormalCols)
            //{
            //    Console.WriteLine("list[i++].ToString().ShouldEqual(\"{0}\");", col);
            //}
            var list = efInfo.NormalCols.ToList();
            var i = 0;
            list[i++].ToString().ShouldEqual("SqlColumnName: DataTopId, SqlTypeName: int, ClrColumName: DataTopId, ClrColumnType: System.Int32, IsPrimaryKey: True, PrimaryKeyOrder: 1, IsNullable: False, MaxLength: -2");
            list[i++].ToString().ShouldEqual("SqlColumnName: MyBool, SqlTypeName: bit, ClrColumName: MyBool, ClrColumnType: System.Boolean, IsPrimaryKey: False, PrimaryKeyOrder: 0, IsNullable: False, MaxLength: -2");
            
        }

        [Test]
        public void Test81DataZeroOrOneRelationshipsOk()
        {
            //SETUP

            //EXECUTE
            var efInfo = _efInfos.SingleOrDefault(x => x.ClrClassType == typeof(DataZeroOrOne));

            //VERIFY
            efInfo.ShouldNotEqualNull();
            efInfo.RelationshipCols.Count().ShouldEqual(0);
        }
    }
}