﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication9.Migrations
{
    public partial class WebApplication9DBPointOfInterestDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
              name: "Description",
              table: "PointsOfInterest",
              type: "TEXT",
              maxLength: 200,
              nullable: false,
              defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Description",
             table: "PointsOfInterest");

        }
    }
}
