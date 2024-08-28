﻿namespace ParkingAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }

    }
}
