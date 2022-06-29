using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gRPCServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WeatherStation",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStation", x => x.StationId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WeatherDataPoint",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AvgWindSpeed = table.Column<int>(type: "int", nullable: false),
                    Barometer = table.Column<int>(type: "int", nullable: false),
                    BattLevel = table.Column<int>(type: "int", nullable: false),
                    InsideHum = table.Column<int>(type: "int", nullable: false),
                    InsideTemp = table.Column<int>(type: "int", nullable: false),
                    OutsideHum = table.Column<int>(type: "int", nullable: false),
                    OutsideTemp = table.Column<int>(type: "int", nullable: false),
                    RainRate = table.Column<int>(type: "int", nullable: false),
                    SolarRad = table.Column<int>(type: "int", nullable: false),
                    StationTempId = table.Column<int>(type: "int", nullable: false),
                    Sunrise = table.Column<int>(type: "int", nullable: false),
                    Sunset = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ts = table.Column<int>(type: "int", nullable: false),
                    UvLevel = table.Column<int>(type: "int", nullable: false),
                    WindSpeed = table.Column<int>(type: "int", nullable: false),
                    WindDir = table.Column<int>(type: "int", nullable: false),
                    XmitBatt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherDataPoint", x => x.StationId);
                    table.ForeignKey(
                        name: "FK_WeatherDataPoint_WeatherStation_StationTempId",
                        column: x => x.StationTempId,
                        principalTable: "WeatherStation",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherDataPoint_StationTempId",
                table: "WeatherDataPoint",
                column: "StationTempId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherDataPoint");

            migrationBuilder.DropTable(
                name: "WeatherStation");
        }
    }
}
