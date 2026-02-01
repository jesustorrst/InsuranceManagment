using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class LlavesForaneasAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualizadoPor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroIdentificacion",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ApellidoMaterno",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polizas_ActualizadoPor",
                table: "Polizas",
                column: "ActualizadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Polizas_CreadoPor",
                table: "Polizas",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ActualizadoPor",
                table: "Clientes",
                column: "ActualizadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CreadoPor",
                table: "Clientes",
                column: "CreadoPor");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Usuarios_ActualizadoPor",
                table: "Clientes",
                column: "ActualizadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Usuarios_CreadoPor",
                table: "Clientes",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Polizas_Usuarios_ActualizadoPor",
                table: "Polizas",
                column: "ActualizadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Polizas_Usuarios_CreadoPor",
                table: "Polizas",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Usuarios_ActualizadoPor",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Usuarios_CreadoPor",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Polizas_Usuarios_ActualizadoPor",
                table: "Polizas");

            migrationBuilder.DropForeignKey(
                name: "FK_Polizas_Usuarios_CreadoPor",
                table: "Polizas");

            migrationBuilder.DropIndex(
                name: "IX_Polizas_ActualizadoPor",
                table: "Polizas");

            migrationBuilder.DropIndex(
                name: "IX_Polizas_CreadoPor",
                table: "Polizas");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_ActualizadoPor",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_CreadoPor",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "ActualizadoPor",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "NumeroIdentificacion",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApellidoMaterno",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
