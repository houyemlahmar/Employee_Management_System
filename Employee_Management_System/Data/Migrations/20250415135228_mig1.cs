using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SalaryBonuses_SalaryBonusId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryBonusId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "EmployeeMonthlyLeaveDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LeaveDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMonthlyLeaveDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeMonthlyLeaveDays_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeMonthlyLeaveDays_EmployeeId",
                table: "EmployeeMonthlyLeaveDays",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SalaryBonuses_SalaryBonusId",
                table: "Employees",
                column: "SalaryBonusId",
                principalTable: "SalaryBonuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SalaryBonuses_SalaryBonusId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeMonthlyLeaveDays");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryBonusId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SalaryBonuses_SalaryBonusId",
                table: "Employees",
                column: "SalaryBonusId",
                principalTable: "SalaryBonuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
