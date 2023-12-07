using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TopicController : Controller
    {
        private readonly TranningDBContext _dbContext;

        public TopicController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            TopicModel topicModel = new TopicModel();
            topicModel.TopicDetailLists = new List<TopicDetail>();

            var data = from m in _dbContext.Topic select m;

            data = data.Where(m => m.deleted_at == null);
            //if (!string.IsNullOrEmpty(SearchString))
            //{
            //    data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString));
            //}
            ////data.ToList();

            //foreach (var item in data)
            //{
            //    topicModel.TopicDetailLists.Add(new TopicDetail
            //    {
            //        id = item.id,
            //        course_id = item.course_id,
            //        name = item.name,
            //        description = item.description,
            //        videos = item.videos,
            //        status = item.status,
            //        created_at = item.created_at,
            //        updated_at = item.updated_at
            //    });
            //}
            ViewData["CurrentFilter"] = SearchString;
            return View(topicModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            var categoryList = _dbContext.Categories
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = categoryList;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadFile(Photo);
                    var topicData = new Topic()
                    {
                        name = topic.name,
                        description = topic.description,
                        course_id = topic.course_id,
                        status = topic.status,
                        videos = uniqueFileName,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    _dbContext.Topic.Add(topicData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    TempData["saveStatus"] = false;
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error adding topic: {ex.Message}");
                    return RedirectToAction(nameof(ErrorPage), new { errorMessage = ex.Message });
                }
                return RedirectToAction(nameof(CategoryController.Index), "Topic");
            }

            var categoryList = _dbContext.Categories
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = categoryList;
            Console.WriteLine(ModelState.IsValid);
            return View(topic);
        }

        public IActionResult ErrorPage(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }


        private string UploadFile(IFormFile file)
        {
            string uniqueFileName;
            try
            {
                string pathUploadServer = "wwwroot\\uploads\\images";

                string fileName = file.FileName;
                fileName = Path.GetFileName(fileName);
                string uniqueStr = Guid.NewGuid().ToString(); // random tao ra cac ky tu khong trung lap
                // tao ra ten fil ko trung nhau
                fileName = uniqueStr + "-" + fileName;
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileName);
                var stream = new FileStream(uploadPath, FileMode.Create);
                file.CopyToAsync(stream);
                // lay lai ten anh de luu database sau nay
                uniqueFileName = fileName;
            }
            catch (Exception ex)
            {
                uniqueFileName = ex.Message.ToString();
            }
            return uniqueFileName;
        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.Courses.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.deleted_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = true;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteStatus"] = false;
                //return Ok(ex.Message);
            }
            return RedirectToAction(nameof(CategoryController.Index), "Topic");
        }
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            CourseDetail course = new CourseDetail();
            var data = _dbContext.Courses.Where(m => m.id == id).FirstOrDefault();
            if (data != null)
            {
                course.id = data.id;
                course.name = data.name;
                course.description = data.description;
                course.status = data.status;
                course.avatar = data.avatar;
            }
            var categoryList = _dbContext.Categories.Where(m => m.deleted_at == null).Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = categoryList;
            return View(course);
        }
        [HttpPost]
        public IActionResult Update(TopicDetail topic, IFormFile Photo)
        {
            try
            {
                var data = _dbContext.Courses.Where(m => m.id == topic.id).FirstOrDefault();
                if (data != null)
                {
                    data.name = topic.name;
                    data.description = topic.description;
                    data.status = topic.status;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (Photo != null)
                    {
                        string uniqueIconAvatar = UploadFile(Photo);
                        data.avatar = uniqueIconAvatar;
                    }

                    _dbContext.SaveChanges(true);
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch
            {
                TempData["UpdateStatus"] = false;
            }
            return RedirectToAction(nameof(CategoryController.Index), "Topic");
        }
    }
}
