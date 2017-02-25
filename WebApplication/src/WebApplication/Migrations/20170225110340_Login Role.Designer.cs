using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UIS;

namespace UIS.Migrations
{
    [DbContext(typeof(UISContext))]
    [Migration("20170225110340_Login Role")]
    partial class LoginRole
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UIS.Client", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255);

                    b.Property<int>("ClientId");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(255);

                    b.Property<string>("ContactName")
                        .HasMaxLength(255);

                    b.HasKey("Email");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("UIS.Login", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasMaxLength(80);

                    b.Property<string>("Role")
                        .HasMaxLength(10);

                    b.HasKey("Email");

                    b.ToTable("Login");
                });
        }
    }
}
