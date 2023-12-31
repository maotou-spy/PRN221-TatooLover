﻿using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class BookingDAO
    {
        Prn221TatooLoverContext db = new Prn221TatooLoverContext();

        public List<Booking> GetBookingInDayByArtistId(DateTime date, int id) => db.Bookings
            .Where(b => b.ArtistId == id && b.BookingDateTime.Date == date.Date)
            .Include(b => b.Customer)
            .Include(c => c.Artist)
            .Include(s => s.Service)
            .ToList();
        public List<Booking> GetBookingByArtistId(int id) => db.Bookings
            .Where(b => b.ArtistId == id)
            .Include(b => b.Customer)
            .Include(c => c.Artist)
            .Include(b => b.Service)
            .ToList();
        public List<Booking> GetBookingInDayByStudioId(DateTime date, int studioId) => db.Bookings
            .Where(b => b.Artist.StudioId == studioId && b.BookingDateTime.Date == date.Date)
            .Include(b => b.Customer)
            .Include(c => c.Artist)
            .Include(b => b.Service)
            .Include(b => b.Bills)
            .ToList();
        public List<Booking> GetBookingByStudioId(int studioId) => db.Bookings
            .Where(s => s.Artist.StudioId == studioId)
            .Include(b => b.Customer)
            .Include(c => c.Artist)
            .Include (b => b.Service)
            .OrderByDescending(b => b.BookingDateTime)
            .ToList();
        

        public static Booking? GetBookingById(int? id)
        {
            Booking? booking = new Booking();
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    booking = context.Bookings
                        .Include(c => c.Artist)
                        .Include(s => s.Service)
                        .Include(b => b.Customer)
                        .SingleOrDefault(f => f.BookingId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return booking;
        }
        public static List<Booking> GetBookings()
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    bookings = context.Bookings
                        .Include(c => c.Artist)
                        .Include(s => s.Service)
                        .Include(b => b.Customer)
                        .OrderByDescending(b => b.BookingDateTime)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }

        public static List<Booking> GetBookingsByCustomerId(int id)
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    bookings = context.Bookings
                        .Include(c => c.Artist)
                        .Include(s => s.Service)
                        .Include(b => b.Customer)
                        .Where(b => b.CustomerId == id)
                        .OrderByDescending(b => b.BookingDateTime)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }
        public static void SaveBooking(Booking booking)
        {
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    context.Bookings.Add(booking);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteBooking(Booking booking)
        {
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    var p1 = context.Bookings.SingleOrDefault(f => f.BookingId == booking.BookingId);
                    context.Bookings.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateBooking(Booking booking)
        {
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    context.Entry<Booking>(booking).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBookingPrice(int bookingId, double price)
        {
            try
            {
                using (var context = new Prn221TatooLoverContext())
                {
                    var booking = context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
                    if (booking != null)
                    {
                        booking.Price = price;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Only get bill of booking where booking status in (2;4] => after done
        public List<Bill> GetBillByStudioId(int id) => db.Bills
            .Where(b => b.Booking.Artist.Studio.StudioId == id)
            .Include(b => b.Booking)
            .Include (b => b.Booking.Customer)
            .Include(b => b.Booking.Artist)
            .Where(b => b.Booking.Status <= 4 && b.Booking.Status > 2)
            .OrderByDescending(b => b.Booking.BookingDateTime)
            .ToList();
    }
}
