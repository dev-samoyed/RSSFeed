using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeed.Data.Migrations
{
    public partial class ChannelsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Channels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Channels");
        }
    }
}
