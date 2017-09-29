using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UISWeb.Migrations
{
    public partial class UpdateCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFile_Customer_CustomerEmail",
                table: "CustomerFile");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFile_CustomerEmail",
                table: "CustomerFile");

            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "CustomerFile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "CustomerFile",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFile_CustomerEmail",
                table: "CustomerFile",
                column: "CustomerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFile_Customer_CustomerEmail",
                table: "CustomerFile",
                column: "CustomerEmail",
                principalTable: "Customer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
