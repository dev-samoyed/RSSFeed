using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeed.Data.Migrations
{
    public partial class AddedPostUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostUrl",
                table: "Posts",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("0cfbf39b-d94f-47c4-b635-a86f3e774e9b"), "Interfax", "http://www.interfax.by/news/feed" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { new Guid("c59046ae-1f1c-4c2f-8d03-bcd094465c76"), "Habr", "http://habrahabr.ru/rss/" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("0cfbf39b-d94f-47c4-b635-a86f3e774e9b"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("c59046ae-1f1c-4c2f-8d03-bcd094465c76"));

            migrationBuilder.DropColumn(
                name: "PostUrl",
                table: "Posts");
        }
    }
}
