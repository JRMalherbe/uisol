using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UIS.Migrations
{
    public partial class boot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    ContactName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRequest",
                columns: table => new
                {
                    LabNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRequest", x => x.LabNo);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 80, nullable: true),
                    Role = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFile",
                columns: table => new
                {
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    CustomerRequestLabNo = table.Column<int>(nullable: true),
                    LinkName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFile", x => x.FileName);
                    table.ForeignKey(
                        name: "FK_CustomerFile_CustomerRequest_CustomerRequestLabNo",
                        column: x => x.CustomerRequestLabNo,
                        principalTable: "CustomerRequest",
                        principalColumn: "LabNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFile_CustomerRequestLabNo",
                table: "CustomerFile",
                column: "CustomerRequestLabNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "CustomerFile");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "CustomerRequest");
        }
    }
}
