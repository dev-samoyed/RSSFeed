﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RSSFeed.Data;

namespace RSSFeed.Data.Migrations
{
    [DbContext(typeof(RSSContext))]
    [Migration("20190308045350_AddedIsNewField")]
    partial class AddedIsNewField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RSSFeed.Data.Entities.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Channels");

                    b.HasData(
                        new { Id = new Guid("050e2ffd-2e2c-4dce-a649-7986b0c12903"), Title = "Interfax", Url = "http://www.interfax.by/news/feed" },
                        new { Id = new Guid("136544ad-fb83-4259-b365-c4428a00cab6"), Title = "Habr", Url = "http://habrahabr.ru/rss/" }
                    );
                });

            modelBuilder.Entity("RSSFeed.Data.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<Guid?>("ChannelId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsNew");

                    b.Property<bool>("IsSeen");

                    b.Property<string>("PostUrl");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("RSSFeed.Data.Entities.Post", b =>
                {
                    b.HasOne("RSSFeed.Data.Entities.Channel", "Channel")
                        .WithMany("Posts")
                        .HasForeignKey("ChannelId");
                });
#pragma warning restore 612, 618
        }
    }
}