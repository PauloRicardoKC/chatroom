﻿// <auto-generated />
using System;
using Chat.Infra.Data.DataBases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chat.Infra.Data.Databases.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220423174611_CreateTableChatMessage")]
    partial class CreateTableChatMessage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Chat.Domain.Entities.ChatMessage", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MessageId")
                        .HasComment("Unique MessageId identifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("Message")
                        .HasComment("Message of the ChatMessage");

                    b.Property<string>("SenderUserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("SenderUserId")
                        .HasComment("SenderUserId of the ChatMessage");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("SentDate")
                        .HasComment("Sent Date of the ChatMessage");

                    b.HasKey("MessageId")
                        .HasName("PK_ChatMessage_MessageId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("MessageId"), false);

                    b.ToTable("ChatMessage", (string)null);

                    b.HasComment("Chat message manager");
                });
#pragma warning restore 612, 618
        }
    }
}
