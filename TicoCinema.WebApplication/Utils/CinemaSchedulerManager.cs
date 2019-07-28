using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Utils
{
    public static class CinemaSchedulerManager
    {
        private static Entities db = new Entities();

        public static void SaveCinemaScheduler(CinemaScheduleViewModel cinemaSchedule)
        {
            Movie movie = db.Movie.Find(cinemaSchedule.MovieId);

            if (movie != null)
            {
                List<DateTime> dateTimes = GenerateDatetimes(cinemaSchedule.BeginDatetime, cinemaSchedule.FinishDatetime,
                    cinemaSchedule.BeginTime, (int)cinemaSchedule.HoursRange);

                IList<CinemaSchedule> cinemaSchedules;
                cinemaSchedules = GenerateCinemaSchedules(cinemaSchedule, dateTimes, (int)movie.DurationTime.TotalMinutes);
                db.CinemaSchedule.AddRange(cinemaSchedules);
                db.SaveChanges();
            }
        }

        public static IList<CinemaSchedule> GenerateCinemaSchedules(int movieId, string beginDate, string finishDate, string beginTime, string hoursRange)
        {
            DateTime.TryParse(beginDate, out DateTime beginDatetime);
            DateTime.TryParse(finishDate, out DateTime finishDatetime);
            TimeSpan.TryParse(beginTime, out TimeSpan beginTimeSpan);
            Enum.TryParse(hoursRange, out Enums.HoursRange rangeEnum);

            IList<CinemaSchedule> cinemaSchedules = new List<CinemaSchedule>();

            Movie movie = db.Movie.Find(movieId);
            if (movie != null)
            {
                List<DateTime> dateTimes = GenerateDatetimes(beginDatetime, finishDatetime, beginTimeSpan, (int)rangeEnum);
                cinemaSchedules = GenerateCinemaSchedules(new CinemaScheduleViewModel(), dateTimes, (int)movie.DurationTime.TotalMinutes);
            }

            return cinemaSchedules;
        }

        private static List<DateTime> GenerateDatetimes(DateTime beginDate, DateTime finishDate, TimeSpan beginTime, int hoursRange)
        {
            int daysRange = (finishDate - beginDate).Days;
            List<DateTime> dateTimes = new List<DateTime>();

            while (daysRange > 0)
            {
                dateTimes.Add(beginDate + beginTime);

                if (hoursRange > 0)
                {
                    int dayTime = beginDate.Day;
                    DateTime currentDayTime = (beginDate + beginTime).AddHours(hoursRange);
                    while (currentDayTime.Day == dayTime)
                    {
                        dateTimes.Add(currentDayTime);
                        currentDayTime = currentDayTime.AddHours(hoursRange);
                    }
                }

                beginDate = beginDate.AddDays(1);
                daysRange--;
            }

            return dateTimes;
        }

        private static IList<CinemaSchedule> GenerateCinemaSchedules(CinemaScheduleViewModel cinemaSchedule, List<DateTime> dates, int durationMovie)
        {
            IList<CinemaSchedule> cinemaSchedules = new List<CinemaSchedule>();
            foreach (var date in dates)
            {
                CinemaSchedule scheduler = new CinemaSchedule()
                {
                    BeginDatetime = date,
                    CinemaId = cinemaSchedule.CinemaId,
                    MovieFormatId = cinemaSchedule.MovieFormatId,
                    MovieId = cinemaSchedule.MovieId
                };
                scheduler.FinishDatetime = scheduler.BeginDatetime.AddMinutes(durationMovie);
                cinemaSchedules.Add(scheduler);
            }

            return cinemaSchedules;
        }
    }
}