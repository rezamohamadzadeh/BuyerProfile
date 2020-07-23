using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations.BaseDb
{
    public partial class AddRankToSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Sells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliateCode = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    Adress2 = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    full_number = table.Column<string>(nullable: true),
                    FromUrl = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    TransActionId = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    DiliveryStatus = table.Column<int>(nullable: false),
                    PayStatus = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Sells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_SupportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportTypeName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_SupportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Supports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    SendDateTime = table.Column<DateTime>(nullable: false),
                    SenderUserId = table.Column<string>(nullable: true),
                    AnswerMessage = table.Column<string>(nullable: true),
                    AnswerDateTime = table.Column<DateTime>(nullable: true),
                    AnswerUserId = table.Column<string>(nullable: true),
                    SupportPosition = table.Column<int>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Supports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Supports_Tb_SupportTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Tb_SupportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Supports_TypeId",
                table: "Tb_Supports",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Sells");

            migrationBuilder.DropTable(
                name: "Tb_Supports");

            migrationBuilder.DropTable(
                name: "Tb_SupportTypes");
        }
    }
}
