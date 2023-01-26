﻿using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace NovaStream.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Drama"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Crime"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Historical"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Adventure"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Science fiction"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Detective"
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Episode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Episodes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Baza",
                            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988",
                            Name = "Episode 1",
                            Number = 1,
                            SeasonId = 1,
                            VideoUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E01.mp4?alt=media&token=728dbcd3-6215-4e76-ae69-6849ba9896f5"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Baza",
                            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988",
                            Name = "Episode 2",
                            Number = 2,
                            SeasonId = 1,
                            VideoUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E02.mp4?alt=media&token=f2bcb626-2a22-4a69-bdb8-69fef93ca2c9"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Baza",
                            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988",
                            Name = "Episode 1",
                            Number = 1,
                            SeasonId = 2,
                            VideoUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%202%2FPeaky-Blinders-S02E01.mp4?alt=media&token=740e66cd-9759-4b7b-8592-8a93919a059b"
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Movie", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrailerUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Name");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Name = "Interstellar",
                            Age = 13,
                            Description = "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.",
                            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FInterstellar%2FInterstellar.png?alt=media&token=0a6c45f5-fc92-4b66-8d50-7222ae8815b8",
                            TrailerUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar%20Trailer.mp4?alt=media&token=562d9a36-a3b2-411b-9525-05ccdc65e11a",
                            VideoUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar.mp4?alt=media&token=0fbe924c-8746-4132-8440-f8331d5214f6",
                            Year = 2014
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.MovieCategory", b =>
                {
                    b.Property<string>("MovieName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("MovieName", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("MovieCategories");

                    b.HasData(
                        new
                        {
                            MovieName = "Interstellar",
                            CategoryId = 1
                        },
                        new
                        {
                            MovieName = "Interstellar",
                            CategoryId = 4
                        },
                        new
                        {
                            MovieName = "Interstellar",
                            CategoryId = 5
                        },
                        new
                        {
                            MovieName = "Interstellar",
                            CategoryId = 6
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.MovieMark", b =>
                {
                    b.Property<string>("MovieName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieName", "UserEmail");

                    b.HasIndex("UserEmail");

                    b.ToTable("MovieMarks");

                    b.HasData(
                        new
                        {
                            MovieName = "Interstellar",
                            UserEmail = "admin@novastream.api"
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("SerialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SerialName");

                    b.ToTable("Seasons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Number = 1,
                            SerialName = "Peaky Blinders"
                        },
                        new
                        {
                            Id = 2,
                            Number = 2,
                            SerialName = "Peaky Blinders"
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Serial", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrailerUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Name");

                    b.ToTable("Serials");

                    b.HasData(
                        new
                        {
                            Name = "Peaky Blinders",
                            Age = 18,
                            Description = "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.",
                            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988",
                            TrailerUrl = "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky%20Blinder%20Season%201%20Trailer.mp4?alt=media&token=ce9640c0-b5ea-4f6f-9d1d-11b5a518ab32",
                            Year = 2013
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.SerialCategory", b =>
                {
                    b.Property<string>("SerialName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("SerialName", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("SerialCategories");

                    b.HasData(
                        new
                        {
                            SerialName = "Peaky Blinders",
                            CategoryId = 1
                        },
                        new
                        {
                            SerialName = "Peaky Blinders",
                            CategoryId = 2
                        },
                        new
                        {
                            SerialName = "Peaky Blinders",
                            CategoryId = 3
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.SerialMark", b =>
                {
                    b.Property<string>("SerialName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SerialName", "UserEmail");

                    b.HasIndex("UserEmail");

                    b.ToTable("SerialMarks");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Email = "admin@novastream.api",
                            Id = 1,
                            PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"
                        });
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Episode", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Season", "Season")
                        .WithMany("Episodes")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.MovieCategory", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Category", "Category")
                        .WithMany("MovieCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NovaStream.Domain.Entities.Concrete.Movie", "Movie")
                        .WithMany("Categories")
                        .HasForeignKey("MovieName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.MovieMark", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Movie", "Movie")
                        .WithMany("Marks")
                        .HasForeignKey("MovieName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NovaStream.Domain.Entities.Concrete.User", "User")
                        .WithMany("MovieMarks")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Season", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Serial", "Serial")
                        .WithMany("Seasons")
                        .HasForeignKey("SerialName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Serial");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.SerialCategory", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Category", "Category")
                        .WithMany("SerialCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NovaStream.Domain.Entities.Concrete.Serial", "Serial")
                        .WithMany("Categories")
                        .HasForeignKey("SerialName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Serial");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.SerialMark", b =>
                {
                    b.HasOne("NovaStream.Domain.Entities.Concrete.Serial", "Serial")
                        .WithMany("Marks")
                        .HasForeignKey("SerialName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NovaStream.Domain.Entities.Concrete.User", "User")
                        .WithMany("SerialMarks")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Serial");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Category", b =>
                {
                    b.Navigation("MovieCategories");

                    b.Navigation("SerialCategories");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Movie", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Marks");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Season", b =>
                {
                    b.Navigation("Episodes");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.Serial", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Marks");

                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("NovaStream.Domain.Entities.Concrete.User", b =>
                {
                    b.Navigation("MovieMarks");

                    b.Navigation("SerialMarks");
                });
#pragma warning restore 612, 618
        }
    }
}
