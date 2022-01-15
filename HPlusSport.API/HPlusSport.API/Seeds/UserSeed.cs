using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;
using System;
using HPlusSport.API.Models;

namespace HPlusSport.API.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
              new User { Id = 1, Email = "adam@example.com" },
                new User { Id = 2, Email = "barbara@example.com" }

                );
        }
    }
}
