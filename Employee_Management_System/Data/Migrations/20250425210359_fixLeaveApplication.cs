using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixLeaveApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_Employes_EmployeId",
                table: "LeaveApplications");

            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_EmployeId",
                table: "LeaveApplications");

            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "LeaveApplications");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_EmployeeId",
                table: "LeaveApplications",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_Employes_EmployeeId",
                table: "LeaveApplications",
                column: "EmployeeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_Employes_EmployeeId",
                table: "LeaveApplications");

            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_EmployeeId",
                table: "LeaveApplications");

            migrationBuilder.AddColumn<int>(
                name: "EmployeId",
                table: "LeaveApplications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_EmployeId",
                table: "LeaveApplications",
                column: "EmployeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_Employes_EmployeId",
                table: "LeaveApplications",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id");
        }
    }
}
