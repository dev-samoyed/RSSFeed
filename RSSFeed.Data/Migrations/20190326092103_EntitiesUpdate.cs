using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeed.Data.Migrations
{
    public partial class EntitiesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_PostId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ChannelId",
                table: "Categories",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_PostId",
                table: "Categories",
                column: "PostId",
                unique: true,
                filter: "[PostId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ChannelId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_PostId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_PostId",
                table: "Categories",
                column: "PostId");
        }
    }
}
