using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MatchTechWebsite.sakila;

public partial class DbAa1c85MtechContext : DbContext
{
    public DbAa1c85MtechContext()
    {
    }

    public DbAa1c85MtechContext(DbContextOptions<DbAa1c85MtechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<ClubsField> ClubsFields { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<Matchdayfixture> Matchdayfixtures { get; set; }

    public virtual DbSet<MatchweekMatchday> MatchweekMatchdays { get; set; }

    public virtual DbSet<Matchweekfixture> Matchweekfixtures { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseMySQL("Server=MYSQL5048.site4now.net;Database=db_aa1c85_mtech;Uid=aa1c85_mtech;Pwd=Rockstar03;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clubs");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ClubsField>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("clubs_fields");

            entity.HasIndex(e => e.ClubKey, "ClubHasField_Key");

            entity.HasIndex(e => e.FieldKey, "FieldBelongsClub_Key");

            entity.Property(e => e.ClubKey).HasColumnName("clubKey");
            entity.Property(e => e.FieldKey).HasColumnName("fieldKey");

            entity.HasOne(d => d.ClubKeyNavigation).WithMany()
                .HasForeignKey(d => d.ClubKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ClubHasField_Key");

            entity.HasOne(d => d.FieldKeyNavigation).WithMany()
                .HasForeignKey(d => d.FieldKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FieldBelongsClub_Key");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fields");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<Matchdayfixture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("matchdayfixtures");

            entity.HasIndex(e => e.AwayTeamKey, "AwayTeam_Key");

            entity.HasIndex(e => e.FieldKey, "Field_Key");

            entity.HasIndex(e => e.HomeTeamKey, "HomeTeam_Key");

            entity.Property(e => e.DateTime).HasColumnType("datetime");

            entity.HasOne(d => d.AwayTeamKeyNavigation).WithMany(p => p.MatchdayfixtureAwayTeamKeyNavigations)
                .HasForeignKey(d => d.AwayTeamKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("AwayTeam_Key");

            entity.HasOne(d => d.FieldKeyNavigation).WithMany(p => p.Matchdayfixtures)
                .HasForeignKey(d => d.FieldKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Field_Key");

            entity.HasOne(d => d.HomeTeamKeyNavigation).WithMany(p => p.MatchdayfixtureHomeTeamKeyNavigations)
                .HasForeignKey(d => d.HomeTeamKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("HomeTeam_Key");
        });

        modelBuilder.Entity<MatchweekMatchday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity
                //.HasNoKey()
                .ToTable("matchweek_matchday");
            

            entity.HasIndex(e => e.MatchDayKey, "MatchDay_Key");

            entity.HasIndex(e => e.MatchWeekKey, "MatchWeek_Key");

            entity.Property(e => e.MatchDayKey).HasColumnName("matchDayKey");
            entity.Property(e => e.MatchWeekKey).HasColumnName("matchWeekKey");

            entity.HasOne(d => d.MatchDayKeyNavigation).WithMany()
                .HasForeignKey(d => d.MatchDayKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("MatchDay_Key");

            entity.HasOne(d => d.MatchWeekKeyNavigation).WithMany()
                .HasForeignKey(d => d.MatchWeekKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("MatchWeek_Key");
        });

        modelBuilder.Entity<Matchweekfixture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("matchweekfixtures");

            entity.HasIndex(e => e.TournamentKey, "tournament_key");

            entity.HasOne(d => d.TournamentKeyNavigation).WithMany(p => p.Matchweekfixtures)
                .HasForeignKey(d => d.TournamentKey)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tournament_key");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("teams");

            entity.HasIndex(e => e.ClubId, "TeamBelongsClub_Key");

            entity.Property(e => e.ClubId).HasColumnName("Club_Id");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Club).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("TeamBelongsClub_Key");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tournament");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
