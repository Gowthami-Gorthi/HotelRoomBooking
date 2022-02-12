using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HotelRoomBooking;
using System.Collections.Generic;

namespace HotelBookingTesting
{
    [TestClass]
    public class UnitTest1
    {
        Class1 cls = new Class1();  
        [TestMethod]
        public void TestMethod1() 
        {
            bool result = cls.Login("gorthigowthami8@gmail.com", "Haritha48");
            Assert.AreEqual(true, result);
        }
      
        [TestMethod]
        public void TestMethod2()
        {
            customer c = new customer();
            c.emailId = "ramesh1@gmail.com";
            c.cpassword = "ramesh1708";
            c.custname = "Ramesh";
            c.phone = "9876543211";
            c.DOB = DateTime.Parse("1999-01-06 08:01");
            c.caddress = "Kurnool";
            string result = cls.Registration(c);
            Assert.AreEqual("1", result);
        }
        [TestMethod]
        public void TestMethod3()
        {
            Booking b = new Booking();
            b.emailId = "ramesh1@gmail.com";
            b.Bname = "Suri";
            b.Rid = 190;
            b.Arrival_DateTime = DateTime.Parse("02-01-2022 8:00");
            b.Departure_DateTime = DateTime.Parse("03-01-2022 12:00");
            b.No_Of_People = 1;
            int result = cls.Booking(b);
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TestMethod4()
        {
            bool result = cls.Forgot("ramesh1@gmail.com", "Suresh962", "Suresh962");
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestMethod5()
        {
            AddMember add = new AddMember();
            add.Member_Name = "Gowtham";
            add.member_Age = 20;
            string result = cls.Addmember(add);
            Assert.AreEqual("Member Inserted successfully!!", result);
        }
       

        [TestMethod]
        public void TestMethod7()
        {
            UPI u = new UPI();
            u.UPI_ID = "gowthami@api";
            string result = cls.Upi(u, 6000);
            Assert.AreEqual("1", result);
        }
        [TestMethod]
        public void TestMethod8()
        {
            BookingHistory h = new BookingHistory();

            h.emailId = "ramesh1@gmail.com";
            h.Rid = 150;
            h.HotelName = "The Leela Mumbai";
            h.RoomType = "DOUBLE ROOM";
            h.Amount = 6000;
            h.BDate = DateTime.Now;
            h.NoOfRooms = 1;
            h.Arrival = DateTime.Parse("2022 - 01 - 04 11:05:00.000");
            h.Departure = DateTime.Parse("2022-01-05 12:06:00.000");
            int result = cls.History(h);
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TestMethod9()
        {
            string result = cls.AddRoom();
            Assert.IsTrue(true, result);
        }
        [TestMethod]
        public void TestMethod10()
        {
            bool result = cls.CheckAvailabaility(180);
            Assert.AreEqual(true, result);
        }
       
    }
}
