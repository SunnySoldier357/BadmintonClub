﻿// <auto-generated />
using System;
using Backend.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(BadmintonClubDBDataContext))]
    partial class BadmintonClubDBDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Models.DB.BlogPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("DatePublished");

                    b.Property<Guid>("PublisherId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("Backend.Models.DB.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("OpponentId");

                    b.Property<int>("OpponentScore");

                    b.Property<Guid>("PlayerId");

                    b.Property<int>("PlayerScore");

                    b.Property<Guid?>("SeasonId");

                    b.Property<Guid>("UserId");

                    b.Property<Guid?>("UserId1");

                    b.HasKey("Id");

                    b.HasIndex("OpponentId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Backend.Models.DB.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<int>("SeasonNumber");

                    b.Property<Guid>("StatisticsId");

                    b.HasKey("Id");

                    b.HasIndex("StatisticsId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Backend.Models.DB.Statistics", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<int>("GamesDrawn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("GamesLost")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("GamesWon")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("PointsAgainst")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("PointsFor")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("Backend.Models.DB.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<int>("ClearanceLevel");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<Guid>("SeasonId");

                    b.Property<Guid>("StatisticsId");

                    b.Property<string>("Title")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("Member");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId")
                        .IsUnique();

                    b.HasIndex("StatisticsId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Models.DB.BlogPost", b =>
                {
                    b.HasOne("Backend.Models.DB.User", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend.Models.DB.Match", b =>
                {
                    b.HasOne("Backend.Models.DB.User", "Opponent")
                        .WithMany()
                        .HasForeignKey("OpponentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.DB.User", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.DB.Season")
                        .WithMany("Matches")
                        .HasForeignKey("SeasonId");

                    b.HasOne("Backend.Models.DB.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.DB.User")
                        .WithMany("Matches")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.DB.Season", b =>
                {
                    b.HasOne("Backend.Models.DB.Statistics", "Statistics")
                        .WithMany()
                        .HasForeignKey("StatisticsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend.Models.DB.User", b =>
                {
                    b.HasOne("Backend.Models.DB.Season", "Season")
                        .WithOne("User")
                        .HasForeignKey("Backend.Models.DB.User", "SeasonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Backend.Models.DB.Statistics", "Statistics")
                        .WithMany()
                        .HasForeignKey("StatisticsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}