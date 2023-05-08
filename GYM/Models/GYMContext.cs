using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GYM.Models
{
    public partial class GYMContext : DbContext
    {
        public GYMContext()
        {
        }

        public GYMContext(DbContextOptions<GYMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassRecord> ClassRecords { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;

        //配合專屬某複雜SQL query的特殊models類別檔
        public virtual DbSet<SearchCashInMonth> SearchCashInMonths { get; set; } = null!;

        public virtual DbSet<SearchRevenueInMonth> SearchRevenueInMonths { get; set; } = null!;

        public virtual DbSet<SearchRemainClass> SearchRemainClasses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=GYM;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassRecord>(entity =>
            {
                entity.ToTable("classRecord");

                entity.Property(e => e.ClassRecordId).HasColumnName("classRecordId");

                entity.Property(e => e.ClassDate)
                    .HasColumnType("date")
                    .HasColumnName("classDate");

                entity.Property(e => e.LessonId).HasColumnName("lesson_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.ClassRecords)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_classRecord_lesson");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ClassRecords)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_classRecord_member");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("lesson");

                entity.Property(e => e.LessonId).HasColumnName("lesson_id");

                entity.Property(e => e.LessonName)
                    .HasMaxLength(50)
                    .HasColumnName("lesson_name");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasColumnName("unit_price");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(50)
                    .HasColumnName("member_name");

                entity.Property(e => e.MemberPassword)
                    .HasMaxLength(50)
                    .HasColumnName("member_password");

                entity.Property(e => e.MemberRank).HasColumnName("member_rank");

                entity.Property(e => e.MemberApproved)
                    .HasMaxLength(10)
                    .HasColumnName("member_approved");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.LessonId).HasColumnName("lesson_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("money")
                    .HasColumnName("paymentAmount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("paymentDate");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payment_lesson");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payment_member");
            });


            //modelBuilder.Entity<SearchRemainClass>(entity =>
            //{
                

                //entity.Property(e => e.lesson_id).HasColumnName("lesson_id");

                //entity.Property(e => e.member_id).HasColumnName("member_id");

                //entity.HasOne(d => d.Lesson)
                //    .WithMany(p => p.SearchRemainClasses);
                //.HasForeignKey(d => d.LessonId)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_classRecord_lesson");

                //entity.HasOne(d => d.Member)
                //    .WithMany(p => p.SearchRemainClasses);
                //.HasForeignKey(d => d.MemberId)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_classRecord_member");
            //});

            //避免寫回database
            modelBuilder.Entity<SearchCashInMonth>(b=>b.ToView("sql_v1"));

            modelBuilder.Entity<SearchRevenueInMonth>(b => b.ToView("sql_v2"));

            modelBuilder.Entity<SearchRemainClass>(b => b.ToView("sql_v3"));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
