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
    [Migration("20190326110315_AddedIndex")]
    partial class AddedIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RSSFeed.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ChannelId");

                    b.Property<string>("Name");

                    b.Property<Guid?>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("PostId")
                        .IsUnique()
                        .HasFilter("[PostId] IS NOT NULL");

                    b.HasIndex("Name", "ChannelId")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL AND [ChannelId] IS NOT NULL");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RSSFeed.Data.Entities.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelType");

                    b.Property<string>("Image");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("RSSFeed.Data.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<Guid?>("ChannelId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsNew");

                    b.Property<bool>("IsSeen");

                    b.Property<string>("PostUrl");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("RSSFeed.Data.Entities.Category", b =>
                {
                    b.HasOne("RSSFeed.Data.Entities.Channel", "Channel")
                        .WithMany("Categories")
                        .HasForeignKey("ChannelId");

                    b.HasOne("RSSFeed.Data.Entities.Post", "Post")
                        .WithOne("Category")
                        .HasForeignKey("RSSFeed.Data.Entities.Category", "PostId");
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
