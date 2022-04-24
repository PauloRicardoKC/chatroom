using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Chat.Infra.Data.DataBases
{
    [ExcludeFromCodeCoverage]
    public class ChatMessageMapping : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("ChatMessage")
                .HasComment("Chat message manager");

            builder.HasKey(c => c.MessageId)
                .HasName("PK_ChatMessage_MessageId")
                .IsClustered(false);

            builder.Property(c => c.MessageId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MessageId")
                .HasComment("Unique MessageId identifier");

            builder.Property(statement => statement.SenderUserId)
               .HasColumnName("SenderUserId")
               .HasColumnType("varchar(50)")
               .IsRequired()
               .HasComment("SenderUserId of the ChatMessage");

            builder.Property(statement => statement.Message)
               .HasColumnName("Message")
               .HasColumnType("varchar(500)")
               .IsRequired()
               .HasComment("Message of the ChatMessage");

            builder.Property(statement => statement.SentDate)
               .HasColumnName("SentDate")
               .HasColumnType("datetime")
               .HasComment("Sent Date of the ChatMessage");            
        }
    }
}