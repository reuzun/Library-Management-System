using System;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Test.MockData
{
    public class MockUsersData
    {
        private static Guid guid = new Guid();

        public static User user = new User {
            UserId = guid,
            Username = "Test_Username",
            Email = "Test_Email",
        };

        public static UserDTO userToAdd = new UserDTO
        {
            Username = "Test_Username",
            Email = "Test_Email",
        };
    }
}

