using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeed.Data.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("050e2ffd-2e2c-4dce-a649-7986b0c12903"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("136544ad-fb83-4259-b365-c4428a00cab6"));

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChannelType",
                table: "Channels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ChannelType",
                table: "Channels");

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("050e2ffd-2e2c-4dce-a649-7986b0c12903"), "Interfax", "http://www.interfax.by/news/feed" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("136544ad-fb83-4259-b365-c4428a00cab6"), "Habr", "http://habrahabr.ru/rss/" });
        }
    }
}
