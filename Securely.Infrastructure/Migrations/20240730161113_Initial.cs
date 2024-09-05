using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Securely.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EncryptionKey",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EncryptionKey", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SecureMessageHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SecureMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EncryptionKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Recipient = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Action = table.Column<int>(type: "int", nullable: false),
                DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SecureMessageHistory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SecureMessage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EncryptionKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Message = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Recipient = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ExpirationUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "DATEADD(day, 1, GETDATE())"),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SecureMessage", x => x.Id);
                table.ForeignKey(
                    name: "FK_SecureMessage_EncryptionKey_EncryptionKeyId",
                    column: x => x.EncryptionKeyId,
                    principalTable: "EncryptionKey",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SecureMessage_EncryptionKeyId",
            table: "SecureMessage",
            column: "EncryptionKeyId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SecureMessage");

        migrationBuilder.DropTable(
            name: "SecureMessageHistory");

        migrationBuilder.DropTable(
            name: "EncryptionKey");
    }
}
