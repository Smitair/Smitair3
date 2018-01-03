using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Smitair3.Migrations.SmitairDb
{
    public partial class SmiMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_ApplicationUser_UserId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Purchases",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                newName: "IX_Purchases_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_ApplicationUser_Id",
                table: "Purchases",
                column: "Id",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_ApplicationUser_Id",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Purchases",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_Id",
                table: "Purchases",
                newName: "IX_Purchases_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_ApplicationUser_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
