using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeed.Data.Migrations
{
    public partial class AddedIsNewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("0cfbf39b-d94f-47c4-b635-a86f3e774e9b"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("c59046ae-1f1c-4c2f-8d03-bcd094465c76"));

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("050e2ffd-2e2c-4dce-a649-7986b0c12903"), "Interfax", "http://www.interfax.by/news/feed" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("136544ad-fb83-4259-b365-c4428a00cab6"), "Habr", "http://habrahabr.ru/rss/" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("050e2ffd-2e2c-4dce-a649-7986b0c12903"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("136544ad-fb83-4259-b365-c4428a00cab6"));

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Posts");

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("0cfbf39b-d94f-47c4-b635-a86f3e774e9b"), "Interfax", "http://www.interfax.by/news/feed" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("c59046ae-1f1c-4c2f-8d03-bcd094465c76"), "Habr", "http://habrahabr.ru/rss/" });
        }
    }
}
