﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Snap.HutaoAPI.Entities;

#nullable disable

namespace Snap.HutaoAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Snap.HutaoAPI.Entities.ItemInfo", b =>
                {
                    b.Property<int>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("InnerId");

                    b.ToTable("GenshinItems");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Player", b =>
                {
                    b.Property<Guid>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("InnerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedAvatarInfo", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("ActivedConstellationNum")
                        .HasColumnType("int");

                    b.Property<int>("AffixLevel")
                        .HasColumnType("int");

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<int>("AvatarLevel")
                        .HasColumnType("int");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("char(36)");

                    b.Property<int>("WeaponId")
                        .HasColumnType("int");

                    b.Property<int>("WeaponLevel")
                        .HasColumnType("int");

                    b.HasKey("InnerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("AvatarDetails");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedBattleInfo", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("BattleIndex")
                        .HasColumnType("int");

                    b.Property<long>("SpiralAbyssLevelId")
                        .HasColumnType("bigint");

                    b.HasKey("InnerId");

                    b.HasIndex("SpiralAbyssLevelId");

                    b.ToTable("SpiralAbyssBattles");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedLevelInfo", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("FloorIndex")
                        .HasColumnType("int");

                    b.Property<int>("LevelIndex")
                        .HasColumnType("int");

                    b.Property<long>("RecordId")
                        .HasColumnType("bigint");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.HasKey("InnerId");

                    b.HasIndex("RecordId");

                    b.ToTable("SpiralAbyssLevels");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedRankInfo", b =>
                {
                    b.Property<Guid>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("InnerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Ranks");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedRecordInfo", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UploadTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.HasKey("InnerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerRecords");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedReliquarySetInfo", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AvatarDetailId")
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("UnionId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("InnerId");

                    b.HasIndex("AvatarDetailId");

                    b.ToTable("ReliquarySetDetails");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.SpiralAbyssAvatar", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<long>("SpiralAbyssBattleId")
                        .HasColumnType("bigint");

                    b.HasKey("InnerId");

                    b.HasIndex("SpiralAbyssBattleId");

                    b.ToTable("SpiralAbyssAvatars");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Statistics", b =>
                {
                    b.Property<long>("InnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("Period")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("InnerId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedAvatarInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Player", "Player")
                        .WithMany("Avatars")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedBattleInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Record.DetailedLevelInfo", "AbyssLevel")
                        .WithMany("Battles")
                        .HasForeignKey("SpiralAbyssLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AbyssLevel");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedLevelInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Record.DetailedRecordInfo", "Record")
                        .WithMany("SpiralAbyssLevels")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedRankInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedRecordInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedReliquarySetInfo", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Record.DetailedAvatarInfo", "AvatarInfo")
                        .WithMany("ReliquarySets")
                        .HasForeignKey("AvatarDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AvatarInfo");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.SpiralAbyssAvatar", b =>
                {
                    b.HasOne("Snap.HutaoAPI.Entities.Record.DetailedBattleInfo", "SpiralAbyssBattle")
                        .WithMany("Avatars")
                        .HasForeignKey("SpiralAbyssBattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpiralAbyssBattle");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Player", b =>
                {
                    b.Navigation("Avatars");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedAvatarInfo", b =>
                {
                    b.Navigation("ReliquarySets");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedBattleInfo", b =>
                {
                    b.Navigation("Avatars");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedLevelInfo", b =>
                {
                    b.Navigation("Battles");
                });

            modelBuilder.Entity("Snap.HutaoAPI.Entities.Record.DetailedRecordInfo", b =>
                {
                    b.Navigation("SpiralAbyssLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
