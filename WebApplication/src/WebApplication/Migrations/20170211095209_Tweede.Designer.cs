using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UIS;

namespace UIS.Migrations
{
    [DbContext(typeof(UISContext))]
    [Migration("20170211095209_Tweede")]
    partial class Tweede
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UIS.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .HasMaxLength(255);

                    b.Property<string>("ContactName")
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("UIS.Login", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("Password");

                    b.HasKey("Email");

                    b.ToTable("Login");
                });
        }
    }
}
