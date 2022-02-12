using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotelRoomBooking;
using MailKit.Net.Smtp;
using MailKit.Security;
//using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace HotelBookingMVC.Controllers
{
    public class HotelBookingController : Controller
    {
        Class1 cls=new Class1();
        // GET: HotelBooking
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string myCountry, DateTime CheckIn, DateTime CheckOut)
        {
            Session["arrival"] = CheckIn;
            Session["Departure"] = CheckOut;
            ViewBag.Country = myCountry;    
            return View(cls.Search(myCountry));
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string emailId, string cpassword)
        {
            if(cls.Login(emailId, cpassword))
            {
                Session["uname"] = emailId;
                return RedirectToAction("Booking");
            }
            else
            {                              
                ViewBag.Msg = "Please Login If you want to book a room ";
                return View();
            }            
        }
        public ActionResult ForgotPwd()
        {            
                return View();           
        }
        [HttpPost]
        public ActionResult ForgotPwd(string emailId,string cpassword,string cpwd)
        {
          if (!cls.Forgot(emailId, cpassword, cpwd))
            {
                ViewBag.msg = "Your search did not return any results. Please try again with other information.";
                return View();
            }
            else
            {
                ViewBag.msg = "Password updated successfully";
                return View();
            }            
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(customer c,string cpwd)
        {
            if (c.cpassword==cpwd)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        string result = cls.Registration(c);
                        try
                        {
                            int count = int.Parse(result);
                            if (count > 0)
                            {
                                return RedirectToAction("Login");
                            }
                        }catch(FormatException e)
                        {
                            ViewData["Message"] = result;
                        }
                       
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                           ViewBag.test=("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                ViewBag.test1=(ve.PropertyName, ve.ErrorMessage);
                            }
                        }                     
                    }
                }
            }
            return View();
        }
        public ActionResult Profile()
        {
            return View(cls.Profile((string)Session["uname"]));
        }
        public ActionResult EditProfile(string emailId)
        {
            if (cls.Profile(emailId) != null)
            {
                 return View(cls.Profile((string)Session["uname"]).FirstOrDefault());
            }
            else
            {
                ViewData["message"] = "Please Register before doing changes";
                return View();
            }
        }
        [HttpPost]
        public ActionResult EditProfile(customer c)
        {
            if (ModelState.IsValid)
            {
                if (cls.EditProfile(c) > 0)
                {
                    ViewData["message"] = "Profile Updated successfully";                   
                }
                return View();
            }
            else
            {
                ViewData["message"] = "Records are not updated";
                return View();
            }            
        }
        public ActionResult Logout()
        {
            return View();
        }       
        public ActionResult BookingHistory()
        {
            if (Session["uname"] != null)
            {
                var res = cls.Display(Session["uname"].ToString());
                return View(res);
            }
            else
            {
                ViewData["message"] = "please login";
                return View();
            }
            
        }
       
        public ActionResult CancelBooking(BookingHistory bookingHistory)
        {
             List<BookingHistory> l = new List<BookingHistory>();
                l.Add(bookingHistory);
                return View(l);                     
        }
        [HttpPost]
        public ActionResult CancelBooking(string BhId,string check)
        {
            if (check == "yes" )
            {
                try
                {
                    if (cls.CancelBooking(int.Parse(BhId)) > 0)
                    {
                        ViewData["Message"] = "Booking Canceled successfully";
                        return View();
                    }
                   }catch(ArgumentNullException e)
                {
                    ViewData["message"] = "First select Room from Booking History";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("BookingHistory");
            }           
            return View();
        }
    //  Booking b;
         BookingHistory bH = new BookingHistory();
        public ActionResult BookingRegistration(HMap h)
        {
            Session["price"] = h.price;
            bH.HotelName = h.HotelName;
            bH.RoomType = h.RoomType;
            /*if(Session["arrival"]!=null || Session["Departure"] != null)
            {
                b.Arrival_DateTime = (DateTime)Session["arrival"];
                b.Departure_DateTime = (DateTime)Session["Departure"];
            }*/
            return View(h);
        }
        [HttpPost]
        public ActionResult BookingRegistration(Booking b, string NoOfRoom)
        {
            /*   for(int i = 0; i < b.No_Of_People; i++)
               {

               } */
            if (Session["uname"] != null)
            {
                b.BDate = DateTime.Now;
                bH.BDate = b.BDate;
                bH.emailId = Session["uname"].ToString();
                bH.Rid = b.Rid;
                bH.NoOfRooms = int.Parse(NoOfRoom);
                bH.Arrival = b.Arrival_DateTime;
                bH.Departure = b.Departure_DateTime;
                b.emailId = (string)Session["uname"];
                if (ModelState.IsValid)
                {
                    if (cls.Booking(b) > 0)
                    {
                        Session["Rid"] = b.Rid;
                        Session["price"] = int.Parse(Session["price"].ToString()) * int.Parse(NoOfRoom);
                        bH.Amount = int.Parse(Session["price"].ToString());
                        if (cls.History(bH) > 0)
                            return RedirectToAction("PaymentMethods");
                    }
                }
            }
            else
            {
                ViewData["success"] = "Please Login Before Reserve a room";
            }
            return View();
        }

        public ActionResult AddMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMember(AddMember s1)
        {
            if (ModelState.IsValid)
            {
                ViewData["d"] = cls.Addmember(s1);
            }
            return View();
        }
        public ActionResult PaymentPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PaymentPage(Payment card)
        {
            string res= cls.Paymentpage(card, int.Parse(Convert.ToString(Session["price"])));
            try
            {
                if (int.Parse(res) > 0)
                {
                    return RedirectToAction("Receipt");
                }
            }catch(Exception e)
            {
                ViewData["Payment"] = res;
            return View();
            }
            return View();
        }
        public ActionResult Upi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upi(UPI upi)
        {
            string res= cls.Upi(upi, int.Parse(Session["price"].ToString()));
            try
            {
                if (int.Parse(res) > 0)
                {
                    return RedirectToAction("Receipt");
                }
            }catch (FormatException ex)
            {
                ViewData["Upi"] = res;
            }
            return View();
        }
        public ActionResult Booking(Hotel h)
        {
            ViewBag.msg = h.HotelName;
            ViewBag.address=h.HAddress;         
            return View(cls.SelectHotel(h));
        }
        public ActionResult Room(HMap rm)
        {
            HashSet<price> priceList = new HashSet<price>();
            priceList=cls.Facility();
            ViewBag.room = priceList;
            if (cls.CheckAvailabaility(rm.Rid))
            {
                return View(rm);
            }
            else
            {
                ViewData["message"] = "Sorry this room is already booked";
                return View();
            }
            
        }
        public ActionResult PaymentMethods()
        {
            return View();
        }        
        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(FeedBack feedBack)
        {
            if(Session["uname"]!=null)
            {
                feedBack.emailId = (string)Session["uname"];
                if (ModelState.IsValid)
                {
                    Stack<FeedBack> f = cls.Reviews(feedBack);
                    if (f.Count() > 0)
                    {
                        return View(f);
                    }
                    else
                    {
                        ViewData["message"] = "You already given feedback";
                        return View();
                    }
                }
            }
            else
            {
                ViewData["message"] = "Please Login to give your valuable feedback";
            }
            return View();
        }
        //Receipt
        public ActionResult Receipt()
        {
            if (Session["rid"] != null)
            {
                return View(cls.Receipt(int.Parse(Session["rid"].ToString())));
            }
            else
            {
                ViewData["message"] = "Please do Booking Registration";
                return View();
            }            
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(string Query, string emailId, string pwd)
        {
            if(ModelState.IsValid)
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("hotelroombooking166@gmail.com");
                email.To.Add(MailboxAddress.Parse("hotelroombooking166@gmail.com"));
                email.Subject = "User Queries";
                email.Body = new TextPart(TextFormat.Html) { Text = Query };

                var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                try
                {
                    smtp.Authenticate(emailId, pwd);
                    smtp.Send(email);
                }
                catch (AuthenticationException e)
                {
                    ViewData["message"] = "Incorrect Credentials";
                    return View();
                }
                smtp.Disconnect(true);
                try
                {
                    if (cls.Contact(emailId, Query) > 0)
                    {
                        ViewData["message"] = "We received Your Message";
                        return View();
                    }
                    else
                    {
                        ViewData["message"] = "Something went wrong";
                        return View();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {                       
                        foreach (var ve in eve.ValidationErrors)
                        {
                            ViewBag.test1 = ve.ErrorMessage;
                        }
                    }
                }
            }
            return View();                   
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        //Admin Login
      //  [Authorize(Users = "DESKTOP-N77EPKE//Gowthami Gorthi")]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(string admin, string cpassword)
        {
            if (cls.AdminLogin(admin, cpassword))
            {
                Session["uname"] = admin;
                return RedirectToAction("Home");
            }
            else
            {
                ViewBag.Msg = "Please Login If you want to check details ";
                return View();
            }           
        }
        public ActionResult ForgotAdmin(string admin,string cpassword,string cpwd)
        {
            if (!cls.ForgotAdmin(admin, cpassword, cpwd))
            {
                ViewBag.msg = "Your search did not return any results. Please try again with other information.";
            }
            return View();
        }
        public ActionResult UserDetails()
        {            
            return View(cls.UserDetails());
        }
        public ActionResult BookingDetails()
        {
            return View(cls.BookingHistory());
        }
        public ActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Home(string check)
        {
            if(check == "Hotel")
            {
                ViewData["msg"] = "hotel";
                return RedirectToAction("Hotel");
            }
            else
            {
                return RedirectToAction("Home");
            }           
        }
        public ActionResult Hotel()
        {
            return View(cls.Hotels());
        }
        public ActionResult Hcreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Hcreate(Hotel h)
        {
            if (ModelState.IsValid)
            {
                if (cls.Hcreate(h) > 0)
                {
                    ViewData["message"] = "Hotel details added successfully";
                }
                else
                {
                    ViewData["message"] = "Hotel details are not added";
                }
            }
            return View();
        }
        public ActionResult HEdit(string id)
        {
            return View(cls.HEdit(id));
        }
        [HttpPost]
        public ActionResult HEdit(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                if (cls.HEdited(hotel) > 0)
                {
                    ViewData["message"] = "Record Inserted Successfully";
                    return RedirectToAction("Hotel");
                }
                    return View();
            }
            else
            {
                ViewData["message"] = "Something went wrong";
                return View();
            }            
        }
       
        public ActionResult HDelete(string id)
        {
              if (cls.HDeleted(id) > 0)
                {
                    ViewData["message"] = "Record deleted Successfully";
                    return RedirectToAction("Hotel");
                }
                return View();         
        }
        public ActionResult HDetails(string id)
        {
            return View(cls.HDetails(id));
        }
        public ActionResult Rcreate(string id)
        {
            ViewBag.HotelName= id;
            return View();
        }
        [HttpPost]
        public ActionResult Rcreate(HMap hMap)
        {
            if (ModelState.IsValid)
            {
                if (cls.Rcreate(hMap) > 0)
                {
                    ViewData["message"] = "Room details added successfully";
                }
                else
                {
                    ViewData["message"] = "Room details are not added";
                }
            }
            return View();
        }
        public ActionResult REdit(int id)
        {
            return View(cls.REdit(id));
        }
        [HttpPost]
        public ActionResult REdit(HMap hMap)
        {
            if (ModelState.IsValid)
            {
                if (cls.REdited(hMap) > 0)
                {
                    ViewData["message"] = "Record Inserted Successfully";
                    return RedirectToAction("HDetails");
                }
                return View();
            }
            else
            {
                ViewData["message"] = "Something went wrong";
                return View();
            }
        }
        public ActionResult RDelete(int id)
        {
            if (cls.RDeleted(id) > 0)
            {
                ViewData["message"] = "Record deleted Successfully";
                return RedirectToAction("HDetails");
            }
            else
            {
                ViewData["message"] = "Something went wrong";
                return View();
            }
        }
        public ActionResult RDetails(int id)
        {
            return View(cls.RDetails(id));
        }
        public ActionResult UserQuery()
        {
            return View(cls.UserQueries());
        }
    }
}