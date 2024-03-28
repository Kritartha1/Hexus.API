using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hexus.Migrations
{
    /// <inheritdoc />
    public partial class _2ndmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DM_AspNetUsers_ReceiverId",
                table: "DM");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DM",
                table: "DM");

            migrationBuilder.DropIndex(
                name: "IX_DM_ReceiverId",
                table: "DM");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "DM");

            migrationBuilder.RenameTable(
                name: "DM",
                newName: "DMs");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "DMs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DMs",
                table: "DMs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DMId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_DMs_DMId",
                        column: x => x.DMId,
                        principalTable: "DMs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_DMId",
                table: "Friends",
                column: "DMId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DMs",
                table: "DMs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "DMs");

            migrationBuilder.RenameTable(
                name: "DMs",
                newName: "DM");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "DM",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DM",
                table: "DM",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DM_ReceiverId",
                table: "DM",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_AspNetUsers_ReceiverId",
                table: "DM",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
