﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmitairDOTNET.DAL;
using System;

namespace Smitair3.Migrations.SmitairDb
{
    [DbContext(typeof(SmitairDbContext))]
    [Migration("20180103090529_SmiMig")]
    partial class SmiMig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Smitair3.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AvatarLink");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("SmitairDOTNET.Models.Comment", b =>
                {
                    b.Property<int>("CommentsID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorID");

                    b.Property<int>("EffectID");

                    b.Property<string>("Text");

                    b.HasKey("CommentsID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SmitairDOTNET.Models.Effect", b =>
                {
                    b.Property<Guid>("EffectID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorID");

                    b.Property<double>("AvgGrade");

                    b.Property<int>("CountGrade");

                    b.Property<string>("Description");

                    b.Property<string>("EffectLink");

                    b.Property<string>("EffectName");

                    b.Property<string>("UserId");

                    b.Property<string>("YoutubeLink");

                    b.HasKey("EffectID");

                    b.HasIndex("UserId");

                    b.ToTable("Effects");
                });

            modelBuilder.Entity("SmitairDOTNET.Models.Purchase", b =>
                {
                    b.Property<Guid>("PurchaseID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EffectId");

                    b.Property<int>("Grade");

                    b.Property<string>("Id");

                    b.HasKey("PurchaseID");

                    b.HasIndex("EffectId");

                    b.HasIndex("Id");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("SmitairDOTNET.Models.Effect", b =>
                {
                    b.HasOne("Smitair3.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SmitairDOTNET.Models.Purchase", b =>
                {
                    b.HasOne("SmitairDOTNET.Models.Effect", "Effect")
                        .WithMany()
                        .HasForeignKey("EffectId");

                    b.HasOne("Smitair3.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("Id");
                });
#pragma warning restore 612, 618
        }
    }
}
