﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCore.EntityFrameworkCore.Context;

namespace NetCore.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NetCore.EntityFrameworkCore.Models.StoreFiles", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<long>("FileBytes");

                    b.Property<string>("FileCategory")
                        .HasMaxLength(16);

                    b.Property<string>("FileExt")
                        .HasMaxLength(8);

                    b.Property<string>("FileName")
                        .HasMaxLength(128);

                    b.Property<string>("FileType")
                        .HasMaxLength(64);

                    b.Property<string>("RelationFilePath")
                        .HasMaxLength(128);

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("UpdateTime");

                    b.HasKey("ID");

                    b.ToTable("StoreFiles");
                });

            modelBuilder.Entity("NetCore.EntityFrameworkCore.Models.TaskJob", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiUrl")
                        .HasMaxLength(256);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CronExpression")
                        .HasMaxLength(512);

                    b.Property<string>("CronExpressionDescription")
                        .HasMaxLength(512);

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("EndRunTime");

                    b.Property<string>("ExceptionMsg");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("RequestHead")
                        .HasMaxLength(1024);

                    b.Property<string>("RequestParams");

                    b.Property<string>("RequestType")
                        .HasMaxLength(8);

                    b.Property<int>("RunCount");

                    b.Property<DateTime?>("StartRunTime");

                    b.Property<string>("TaskGroup")
                        .HasMaxLength(64);

                    b.Property<string>("TaskName")
                        .HasMaxLength(64);

                    b.Property<string>("TaskState")
                        .HasMaxLength(8);

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("UpdateTime");

                    b.HasKey("ID");

                    b.ToTable("TaskJob");
                });

            modelBuilder.Entity("NetCore.EntityFrameworkCore.Models.Test", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("UpdateTime");

                    b.HasKey("ID");

                    b.ToTable("Test");
                });
#pragma warning restore 612, 618
        }
    }
}
