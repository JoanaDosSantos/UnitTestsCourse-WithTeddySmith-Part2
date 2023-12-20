using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Repository
{
    public class ClubRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Clubs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                        new Club()
                        {
                            Title = "Running Club 1",
                            Image = "https://br.pinterest.com/pin/844636105114938558/",
                            Description = "This is the description of the first No",
                            ClubCategory = Data.Enum.ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "Rua da sua irmã",
                                City = "Quinto dos infernos",
                                State = "Casa da sua mãe"
                            }
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void ClubRepository_Add_ReturnsBool()
        {
            //Arrange
            var club = new Club()
            {
                Title = "Running Club 1",
                Image = "https://br.pinterest.com/pin/844636105114938558/",
                Description = "This is the description of the first No",
                ClubCategory = Data.Enum.ClubCategory.City,
                Address = new Address()
                {
                    Street = "Rua da sua irmã",
                    City = "Quinto dos infernos",
                    State = "Casa da sua mãe"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.Add(club);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }

    }
}
