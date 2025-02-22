﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkiServerBackend.Data;

#nullable disable

namespace SkiServerBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ServiceAuftrag", b =>
                {
                    b.Property<int>("AuftragID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AuftragID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuftragID"));

                    b.Property<int>("DienstleistungID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Erstellungsdatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("KundeId")
                        .HasColumnType("int");

                    b.Property<int>("MitarbeiterID")
                        .HasColumnType("int");

                    b.Property<int?>("Priorität")
                        .HasColumnType("int");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.HasKey("AuftragID");

                    b.HasIndex("DienstleistungID");

                    b.HasIndex("KundeId");

                    b.ToTable("ServiceAuftraege");
                });

            modelBuilder.Entity("SkiServerBackend.Models.Dienstleistung", b =>
                {
                    b.Property<int>("DienstleistungId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DienstleistungId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Preis")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DienstleistungId");

                    b.ToTable("Dienstleistung");
                });

            modelBuilder.Entity("SkiServerBackend.Models.Kunde", b =>
                {
                    b.Property<int>("KundeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KundeId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KundeId");

                    b.ToTable("Kunden");
                });

            modelBuilder.Entity("SkiServerBackend.Models.Mitarbeiter", b =>
                {
                    b.Property<int>("MitarbeiterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MitarbeiterID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswortHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MitarbeiterID");

                    b.ToTable("Mitarbeiter");
                });

            modelBuilder.Entity("ServiceAuftrag", b =>
                {
                    b.HasOne("SkiServerBackend.Models.Dienstleistung", "Dienstleistung")
                        .WithMany()
                        .HasForeignKey("DienstleistungID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkiServerBackend.Models.Kunde", "Kunde")
                        .WithMany("ServiceAuftraege")
                        .HasForeignKey("KundeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dienstleistung");

                    b.Navigation("Kunde");
                });

            modelBuilder.Entity("SkiServerBackend.Models.Kunde", b =>
                {
                    b.Navigation("ServiceAuftraege");
                });
#pragma warning restore 612, 618
        }
    }
}
