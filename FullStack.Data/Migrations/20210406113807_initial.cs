using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStack.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminRole = table.Column<bool>(type: "bit", nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AdminRole", "Email", "FirstName", "LastName", "Locked", "Password", "PhoneNumber" },
                values: new object[,]
                {
                    { 2, true, "properproperties1@gmail.com", "John", "Smith", false, "ppAdmin1", null },
                    { 3, true, "properproperties2@gmail.com", "Johan", "Smit", false, "ppAdmin2", null },
                    { 1, false, "regardtvisagie@gmail.com", "Regardt", "Visagie", false, "Reg14061465", null },
                    { 4, false, "mk@yahoo.com", "Michelle", "Koorts", false, "Koorts123", null },
                    { 5, false, "pieterj@yhotmail.com", "Pieter", "Joubert", false, "Jouba1987", null },
                    { 6, false, "cs@ymail.com", "Chulu", "Sibuzo", false, "Chulu1982", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
