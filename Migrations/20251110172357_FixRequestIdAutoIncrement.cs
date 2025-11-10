using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yinyang_Api.Migrations
{
    /// <inheritdoc />
    public partial class FixRequestIdAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_RequestId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Requests",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "RequestId",
                table: "Requests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_RequesterId",
                table: "Requests",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_RequesterId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Requests",
                newName: "status");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RequestId",
                table: "Requests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Requests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestId",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_RequestId",
                table: "Requests",
                column: "RequestId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
