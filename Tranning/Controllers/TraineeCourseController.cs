using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TraineeCourseController : Controller
    {
        private readonly TranningDBContext _dbContext;
        public TraineeCourseController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            TraineeCourseModel traineecourseModel = new TraineeCourseModel();
            traineecourseModel.TraineeCourseDetailLists = new List<TraineeCourseDetail>();

            var data = _dbContext.TraineeCourses
                           .Where(m => m.deleted_at == null)
                           .ToList();

            foreach (var item in data)
            {
                var traineeList = _dbContext.Users
                                        .Where(m => m.id == item.userid && m.deleted_at == null)
                                        .FirstOrDefault();
                var courseList = _dbContext.Courses
                                        .Where(m => m.id == item.courseid && m.deleted_at == null)
                                        .FirstOrDefault();

                traineecourseModel.TraineeCourseDetailLists.Add(new TraineeCourseDetail
                {
                    id = item.id,
                    courseid = item.courseid,
                    courseName = courseList?.name, // Use ?. to avoid null reference exception
                    userid = item.userid,
                    traineeName = traineeList?.full_name, // Use ?. to avoid null reference exception
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }

            ViewData["CurrentFilter"] = SearchString;
            return View(traineecourseModel);
        }



        [HttpGet]
        public IActionResult Add()
        {
            TraineeCourseDetail traineecourse = new TraineeCourseDetail();
            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;

            return View(traineecourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TraineeCourseDetail traineecourse)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var traineecourseData = new TraineeCourse()
                    {

                        courseid = traineecourse.courseid,
                        userid = traineecourse.userid,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    _dbContext.TraineeCourses.Add(traineecourseData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    return Ok(new { Status = "Error", Message = ex.Message });

                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourse");
            }


            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;


            Console.WriteLine(ModelState.IsValid);
            foreach (var key in ModelState.Keys)
            {
                var error = ModelState[key].Errors.FirstOrDefault();
                if (error != null)
                {
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }
            return View(traineecourse);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.TraineeCourses.FirstOrDefault(m => m.id == id);

                if (data != null)
                {
                    // Soft delete by updating the deleted_at field
                    data.deleted_at = DateTime.Now;
                    _dbContext.SaveChanges();
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteStatus"] = false;
                // Log the exception if needed: _logger.LogError(ex, "An error occurred while deleting the topic.");
            }

            return RedirectToAction(nameof(Index), new { SearchString = "" });
        }

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            TraineeCourseDetail traineecourse = new TraineeCourseDetail();
            var data = _dbContext.TraineeCourses.Where(m => m.id == id).FirstOrDefault();

            if (data != null)
            {
                traineecourse.id = data.id;
                traineecourse.userid = data.userid;
                traineecourse.courseid = data.courseid;
            }

            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();

            ViewBag.Stores = new SelectList(courseList, "Value", "Text", data?.courseid);

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();

            ViewBag.Stores1 = new SelectList(traineeList, "Value", "Text", data?.userid);

            return View(traineecourse);
        }


        [HttpPost]
        public IActionResult Update(TraineeCourseDetail traineecourse)
        {
            
            try
            {
                var data = _dbContext.TraineeCourses.Where(m => m.id == traineecourse.id).FirstOrDefault();

                if (data != null)
                {
                    Console.WriteLine(traineecourse.courseid.ToString());

                    Console.WriteLine(traineecourse.userid.ToString());
                    data.courseid = traineecourse.courseid;
                    data.userid = traineecourse.userid;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["UpdateStatus"] = true;

                }
                else
                {
                    Console.WriteLine(traineecourse.id.ToString());
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(traineecourse.id.ToString());
                TempData["UpdateStatus"] = false;
                return Ok(new { Status = "Error", Message = ex.Message });
            }
            return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourse");

        }
    }
}
