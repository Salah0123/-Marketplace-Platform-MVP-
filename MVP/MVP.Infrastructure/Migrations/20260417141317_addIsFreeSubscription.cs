using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsFreeSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionType",
                table: "AspNetUsers",
                newName: "IsFreeSubscribtion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFreeSubscribtion",
                table: "AspNetUsers",
                newName: "SubscriptionType");
        }
    }
}
